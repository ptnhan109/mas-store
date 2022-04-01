using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mas.Application.ManufactureServices.Dtos
{
    public class AddManufacture
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public Guid? GroupId { get; set; }
        
        [ForeignKey(nameof(GroupId))]
        public virtual ManufactureGroup Group { get; set; }

        public string Mail { get; set; }

        public string TaxCode { get; set; }

        public string Address { get; set; }

        public string Note { get; set; }

        public string Phone { get; set; }

        public string Province { get; set; }

        public Manufacture ToEntity() => new Manufacture()
        {
            CreatedAt = DateTime.Now,
            Id = Guid.NewGuid(),
            UpdatedAt = DateTime.Now,
            Address = Address,
            GroupId = GroupId,
            Mail = Mail,
            Name = Name,
            Note = Note,
            Phone = Phone,
            Province = Province,
            Code = Code
        };
    }
}
