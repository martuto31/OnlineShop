using Microsoft.AspNetCore.Identity;

namespace OnlineShop.Models
{
    public class User : IdentityUser
    {
        public int Id { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
    }
}
