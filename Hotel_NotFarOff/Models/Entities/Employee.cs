using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hotel_NotFarOff.Models.Entities
{
    public partial class Employee : BaseEntity
    {
        public Employee()
        {
            RoomCategories = new HashSet<RoomCategory>();
            Rooms = new HashSet<Room>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Не указано ФИО")]
        [RegularExpression(@"([А-ЯЁ][а-яё]+[\-\s]?){3,}", ErrorMessage = "Введите корректный ФИО")]
     
        public string FullName { get; set; }

        [Required(ErrorMessage = "Не указан паспорт")]
        [RegularExpression(@"\d{4}\s\d{6}", ErrorMessage = "Введите корректный паспорт")]
        public string Passport { get; set; }

        [Required(ErrorMessage = "Не указано номер телефона")]
        [RegularExpression(@"[1-9]{1}[0-9]{10}", ErrorMessage = "Введите корректный номер телефона")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Не указана дата рождения")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Не указана должность")]
        public int PostId { get; set; }

        [Required(ErrorMessage = "Не указан пол")]
        public string Gender { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Post Post { get; set; }
        public virtual Account Account { get; set; }
        public virtual ICollection<RoomCategory> RoomCategories { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }

        public string LastChange => UpdatedDate == null ? "Изменений не было" : UpdatedDate.Value.ToString();
    }
}
