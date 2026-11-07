using System;
using System.Collections.Generic;
using System.Text;

namespace WebApplication1.Models
{
    public class Admin
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiry { get; set; }
    }
}
