using Microsoft.AspNetCore.Identity;

namespace Pronia.Models
{
    public class AppUser : IdentityUser
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}
