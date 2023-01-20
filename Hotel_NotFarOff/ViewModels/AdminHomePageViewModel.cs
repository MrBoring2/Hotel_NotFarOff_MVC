namespace Hotel_NotFarOff.ViewModels
{
    public class AdminHomePageViewModel
    {
        public string FullName { get; set; }
        public string PostName { get; set; }
        public string Login { get; set; }

        public AdminHomePageViewModel(string fullName, string postName, string login)
        {
            FullName = fullName;
            PostName = postName;
            Login = login;
        }

        public AdminHomePageViewModel()
        {
        }
    }
}
