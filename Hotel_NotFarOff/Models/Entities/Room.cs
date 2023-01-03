using System;
using System.Collections.Generic;

namespace Hotel_NotFarOff.Models.Entities
{
    public partial class Room
    {
        public Room()
        {
            Bookings = new HashSet<Booking>();
        }

        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public int RoomCategoryId { get; set; }
        public int EmployeeId { get; set; }
        public bool IsBooked { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual RoomCategory RoomCategory { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
