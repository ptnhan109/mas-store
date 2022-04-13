using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mas.Application.ManufactureGroupServices.Dtos
{
    public class ManufactureGroupItem
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        public string CreatedAt { get; set; }

        public ManufactureGroupItem(ManufactureGroup group)
        {
            Name = group.Name;
            Description = group.Description;
            Quantity = group.Manufactures.Any() ? group.Manufactures.Count : 0;
            CreatedAt = group.CreatedAt.ToString("dd-MM-yyyy");
            Id = group.Id;
        }
    }
}
