﻿using OnlineShop.Models;

namespace OnlineShop.Shared.DTO.UserDTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public int RoleId { get; set; }
    }
}
