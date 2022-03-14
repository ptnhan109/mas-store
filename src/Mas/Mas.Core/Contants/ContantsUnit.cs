using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mas.Core.Contants
{
    public class ContantsUnit
    {
        public static List<Unit> Units() => new List<Unit>()
        {
            new Unit(){Id=1,Name ="Cái",Description = "badge bg-primary"},
            new Unit(){Id=2,Name ="Bao",Description = "badge bg-secondary"},
            new Unit(){Id=3,Name ="Gói",Description = "badge bg-success"},
            new Unit(){Id=4,Name ="Chiếc",Description = "badge bg-danger"},
            new Unit(){Id=5,Name ="Chiếc",Description = "badge bg-warning"},
            new Unit(){Id=6,Name ="Chiếc",Description = "badge bg-info"},
            new Unit(){Id=7,Name ="Lốc",Description = "badge bg-light"},
            new Unit(){Id=8,Name ="Cuộn",Description = "badge bg-dark"},
            new Unit(){Id=9,Name ="Dây",Description = "badge bg-primary"},
            new Unit(){Id=10,Name ="Đôi",Description = "badge bg-secondary"},
            new Unit(){Id=11,Name ="Hộp",Description = "badge bg-success"},
            new Unit(){Id=12,Name ="Lần",Description = "badge bg-warning"},
            new Unit(){Id=13,Name ="Mét",Description = "badge bg-info"},
            new Unit(){Id=14,Name ="Thùng",Description = "badge bg-light"},

        };

        public static Unit GetUnit(int id) => Units().FirstOrDefault(c => c.Id == id);

        public static int GetId(string name) => Units().FirstOrDefault(c => c.Name.ToLower().Equals(name.ToLower())).Id;
    }
}
