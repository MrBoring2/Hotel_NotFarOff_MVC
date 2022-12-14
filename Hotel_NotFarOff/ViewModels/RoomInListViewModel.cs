namespace Hotel_NotFarOff.ViewModels
{
    public class RoomInListViewModel
    {

        public RoomInListViewModel(int id, string title, decimal pricePerDay, int roomCount, int numbeOfSeats, double roomSize, string shortDescription, byte[] mainImage)
        {
            Id = id;
            Title = title;
            PricePerDay = pricePerDay;
            RoomCount = roomCount;
            NumbeOfSeats = numbeOfSeats;
            RoomSize = roomSize;
            ShortDescription = shortDescription;
            MainImage = mainImage;
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal PricePerDay { get; set; }
        public int RoomCount { get; set; }
        public int NumbeOfSeats { get; set; }
        public double RoomSize { get; set; }
        public string ShortDescription { get; set; }
        public byte[] MainImage { get; set; }
    }
}
