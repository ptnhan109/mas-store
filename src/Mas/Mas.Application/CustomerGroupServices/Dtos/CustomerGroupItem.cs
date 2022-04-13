using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mas.Application.CustomerGroupServices.Dtos
{
    public class CustomerGroupItem
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string CreatedAt { get; set; }

        public CustomerGroupItem(CustomerGroup group)
        {
            Name = group.Name;
            Description = group.Description;
            CreatedAt = group.CreatedAt.ToString("dd-MM-yyyy");
            Id = group.Id;
        }
    }
}
