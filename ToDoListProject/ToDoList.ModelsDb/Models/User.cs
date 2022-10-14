using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ToDoList.ModelsDB.Models;

#nullable disable

namespace ToDoList.ModelsDb.Models
{
    public partial class User
    {
        public User()
        {
            ToDos = new HashSet<ToDo>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string ImageUrl { get; set; } = "";
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsAdmin { get; set; }
        public bool Archived { get; set; }

        public virtual ICollection<ToDo> ToDos { get; set; }
    }
}
