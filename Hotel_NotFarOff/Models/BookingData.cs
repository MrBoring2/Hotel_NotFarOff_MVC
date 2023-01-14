using Hotel_NotFarOff.Validation.ValidationAttributes;
using Microsoft.Build.Framework;
using System;

namespace Hotel_NotFarOff.Models
{
    [CheckOutAboveCheckIn]
    public class BookingData
    {
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
        public int RoomCategoryId { get; set; }

        public BookingData()
        {
        }

        public BookingData(DateTime checkIn, DateTime checkOut, int adultCount, int childCount, int roomCategoryId)
        {
            CheckIn = checkIn;
            CheckOut = checkOut;
            AdultCount = adultCount;
            ChildCount = childCount;
            RoomCategoryId = roomCategoryId;
        }
    }
}
