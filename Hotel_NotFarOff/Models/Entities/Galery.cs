using System;
using System.Collections.Generic;

namespace Hotel_NotFarOff.Models.Entities
{
    public partial class Galery
    {
        public int Id { get; set; }
        public byte[] Image { get; set; }
        public int RoomCategoryId { get; set; }

        public virtual RoomCategory RoomCategory { get; set; }
    }
}
