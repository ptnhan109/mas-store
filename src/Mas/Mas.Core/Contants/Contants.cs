using Mas.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mas.Core.Contants
{
    public static class ContantSessions
    {
        public const string Username = nameof(Username);
        public const string Name = nameof(Name);
        public const string Role = nameof(Role);
        public const string Avatar = nameof(Avatar);
    }

    public class ContantRole
    {
        public static string GetRole(EnumRole role)
        {
            switch (role)
            {
                case EnumRole.Admin:
                    return "Chủ cửa hàng";
                case EnumRole.Employee:
                    return "Nhân viên bán hàng";
                default:
                    return string.Empty;
            }
        }
    }
}
