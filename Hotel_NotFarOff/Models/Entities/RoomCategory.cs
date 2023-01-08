using System;
using System.Collections.Generic;

namespace Hotel_NotFarOff.Models.Entities
{
    public partial class RoomCategory
    {
        public RoomCategory()
        {
            RoomImages = new HashSet<RoomImage>();
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
        public byte[] MainImage { get; set; }
        public virtual ICollection<RoomImage> RoomImages { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
    }
}
