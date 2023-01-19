using Hotel_NotFarOff.Models;
using Hotel_NotFarOff.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Hotel_NotFarOff.ViewModels
{
    public class RoomCategoryInListViewModel
    {
        public RoomCategory RoomCategory { get; private set; }

        public RoomCategoryInListViewModel(RoomCategory roomCategory)
        {
            RoomCategory = roomCategory;
            Services = string.Join(", ", RoomCategory.Services.Select(p => p.Title));
            RoomCategoryId = RoomCategory.Id;
        }
        public string PricePerDayString => RoomCategory.PricePerDay.ToString("0.00") + " ₽";
        public string Message { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int RoomCategoryId { get; set; }
        public string Services { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
