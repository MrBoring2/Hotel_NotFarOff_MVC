using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hotel_NotFarOff.Models.Entities
{
    public partial class Account
    {
        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "Не указан логин")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Не указан паспорт")]
        public string Password { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
