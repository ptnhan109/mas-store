using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mas.Application.ManufactureGroupServices.Dtos
{
    public class UpdateManufactureGroup : AddManufactureGroup
    {
        public Guid Id { get; set; }

        public override ManufactureGroup ToEntity()
        {
            var entity = base.ToEntity();
            entity.Id = Id;

            return entity;
        }
    }
}
