using Mas.Application.CustomerGroupServices.Dtos;
using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mas.Application.CustomerGroupServices
{
    public interface ICustomerGroupService
    {
        Task<IEnumerable<CustomerGroupItem>> CustomerGroups();

        Task<CustomerGroup> AddAsync(AddCustomerGroup request);

        Task UpdateAsync(UpdateCustomerGroup request);

        Task DeleteAsync(Guid id);
    }
}
