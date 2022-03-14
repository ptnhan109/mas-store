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

        public async Task<Category> AddAsync(AddCatRequest request)
        {
            return await _repository.AddAsync(request.ToEntity());
        }

        public async Task<IEnumerable<CategoryItem>> Categories()
        {
            Expression<Func<Category, CategoryItem>> selector = c => new CategoryItem(c);
            Expression<Func<Category, bool>> where = c => !c.IsDeleted;
            var categories = await _repository.FindAllAsync(where, null);

            return categories.Select(c => new CategoryItem(c))
                .ToList();
        }

        public async Task DeleteAsync(Guid id)
        {
            var cate = await _repository.FindAsync(id);
            cate.IsDeleted = true;
            await _repository.UpdateAsync(cate);
        }

        public async Task UpdateAsync(UpdateCatRequest request)
        {
            await _repository.UpdateAsync(request.ToEntity());
        }
    }
}
