using System.Collections.Generic;
using ToDoList.Common.Extensions;
using ToDoList.ModelViews.ModelViews.User;

namespace ToDoList.ModelViews.ModelViews.ToDo
{
    public class GetAllTasksResponse
    {
        public PagedResult<ToDoMV> Tasks { get; set; }

        public Dictionary<int, GetUserMV> Creators { get; set; }
        public Dictionary<int, GetUserMV> Users { get; set; }

    }
}
