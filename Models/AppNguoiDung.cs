using Microsoft.AspNetCore.Identity;

namespace OnlineMovieTicket.Models
{
    public class AppNguoiDung : IdentityUser
    {
        [PersonalData]
        public string? Ho { get; set; }
        [PersonalData]
        public string? Ten { get; set; }
    }

}
