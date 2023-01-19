using System;
using System.Collections.Generic;

namespace Hotel_NotFarOff.Models.Entities
{
    public partial class Service
    {
        public Service()
        {
            RoomCategories = new HashSet<RoomCategory>();
        }

        public int Id { get; set; }
        public string Title { get; set; }

        public virtual ICollection<RoomCategory> RoomCategories { get; set; }
    }
}
