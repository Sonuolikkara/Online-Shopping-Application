using System;
using System.Collections.Generic;
using System.Text;

namespace WebApplication1.Models
{
    public class LoginResponseModel
    {
        public int CustomerId { get; set; }

        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

    }
}
