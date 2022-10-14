using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.ModelViews.ModelViews.ToDo
{
    public class UpdateToDoMV
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public string ImageString { get; set; }
        public int AssignedTo { get; set; }
    }
}
