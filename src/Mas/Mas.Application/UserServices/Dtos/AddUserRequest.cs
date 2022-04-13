using Mas.Core.Entities;
using Mas.Core.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mas.Application.UserServices.Dtos
{
    public class AddUserRequest
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public EnumRole Role { get; set; }

        public IFormFile Avatar { get; set; }

        public string Phone { get; set; }
    }

    public class UpdateUserRequest : AddUserRequest
    {
        public Guid Id { get; set; }
    }
}
