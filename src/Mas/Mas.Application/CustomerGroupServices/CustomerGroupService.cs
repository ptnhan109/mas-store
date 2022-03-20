using Mas.Application.CustomerGroupServices.Dtos;
using Mas.Core;
using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mas.Application.CustomerGroupServices
{
    public class CustomerGroupService : ICustomerGroupService
    {
        private readonly IAsyncRepository<CustomerGroup> _repository;

        public CustomerGroupService(IAsyncRepository<CustomerGroup> repository)
        {
            _repository = repository;
        }
        public async Task<CustomerGroup> AddAsync(AddCustomerGroup request)
        {
            return await _repository.AddAsync(request.ToEntity());
        }

        public async Task<IEnumerable<CustomerGroupItem>> CustomerGroups()
        {
            return (await _repository.FindAllAsync(null, null)).Select(c => new CustomerGroupItem(c));
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task UpdateAsync(UpdateCustomerGroup request)
        {
            await _repository.UpdateAsync(request.ToEntity());
        }
    }
}
