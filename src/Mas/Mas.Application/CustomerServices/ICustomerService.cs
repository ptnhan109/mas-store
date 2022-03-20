using Mas.Application.CustomerServices.Dtos;
using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mas.Application.CustomerServices
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetCustomers(string keyword, Guid? group);

        Task AddCustomer(AddCustomerRequest request);

        Task DeleteCustomer(Guid id);

        Task UpdateCustomer(UpdateCustomerRequest request);
    }
}
