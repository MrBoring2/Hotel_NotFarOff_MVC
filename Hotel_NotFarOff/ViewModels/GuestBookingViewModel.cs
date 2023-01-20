using Hotel_NotFarOff.Contexts;
using Hotel_NotFarOff.Models;
using Hotel_NotFarOff.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Hotel_NotFarOff.ViewModels
{
    public class GuestBookingViewModel
    {
        public BookingData BookingData { get; set; }
        public int NightCount => BookingData == null ? 0 : (BookingData.CheckOut - BookingData.CheckIn).Days;
        public RoomCategory RoomCategory { get; set; }
        [Required(ErrorMessage = "Поле ФИО заказчика обязательно.")]
        public string TenantFullName { get; set; }
        [Required(ErrorMessage = "Поле Телефон заказчика обязательно.")]
        [RegularExpression(@"[1-9]{1}[0-9]{10}", ErrorMessage = "Введите корректный номер телефона")]
        public string TenantPhone { get; set; }
        [Required(ErrorMessage = "Поле Email заказчика обязательно.")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Введите корректрый Email адрес.")]
        public string TenantEmail { get; set; }
        public GuestModel[] Guests { get; set; }
        public decimal TotalPrice => RoomCategory == null ? 0 : RoomCategory.PricePerDay * NightCount;
        public List<PaymentMethod> PaymentMethods { get; set; }
        public int SelectedPaymentMethodId { get; set; }
        public string DisplayNights
        {
            get
            {
                string nights = "";
                string number = Convert.ToString(NightCount);
                int lastNumber = Convert.ToInt32(number[number.Length - 1].ToString());
                int twoLastNumbers = number.Length > 1 ? Convert.ToInt32(number.Remove(0, number.Length - 2)) : lastNumber;
                if (lastNumber >= 2 && lastNumber <= 4)
                {
                    nights = "ночи";
                }
                else if ((lastNumber >= 5 && lastNumber <= 9) || (lastNumber == 0) || (twoLastNumbers >= 11 && twoLastNumbers <= 19))
                {
                    nights = "ночей";
                }
                else
                {
                    nights = "ночь";
                }


                return $"{NightCount} {nights}";
            }
        }

        public GuestBookingViewModel()
        {
        }

        public GuestBookingViewModel(BookingData bookingData, RoomCategory roomCategory, ICollection<PaymentMethod> paymentMethods)
        {
            BookingData = bookingData;
            RoomCategory = roomCategory;
            Guests = new GuestModel[bookingData.AdultCount + bookingData.ChildCount];
            for (int i = 0; i < Guests.Length; i++)
            {
                Guests[i] = new GuestModel();
            }
            PaymentMethods = paymentMethods.ToList();
        }
        public GuestBookingViewModel(BookingData bookingData, ICollection<PaymentMethod> paymentMethods)
        {
            BookingData = bookingData;
            Guests = new GuestModel[bookingData.AdultCount + bookingData.ChildCount];
            for (int i = 0; i < Guests.Length; i++)
            {
                Guests[i] = new GuestModel();
            }
            PaymentMethods = paymentMethods.ToList();
        }
    }
}
