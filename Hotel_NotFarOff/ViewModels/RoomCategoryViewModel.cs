using Hotel_NotFarOff.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Hotel_NotFarOff.ViewModels
{
    public class RoomCategoryViewModel : BaseEntity
    {
        public RoomCategoryViewModel()
        {
        }

        public RoomCategoryViewModel(RoomCategory roomCategory)
        {
            Id = roomCategory.Id;
            Title = roomCategory.Title;
            PricePerDay = (float)roomCategory.PricePerDay;
            RoomCount = roomCategory.RoomCount;
            NumbeOfSeats = roomCategory.NumbeOfSeats;
            RoomSize = roomCategory.RoomSize;
            ShortDescription = roomCategory.ShortDescription;
            Description = roomCategory.Description;
            MainImage = roomCategory.MainImage;
            RoomImages = roomCategory.RoomImages;
            Employee = roomCategory.Employee;
            UpdatedDate = roomCategory.UpdatedDate;
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Не указано название")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Не указана цена за ночь")]
        [DataType(DataType.Currency, ErrorMessage = "Значение должно быть числом")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Значение должно быть больше 0")]
        public double PricePerDay { get; set; }
        [Required(ErrorMessage = "Не указано количество комнат в комере")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Значение должно быть больше 0")]
        public int RoomCount { get; set; }
        [Required(ErrorMessage = "Не указана вместимость")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Значение должно быть больше 0")]
        public int NumbeOfSeats { get; set; }
        [Required(ErrorMessage = "Не указана площадь")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Значение должно быть больше 0")]
        public double RoomSize { get; set; }
        [Required(ErrorMessage = "Не указано краткое описание")]
        public string ShortDescription { get; set; }
        [Required(ErrorMessage = "Не указано описание")]
        public string Description { get; set; }
        public byte[] MainImage { get; set; }
        [BindProperty]
        [Display(Name = "MainImageFile")]
        public IFormFile MainImageFile { get; set; }
        public List<RoomImage> RoomImages { get; set; }
        [BindProperty]
        [Display(Name = "RoomImage1")]
        public IFormFile RoomImage1 { get; set; }
        [BindProperty]
        [Display(Name = "RoomImage2")]
        public IFormFile RoomImage2 { get; set; }
        [BindProperty]
        [Display(Name = "RoomImage3")]
        public IFormFile RoomImage3 { get; set; }
        public List<SelectListItem> Services { get; set; }
        public Employee Employee { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int[] ServicesIds { get; set; }
        public string StringServices => Services != null && ServicesIds != null ? string.Join(", ", Services.Where(p => ServicesIds.Any(j => j == Convert.ToInt32(p.Value))).Select(p => p.Text)) : "";
        public string LastChange => UpdatedDate == null ? "Изменений не было" : UpdatedDate.Value.ToString();
        public string LastEmlpoyee => Employee == null ? "Изменений не было" : Employee.Account.Login.ToString();
    }
}
