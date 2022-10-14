using AutoMapper;
using ToDoList.ModelsDb.Models;
using ToDoList.ModelViews.ModelViews.User;

namespace ToDoList.Core.Mappers
{
    public class UserMapping:Profile
    {
        public UserMapping()
        {
            CreateMap<UserMV, User>().ReverseMap();
            CreateMap<GetUserMV, User>().ReverseMap();
            CreateMap<SignUpUserMV, User>().ReverseMap();
            CreateMap<SignUpResponseUserMv, User>().ReverseMap();
            CreateMap<LoginResponseUserMV, User>().ReverseMap();
        }
    }
}
