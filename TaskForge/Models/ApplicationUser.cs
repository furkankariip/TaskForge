using Microsoft.AspNetCore.Identity;

namespace TaskForge.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}