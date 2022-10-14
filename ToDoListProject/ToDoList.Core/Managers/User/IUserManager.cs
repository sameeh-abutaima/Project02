using System.Collections.Generic;
using ToDoList.Core.Managers.Base;
using ToDoList.ModelViews.ModelViews.User;

namespace ToDoList.Core.Managers.User
{
    public interface IUserManager:IManager
    {
        List<UserMV> GetUsers();
        List<UserMV> GetArchivedUsers();
        UserMV UpdateProfile(UserMV currentUser, UserMV userMV);

        LoginResponseUserMV Login(LoginUserMV loginUserMV);

        SignUpResponseUserMv SignUp(SignUpUserMV signUpUserMV);

        void DeleteUser(UserMV currentUser, int id);
    }
}
