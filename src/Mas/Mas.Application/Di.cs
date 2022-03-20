using Mas.Application.CategoryServices;
using Mas.Application.CustomerGroupServices;
using Mas.Application.CustomerServices;
using Mas.Application.InvoiceServices;
using Mas.Application.ProductServices;
using Mas.Application.UserServices;
using Mas.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mas.Application
{
    public static class Di
    {
        public static void AddDependencies(this IServiceCollection service)
        {
            service.AddScoped(typeof(IAsyncRepository<>), typeof(AsyncRepository<>));
            service.AddScoped<IUserService, UserService>();
            service.AddScoped<ICategoryService,CategoryService>();
            service.AddScoped<IProductService, ProductService>();
            service.AddScoped<IInvoiceService, InvoiceService>();
            service.AddScoped<ICustomerService, CustomerService>();
            service.AddScoped<ICustomerGroupService, CustomerGroupService>();
        }
    }
}
