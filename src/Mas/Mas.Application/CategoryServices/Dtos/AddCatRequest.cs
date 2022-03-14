using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mas.Application.CategoryServices.Dtos
{
    public class AddCatRequest
    {
        public string Name { get; set; }

        public string Location { get; set; }

        public string Description { get; set; }

        public virtual Category ToEntity() => new Category()
        {
            Id = Guid.NewGuid(),
            Name = Name,
            Location = Location,
            Description = Description,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            Code = string.Empty,
            IsDeleted = false
        };
    }

    public class UpdateCatRequest : AddCatRequest
    {
        public Guid Id { get; set; }

        public override Category ToEntity()
        {
            var cat = base.ToEntity();
            cat.Id = Id;

            return cat;
        }
    }
}
