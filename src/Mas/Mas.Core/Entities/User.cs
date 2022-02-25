using Mas.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mas.Core.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public EnumRole Role { get; set; }

        public string Name { get; set; }

    }
}
