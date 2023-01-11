﻿using Hotel_NotFarOff.Models;
using Hotel_NotFarOff.Models.Entities;
using System;

namespace Hotel_NotFarOff.ViewModels
{
    public class BookingViewModel
    {
        public BookingData BookingData { get; set; }
        public int NightCount => (BookingData.CheckOut - BookingData.CheckIn).Days;
        public RoomCategory RoomCategory { get; set; }
        public string TenantFullName { get; set; }
        public string TenantPhone { get; set; }
        public string TenantEmail { get; set; }
        public string[] Guests { get; set; }
        public decimal TotalPrice => RoomCategory.PricePerDay * NightCount;
        public string DisplayNights
        {
            get
            {
                string nights = "";
                if ((NightCount % 100) > 10 && (NightCount % 100) < 20)
                    nights = "ночей";
                if (NightCount % 10 == 1)
                    nights = "ночь";
                if ((NightCount % 10 == 2) || (NightCount % 10 == 3) || (NightCount % 10 == 4))
                    nights = "ночи";
                return $"{NightCount} {nights}";
            }
        }

        public BookingViewModel(BookingData bookingData, RoomCategory roomCategory)
        {
            BookingData = bookingData;
            RoomCategory = roomCategory;
        }
        public BookingViewModel(BookingData bookingData)
        {
            BookingData = bookingData;
        }
    }
}
