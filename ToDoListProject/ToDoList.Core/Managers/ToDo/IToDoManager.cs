using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.Managers.Base;
using ToDoList.ModelViews.ModelViews.ToDo;
using ToDoList.ModelViews.ModelViews.User;

namespace ToDoList.Core.Managers.ToDo
{
    public interface IToDoManager: IManager
    {
        GetAllTasksResponse GetMyTasks(int loggedInUserId, int page = 1, int pageSize = 10, string sortColumn = "", string sortDirection = "ascending", string searchText = "");
        GetAllTasksResponse GetAllTasks(int page = 1, int pageSize = 10, string sortColumn = "", string sortDirection = "ascending", string searchText = "");
        ToDoMV AddTask(UserMV loggedInUser, AddToDoMV addToDoMV);
        ToDoMV UpdateTask(int loggedInUserId,UpdateToDoMV updateToDoMV);
        void DeleteTask(int loggedInUserId,int id);
        void HasRead(int loggedInUserId,int id);
        GetAllTasksResponse GetArchivedTasks(int page = 1, int pageSize = 10, string sortColumn = "", string sortDirection = "ascending", string searchText = "");
    }
}
