using System.Collections.Generic;

namespace Hotel_NotFarOff.Models.Entities
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; } 
    }
}
