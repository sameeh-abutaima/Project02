using AutoMapper;
using ToDoList.Common.Extensions;
using ToDoList.ModelsDB.Models;
using ToDoList.ModelViews.ModelViews.ToDo;

namespace ToDoList.Core.Mappers
{
    public class ToDoMapping:Profile
    {
        public ToDoMapping()
        {
            CreateMap<AddToDoMV, ToDo>().ReverseMap();
            CreateMap<ToDoMV, ToDo>().ReverseMap();
            CreateMap<ToDoMV, ToDo>().ReverseMap();
            CreateMap<PagedResult<ToDoMV>, PagedResult<ToDo>>().ReverseMap();
        }
    }
}
