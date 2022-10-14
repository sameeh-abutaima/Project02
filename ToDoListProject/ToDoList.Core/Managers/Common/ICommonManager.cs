using ToDoList.Core.Managers.Base;
using ToDoList.ModelViews.ModelViews.User;

namespace ToDoList.Core.Managers.Common
{
    public interface ICommonManager:IManager
    {
        UserMV GetUserRole(UserMV userMV);
    }
}
