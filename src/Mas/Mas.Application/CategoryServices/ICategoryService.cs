using Mas.Application.CategoryServices.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mas.Application.CategoryServices
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryItem>> Categories();
    }
}
