using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Shared.Options
{
    public static class JsonTokenOptions
    {
        public const string Jwt = "Jwt";
        public static string Key = "My-secret-key123456789123456789";
        public static  string Issuer = "https://localhost:7260/";
        public static string Audience = "https://localhost:7260/";
    }
}
