using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hotel_NotFarOff.Models.Entities
{
    public partial class Room : BaseEntity
    {
        public Room()
        {
            Bookings = new HashSet<Booking>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Не указан номер комнаты")]
        public int RoomNumber { get; set; }

        [Required(ErrorMessage = "Не указана категория номера")]
        public int RoomCategoryId { get; set; }
        public bool IsBooked { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual RoomCategory RoomCategory { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public string RoomStatus => IsBooked ? "Забронирован" : "Свободен";
        public string LastChange => UpdatedDate == null ? "Изменений не было" : UpdatedDate.Value.ToString();
        public string LastEmlpoyee => Employee == null ? "Изменений не было" : Employee.Account.Login.ToString();
    }
}
