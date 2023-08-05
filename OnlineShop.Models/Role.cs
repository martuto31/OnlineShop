using Microsoft.AspNetCore.Identity;

namespace OnlineShop.Models
{
    public class Role : IdentityRole
    {
        public virtual ICollection<User> Users { get; set; }
    }
}
