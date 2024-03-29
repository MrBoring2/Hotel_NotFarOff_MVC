﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hotel_NotFarOff.Models.Entities
{
    public partial class Guest
    {
        public Guest()
        {
            Bookings = new HashSet<Booking>();
        }

        public int Id { get; set; }
        [Required]
        public string FullName { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
