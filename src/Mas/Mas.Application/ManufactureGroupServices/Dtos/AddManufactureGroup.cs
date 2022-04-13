using Mas.Common;
using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mas.Application.ManufactureGroupServices.Dtos
{
    public class AddManufactureGroup
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ManufactureGroup ToEntity() => new ManufactureGroup()
        {
            CreatedAt = DateTime.Now,
            Description = Description,
            Id = Guid.NewGuid(),
            Name = Name,
            UpdatedAt = DateTime.Now,
            SearchParams = Name.ToRemoveUnicode()
        };
    }
}
