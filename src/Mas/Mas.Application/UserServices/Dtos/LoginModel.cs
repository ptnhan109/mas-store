using System;
using System.Collections.Generic;
using System.Text;

namespace Mas.Application.UserServices.Dtos
{
    public class LoginModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public bool IsRemember { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}
