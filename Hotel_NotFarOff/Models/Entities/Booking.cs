using System;
using System.Collections.Generic;

namespace Hotel_NotFarOff.Models.Entities
{
    public partial class Booking
    {
        public Booking()
        {
            Guests = new HashSet<Guest>();
        }

        public int Id { get; set; }
        public string TenantFio { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int RoomId { get; set; }
        public int BookingStatusId { get; set; }

        public virtual BookingStatus BookingStatus { get; set; }
        public virtual Room Room { get; set; }

        public virtual ICollection<Guest> Guests { get; set; }
    }
}
