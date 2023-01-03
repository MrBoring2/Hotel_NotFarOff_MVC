using System;
using System.Collections.Generic;

namespace Hotel_NotFarOff.Models.Entities
{
    public partial class BookingStatus
    {
        public BookingStatus()
        {
            Bookings = new HashSet<Booking>();
        }

        public int Id { get; set; }
        public string Title { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
