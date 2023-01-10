using System;

namespace Hotel_NotFarOff.Models
{
    public class BookingData
    {
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
        public int RoomCategoryId { get; set; }
    }
}
