using System;
using System.Collections.Generic;

namespace Hotel_NotFarOff.Models.Entities
{
    public partial class RoomCategory
    {
        public RoomCategory()
        {
            Galeries = new HashSet<Galery>();
            Rooms = new HashSet<Room>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public decimal PricePerDay { get; set; }
        public int RoomCount { get; set; }
        public int NumbeOfSeats { get; set; }
        public double RoomSize { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Services { get; set; }

        public virtual ICollection<Galery> Galeries { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
    }
}
