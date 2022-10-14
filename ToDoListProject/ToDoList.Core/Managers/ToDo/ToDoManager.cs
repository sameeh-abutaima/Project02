using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using ToDoList.Common.Exceptions;
using ToDoList.Common.Extensions;
using ToDoList.Common.Helpers;
using ToDoList.ModelsDB.Models;
using ToDoList.ModelViews.ModelViews.ToDo;
using ToDoList.ModelViews.ModelViews.User;

namespace ToDoList.Core.Managers.ToDo
{
    public class ToDoManager : IToDoManager
    {
        #region Dependency Injections
        private readonly ToDoListDbContext _context;
        private readonly IMapper _mapper;
        public ToDoManager(ToDoListDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion Dependency Injections 

        #region Public Methods

        public GetAllTasksResponse GetMyTasks(int loggedInUserId, int page = 1, int pageSize = 10, string sortColumn = "", string sortDirection = "ascending", string searchText = "")
        {
            var tasks = _context.ToDoes
                                .Where(task => task.AssignedTo == loggedInUserId
                                                    && (string.IsNullOrWhiteSpace(searchText)
                                                    || (task.Title.Contains(searchText)
                                                    || task.Content.Contains(searchText))));
            if (!string.IsNullOrWhiteSpace(sortColumn) && sortDirection.Equals("ascending", StringComparison.InvariantCultureIgnoreCase))
            {
                tasks = tasks.OrderBy(sortColumn);
            }
            else if (!string.IsNullOrWhiteSpace(sortColumn) && sortDirection.Equals("descending", StringComparison.InvariantCultureIgnoreCase))
            {
                tasks = tasks.OrderByDescending(sortColumn);
            }

            var res = tasks.GetPaged(page, pageSize);

            var assignedByIds = res.Data
                             .Select(a => a.AssignedBy)
                             .Distinct()
                             .ToList();

            var assignedToIds = res.Data
                             .Select(a => a.AssignedTo)
                             .Distinct()
                             .ToList();

            var creators = _context.Users
                                     .Where(a => assignedByIds.Contains(a.Id))
                                     .ToDictionary(a => a.Id, x => _mapper.Map<GetUserMV>(x));
            var users = _context.Users
                                     .Where(a => assignedToIds.Contains(a.Id))
                                     .ToDictionary(a => a.Id, x => _mapper.Map<GetUserMV>(x));

            var data = new GetAllTasksResponse
            {
                Tasks = _mapper.Map<PagedResult<ToDoMV>>(res),
                Creators = creators,
                Users = users,
            };

            data.Tasks.Sortable.Add("Title", "Title");

            return data;
        }
        public GetAllTasksResponse GetAllTasks(int page = 1, int pageSize = 10, string sortColumn = "", string sortDirection = "ascending", string searchText = "")
        {
            var tasks = _context.ToDoes
                                .Where(task => string.IsNullOrWhiteSpace(searchText)
                                                    || (task.Title.Contains(searchText)
                                                        || task.Content.Contains(searchText)));
            if (!string.IsNullOrWhiteSpace(sortColumn) && sortDirection.Equals("ascending", StringComparison.InvariantCultureIgnoreCase))
            {
                tasks = tasks.OrderBy(sortColumn);
            }
            else if (!string.IsNullOrWhiteSpace(sortColumn) && sortDirection.Equals("descending", StringComparison.InvariantCultureIgnoreCase))
            {
                tasks = tasks.OrderByDescending(sortColumn);
            }

            var res = tasks.GetPaged(page, pageSize);

            var assignedByIds = res.Data
                             .Select(a => a.AssignedBy)
                             .Distinct()
                             .ToList();

            var assignedToIds = res.Data
                             .Select(a => a.AssignedTo)
                             .Distinct()
                             .ToList();

            var creators = _context.Users
                                     .Where(a => assignedByIds.Contains(a.Id))
                                     .ToDictionary(a => a.Id, x => _mapper.Map<GetUserMV>(x));
            var users = _context.Users
                                     .Where(a => assignedToIds.Contains(a.Id))
                                     .ToDictionary(a => a.Id, x => _mapper.Map<GetUserMV>(x));

            var data = new GetAllTasksResponse
            {
                Tasks = _mapper.Map<PagedResult<ToDoMV>>(res),
                Creators = creators,
                Users = users,
            };

            data.Tasks.Sortable.Add("Title", "Title");

            return data;

        }

        public ToDoMV AddTask(UserMV loggedInUser, AddToDoMV addToDoMV)
        {

            if (!loggedInUser.IsAdmin && addToDoMV.AssignedTo != loggedInUser.Id)
            {
                throw new ServiceValidationException("No Permissions");
            }

            var newToDo = _mapper.Map<ModelsDB.Models.ToDo>(addToDoMV);
            newToDo.AssignedBy = loggedInUser.Id;

            var url = "";
            if (!string.IsNullOrWhiteSpace(addToDoMV.ImageString))
            {
                url = Helper.SaveImage(addToDoMV.Title, addToDoMV.ImageString, "ToDoListImages");
            }

            if (!string.IsNullOrWhiteSpace(url))
            {
                var baseURL = "https://localhost:44324/";
                newToDo.ImageUrl = @$"{baseURL}/api/v1/toDo/retrieve?filename={url}";
            }

            var user = _context.ToDoes.Add(newToDo).Entity;
            _context.SaveChanges();

            var res = _mapper.Map<ToDoMV>(user);
            return res;
        }

        public ToDoMV UpdateTask(UpdateToDoMV updateToDoMV, int loggedInUserId)
        {
            var task = _context.ToDoes
                                    .FirstOrDefault(task => task.Id == updateToDoMV.Id
                                                            && task.AssignedBy == loggedInUserId)
                                    ?? throw new ServiceValidationException("Task not found");

            var url = "";

            if (!string.IsNullOrWhiteSpace(updateToDoMV.ImageString))
            {
                url = Helper.SaveImage(updateToDoMV.Title, updateToDoMV.ImageString, "ToDoListImages");
            }

            task.Title = updateToDoMV.Title;
            task.Content = updateToDoMV.Content;
            if (task.User.IsAdmin && task.AssignedTo != updateToDoMV.AssignedTo)
            {
                task.AssignedTo = updateToDoMV.AssignedTo;
            }

            if (!string.IsNullOrWhiteSpace(url))
            {
                var baseURL = "https://localhost:44309/";
                task.ImageUrl = @$"{baseURL}/api/v1/toDo/retrieve?filename={url}";
            }

            _context.SaveChanges();
            return _mapper.Map<ToDoMV>(task);
        }

        public void DeleteTask(int id, int loggedInUserId)
        {
            var task = _context.ToDoes
                            .FirstOrDefault(task => task.Id == id && task.AssignedBy == loggedInUserId)
                            ?? throw new ServiceValidationException("No Permission Or Not Founded");
            task.Archived = true;
            _context.SaveChanges();
        }

        public void HasRead(int id, int loggedInUserId)
        {
            var task = _context.ToDoes
                            .FirstOrDefault(task => task.Id == id && task.AssignedTo == loggedInUserId)
                            ?? throw new ServiceValidationException("No Permission Or Not Founded");
            task.IsRead = true;
            _context.SaveChanges();
        }

        public List<ToDoMV> GetArchivedTasks()
        {
            _context.IgnoreFilter = true;
            var tasks = _context.ToDoes.Where(task => task.Archived);
            var res = _mapper.Map<List<ToDoMV>>(tasks);
            _context.IgnoreFilter = false;
            return res;
        }

        #endregion Public Methods

        #region Private Methods
        //private static bool
        #endregion Private Methods

    }
}
