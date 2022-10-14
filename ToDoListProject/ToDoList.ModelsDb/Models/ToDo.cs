using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ToDoList.ModelsDb.Models;

#nullable disable

namespace ToDoList.ModelsDB.Models
{
    public partial class ToDo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int AssignedTo { get; set; }
        public int AssignedBy { get; set; }
        public bool IsRead { get; set; }
        public bool Archived { get; set; }

        [ForeignKey("AssignedBy")]
        public virtual User User { get; set; }
    }
}
