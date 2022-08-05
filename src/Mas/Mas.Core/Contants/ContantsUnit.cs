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
            new Unit(){Id=1,Name ="Bao",Description = "badge bg-primary"},
            new Unit(){Id=2,Name ="Cây",Description = "badge bg-secondary"},
            new Unit(){Id=3,Name ="Chai",Description = "badge bg-success"},
            new Unit(){Id=4,Name ="Thùng",Description = "badge bg-danger"},
            new Unit(){Id=5,Name ="Lon",Description = "badge bg-warning"},
            new Unit(){Id=6,Name ="Hộp",Description = "badge bg-info"},
            new Unit(){Id=7,Name ="Can",Description = "badge bg-light"},
            new Unit(){Id=8,Name ="Tuýp",Description = "badge bg-dark"},
            new Unit(){Id=9,Name ="Gói",Description = "badge bg-secondary"},
            new Unit(){Id=10,Name ="Lọ",Description = "badge bg-success"},
            new Unit(){Id=11,Name ="Vỉ",Description = "badge bg-danger"},
            new Unit(){Id=12,Name ="Tô",Description = "badge bg-warning"},
            new Unit(){Id=13,Name ="Chiếc",Description = "badge bg-info"},
            new Unit(){Id=14,Name ="Đôi",Description = "badge bg-light"},
            new Unit(){Id=15,Name ="Túi",Description = "badge bg-dark"},
            new Unit(){Id=16,Name ="Bịch",Description = "badge bg-secondary"},
            new Unit(){Id=17,Name ="Quyển",Description = "badge bg-success"},
            new Unit(){Id=18,Name ="Bộ",Description = "badge bg-danger"},
            new Unit(){Id=19,Name ="Cuộn",Description = "badge bg-warning"},
            new Unit(){Id=20,Name ="Viên",Description = "badge bg-info"},
            new Unit(){Id=21,Name ="Cái",Description = "badge bg-light"},

        };

        public static Unit GetUnit(int id) => Units().FirstOrDefault(c => c.Id == id);

        public static int GetId(string name) => Units().FirstOrDefault(c => c.Name.ToLower().Equals(name.ToLower())).Id;
    }
}
