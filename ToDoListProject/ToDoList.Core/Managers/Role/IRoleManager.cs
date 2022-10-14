using ToDoList.Core.Managers.Base;
using ToDoList.ModelViews.ModelViews.User;

namespace ToDoList.Core.Managers.Role
{
    public interface IRoleManager:IManager
    {
        bool CheckAccess(UserMV userMV);
    }
}
