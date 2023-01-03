using System;
using System.Collections.Generic;

namespace Hotel_NotFarOff.Models.Entities
{
    public partial class SiteProfle
    {
        public int EmployeeId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
