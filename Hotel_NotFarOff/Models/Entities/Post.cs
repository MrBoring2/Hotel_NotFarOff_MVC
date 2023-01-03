using System;
using System.Collections.Generic;

namespace Hotel_NotFarOff.Models.Entities
{
    public partial class Post
    {
        public Post()
        {
            Employees = new HashSet<Employee>();
        }

        public int Id { get; set; }
        public string Title { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
