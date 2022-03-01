using Mas.Application.CategoryServices.Dtos;
using Mas.Core;
using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mas.Application.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly IAsyncRepository<Category> _repository;

        public CategoryService(IAsyncRepository<Category> repository)
        {
            this._repository = repository;
        }
        public async Task<IEnumerable<CategoryItem>> Categories()
        {
            Expression<Func<Category, CategoryItem>> selector = c => new CategoryItem(c);
            var categories = await _repository.FindAllAsync(null,null);

            return categories.Select(c => new CategoryItem(c))
                .ToList();
        }
    }
}
