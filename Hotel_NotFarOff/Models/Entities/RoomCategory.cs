using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hotel_NotFarOff.Models.Entities
{
    public partial class RoomCategory : BaseEntity
    {
        public RoomCategory()
        {
            RoomImages = new List<RoomImage>(3);
            Rooms = new HashSet<Room>();
            Services = new HashSet<Service>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Не указано название")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Не указана цена за ночь")]
        public decimal PricePerDay { get; set; }
        [Required(ErrorMessage = "Не указано количество комнат в комере")]
        public int RoomCount { get; set; }
        [Required(ErrorMessage = "Не указана вместиомсть")]
        public int NumbeOfSeats { get; set; }
        [Required(ErrorMessage = "Не указана площадь")]
        public double RoomSize { get; set; }
        [Required(ErrorMessage = "Не указано краткое описание")]
        public string ShortDescription { get; set; }
        [Required(ErrorMessage = "Не указано описание")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Не указано главное изображение")]
        public byte[] MainImage { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public virtual Employee Employee { get; set; }
        [Required(ErrorMessage = "Не указаны дополнительные изображения")]
        public virtual List<RoomImage> RoomImages { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
        [Required(ErrorMessage = "Не указаны сервисы")]
        public virtual ICollection<Service> Services { get; set; }
    }
}
