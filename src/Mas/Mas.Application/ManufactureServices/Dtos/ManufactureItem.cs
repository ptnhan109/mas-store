using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mas.Application.ManufactureServices.Dtos
{
    public class ManufactureItem
    {
        public Guid Id { get; set; }

        public string Group { get; set; }

        public string Name { get; set; }

        public int TotalOrder { get; set; }

        public string Province { get; set; }

        public ManufactureItem(Manufacture entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            TotalOrder = 0;
            Province = entity.Province;
            Group = entity.Group.Name;
        }
    }
}
