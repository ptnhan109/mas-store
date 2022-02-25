using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mas.Core.Contants
{
    public class ContantsUnit
    {
        public static List<Unit> Units() => new List<Unit>()
        {
            new Unit(){Id=1,Name ="Bộ",Description = string.Empty},
            new Unit(){Id=2,Name ="Cái",Description = string.Empty},
            new Unit(){Id=3,Name ="Chiếc",Description = string.Empty},
            new Unit(){Id=4,Name ="Chục",Description = string.Empty},
            new Unit(){Id=5,Name ="Cuộn",Description = string.Empty},
            new Unit(){Id=6,Name ="Dây",Description = string.Empty},
            new Unit(){Id=7,Name ="Đôi",Description = string.Empty},
            new Unit(){Id=8,Name ="Hộp",Description = string.Empty},
            new Unit(){Id=9,Name ="Lần",Description = string.Empty},
            new Unit(){Id=10,Name ="Mét",Description = string.Empty},
            new Unit(){Id=11,Name ="Tá",Description = string.Empty},
            new Unit(){Id=12,Name ="Thùng",Description = string.Empty},

        };
    }
}
