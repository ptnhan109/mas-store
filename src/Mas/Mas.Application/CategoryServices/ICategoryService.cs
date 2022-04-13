using Mas.Application.CategoryServices.Dtos;
using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mas.Application.CategoryServices
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryItem>> Categories();

        Task<Category> AddAsync(AddCatRequest request);

        Task UpdateAsync(UpdateCatRequest request);

        Task DeleteAsync(Guid id);
    }
}
