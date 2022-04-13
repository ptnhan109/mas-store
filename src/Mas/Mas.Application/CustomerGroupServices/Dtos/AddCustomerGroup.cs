﻿using Mas.Common;
using Mas.Core.Contants;
using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mas.Application.CustomerGroupServices.Dtos
{
    public class AddCustomerGroup
    {
        public string Name { get; set; }

        public string Location { get; set; }

        public string Description { get; set; }

        public virtual CustomerGroup ToEntity() => new CustomerGroup()
        {
            Id = Guid.NewGuid(),
            Name = Name,
            Description = Description,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            Code = ContantPrefix.CustomerGroup.GenerateCode(4),
            SearchParams = Name.ToRemoveUnicode()
        };
    }

    public class UpdateCustomerGroup : AddCustomerGroup
    {
        public Guid Id { get; set; }

        public override CustomerGroup ToEntity()
        {
            var cus = base.ToEntity();
            cus.Id = Id;

            return cus;
        }
    }
}
