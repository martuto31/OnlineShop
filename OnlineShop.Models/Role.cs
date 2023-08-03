using Microsoft.AspNetCore.Identity;

namespace OnlineShop.Models
{
    public class Role : IdentityRole
    {
        public int Id { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
