using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Services.User
{
    public interface IJsonTokenService
    {
        public string GenerateToken(Models.User user, List<Claim> claims);
    }
}
