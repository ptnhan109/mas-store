using Mas.Application.CategoryServices;
using Mas.Application.CustomerGroupServices;
using Mas.Application.CustomerServices;
using Mas.Application.ImportServices;
using Mas.Application.InventoryServices;
using Mas.Application.InvoiceServices;
using Mas.Application.ManufactureGroupServices;
using Mas.Application.ManufactureServices;
using Mas.Application.ProductServices;
using Mas.Application.ReportServices;
using Mas.Application.UserServices;
using Mas.Core;
using Microsoft.AspNetCore.Http;
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
            service.AddScoped<IManufactureGroupService, ManufactureGroupService>();
            service.AddScoped<IManufactureService, ManufactureService>();
            service.AddScoped<IInventoryService, InventoryService>();
            service.AddScoped<IImportService, ImportService>();
            service.AddScoped<IReportService, ReportService>();

            service.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
    }
}
