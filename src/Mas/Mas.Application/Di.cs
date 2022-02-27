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
        }
    }
}
