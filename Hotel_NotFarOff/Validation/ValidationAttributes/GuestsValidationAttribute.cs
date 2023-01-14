using Hotel_NotFarOff.Models;
using System.ComponentModel.DataAnnotations;

namespace Hotel_NotFarOff.Validation.ValidationAttributes
{
    public class CheckOutAboveCheckInAttribute : ValidationAttribute
    {
        public CheckOutAboveCheckInAttribute()
        {
            ErrorMessage = "Дата выезда должна быть больше даты заезда.";
        }
        public override bool IsValid(object? value)
        {
            BookingData? p = value as BookingData;
            return p != null && p.CheckOut > p.CheckIn;
        }
    }
}
