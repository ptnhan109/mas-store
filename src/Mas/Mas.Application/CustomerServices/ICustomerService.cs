using Mas.Application.CustomerServices.Dtos;
using Mas.Common;
using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mas.Application.CustomerServices
{
    public interface ICustomerService
    {
        Task<PagedResult<CustomerItem>> GetCustomers(string keyword, Guid? group, int? page = 1, int? pageSize = 20);

        Task AddCustomer(AddCustomerRequest request);

        Task DeleteCustomer(Guid id);

        Task UpdateCustomer(UpdateCustomerRequest request);

        Task<CustomerItem> GetCustomer(Guid id);
    }
}
