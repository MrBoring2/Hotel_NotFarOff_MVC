using System.ComponentModel.DataAnnotations;

namespace Hotel_NotFarOff.Models
{
    public class GuestModel
    {
        [Required(ErrorMessage = "Поле ФИО гостя обязательно.")]
        public string FullName { get; set; }
    }
}
