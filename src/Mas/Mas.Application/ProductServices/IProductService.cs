using Mas.Application.ProductServices.Dtos;
using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mas.Application.ProductServices
{
    public interface IProductService
    {
        Task<Product> AddProduct(AddProductModel model);
    }
}
