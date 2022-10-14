using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.ModelViews.ModelViews.ToDo
{
    public class ToDoMV
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public int AssignedTo { get; set; }
        public int AssignedBy { get; set; }
    }
}
