using Mas.Core.Entities;
using Mas.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mas.Application.UserServices.Dtos
{
    public class UserItem
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Name { get; set; }

        public EnumRole Role { get; set; }

        public string Image { get; set; }

        public UserItem(User user)
        {
            Username = user.Username;
            Role = user.Role;
            Name = user.Name;
            Id = user.Id;
            Image = user.Image;
        }
    }
}
