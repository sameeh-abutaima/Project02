using AutoMapper;
using System.Linq;
using ToDoList.Common.Exceptions;
using ToDoList.ModelsDB.Models;
using ToDoList.ModelViews.ModelViews.User;

namespace ToDoList.Core.Managers.Common
{
    public class CommonManager : ICommonManager
    {
        #region Dependency Injections
        private readonly ToDoListDbContext _context;
        private readonly IMapper _mapper;
        public CommonManager(ToDoListDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion Dependency Injections

        #region Public Methods
        public UserMV GetUserRole(UserMV userMV)
        {
            var user = _context.Users
                .FirstOrDefault(usr => usr.Id == userMV.Id)
                ?? throw new ServiceValidationException("Invalid UserId Received!!");
            return _mapper.Map<UserMV>(user);
        }
        #endregion Public Methods
    }
}
