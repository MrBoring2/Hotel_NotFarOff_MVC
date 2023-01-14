using Hotel_NotFarOff.Models;
using Hotel_NotFarOff.Models.Entities;
using System.Collections.Generic;

namespace Hotel_NotFarOff.ViewModels
{
    public class RoomCategoryViewModel
    {
        public RoomCategory RoomCategory { get; private set; }

        public RoomCategoryViewModel(RoomCategory roomCategory)
        {
            RoomCategory = roomCategory;
            RoomCategoryId = RoomCategory.Id;
        }
        public string PricePerDayString => RoomCategory.PricePerDay.ToString("0.00") + " ₽";
        public string Message { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int RoomCategoryId { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
