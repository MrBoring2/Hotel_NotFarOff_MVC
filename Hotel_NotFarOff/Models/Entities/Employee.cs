using System;
using System.Collections.Generic;

namespace Hotel_NotFarOff.Models.Entities
{
    public partial class Employee
    {
        public Employee()
        {
            Rooms = new HashSet<Room>();
        }

        public int Id { get; set; }
        public string FullName { get; set; }
        public string Passport { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int PostId { get; set; }
        public string Gender { get; set; }

        public virtual Post Post { get; set; }
        public virtual SiteProfle SiteProfle { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
    }
}
