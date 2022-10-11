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
            new Unit(){Id=22,Name="Ly",Description=""},
            new Unit(){Id=23,Name="Lốc",Description=""},
            new Unit(){Id=24,Name="Lọ",Description=""},
            new Unit(){Id=25,Name="Kg",Description=""},
            new Unit(){Id=26,Name="Tuýp",Description=""},
            new Unit(){Id=27,Name="Hũ",Description=""},
            new Unit(){Id=28,Name="Tuyp",Description=""},
            new Unit(){Id=29,Name="Cốc",Description=""},
            new Unit(){Id=30,Name="Khay",Description=""},
            new Unit(){Id=31,Name="Thanh",Description=""},
            new Unit(){Id=32,Name="Bánh",Description=""},
            new Unit(){Id=33,Name="Thỏi",Description=""},
            new Unit(){Id=34,Name="Quả",Description=""},
            new Unit(){Id=35,Name="Tập",Description=""},
            new Unit(){Id=36,Name="Miếng",Description=""},
            new Unit(){Id=37,Name="Con",Description=""},
            new Unit(){Id=38,Name="Bó",Description=""},
            new Unit(){Id=39,Name="Que",Description=""},
            new Unit(){Id=40,Name="Suất",Description=""},
            new Unit(){Id=41,Name="Trái",Description=""},
            new Unit(){Id=42,Name="Bom",Description=""},
            new Unit(){Id=43,Name="Giỏ",Description=""},
            new Unit(){Id=44,Name="Đĩa",Description=""},
            new Unit(){Id=45,Name="Dây",Description=""},
            new Unit(){Id=46,Name="Tệp",Description=""},
            new Unit(){Id=47,Name="Vi",Description=""},
            new Unit(){Id=48,Name="Bát",Description=""},
            new Unit(){Id=49,Name="Xấp",Description=""},
            new Unit(){Id=50,Name="Ram",Description=""},
            new Unit(){Id=51,Name="Ống",Description=""},
            new Unit(){Id=52,Name="Tuýt",Description=""}

        };

        public static Unit GetUnit(int id) => Units().FirstOrDefault(c => c.Id == id);

        public static int GetId(string name) => Units().FirstOrDefault(c => c.Name.ToLower().Equals(name.ToLower())).Id;
    }
}
