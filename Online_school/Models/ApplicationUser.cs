using Microsoft.AspNetCore.Identity;

namespace Online_school.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<Group_User> Groups { get; set; }

       // public string? UserName { get; set; }

    }
}
