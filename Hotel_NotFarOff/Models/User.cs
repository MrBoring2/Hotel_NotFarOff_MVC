using Microsoft.AspNetCore.Identity;

namespace Hotel_NotFarOff.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public string Login { get; set; }
        public int PostId { get; set; }
    }
}
