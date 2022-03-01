using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mas.Application.CategoryServices.Dtos
{
    public class CategoryItem
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public CategoryItem(Category category)
        {
            Id = category.Id;
            Name = category.Name;
        }
    }
}
