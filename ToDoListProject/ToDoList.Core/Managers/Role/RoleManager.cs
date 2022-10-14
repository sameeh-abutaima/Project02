using AutoMapper;
using System.Linq;
using ToDoList.ModelsDB.Models;
using ToDoList.ModelViews.ModelViews.User;

namespace ToDoList.Core.Managers.Role
{
    public class RoleManager : IRoleManager
    {
        #region Dependency Injections
        private readonly ToDoListDbContext _context;
        private readonly IMapper _mapper;
        public RoleManager(ToDoListDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion Dependency Injections 

        #region Public Methods
        public bool CheckAccess(UserMV userMV)
        {
            var isAdmin = _context.Users
                .Any(usr => usr.Id == userMV.Id && usr.IsAdmin);
            return isAdmin;
        }
        #endregion Public Methods
    }
}
