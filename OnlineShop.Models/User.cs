using Microsoft.AspNetCore.Identity;

namespace OnlineShop.Models
{
    public class User : IdentityUser
    {
        public Role Role { get; set; }
        public string RoleId { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public ICollection<UserWithProducts> Products { get; set; }
    }
}
