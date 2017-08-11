using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ShtikLive.Identity
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(16)]
        public string Handle { get; set; }
    }
}
