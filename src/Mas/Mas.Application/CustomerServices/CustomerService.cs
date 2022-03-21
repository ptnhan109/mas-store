using Mas.Application.CustomerServices.Dtos;
using Mas.Common;
using Mas.Core;
using Mas.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mas.Application.CustomerServices
{
    public class CustomerService : ICustomerService
    {
        private readonly IAsyncRepository<Customer> _repository;

        public CustomerService(IAsyncRepository<Customer> repository)
        {
            _repository = repository;
        }

        public async Task AddCustomer(AddCustomerRequest request)
        {
            await _repository.AddAsync(request.ToEntity());
        }

        public async Task DeleteCustomer(Guid id)
        {
            var entity = await _repository.FindAsync(id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                await _repository.UpdateAsync(entity);
            }
        }

        public async Task<CustomerItem> GetCustomer(Guid id)
        {
            var entity = await _repository.FindAsync(id, null);
            return new CustomerItem(entity);
        }

        public async Task<PagedResult<CustomerItem>> GetCustomers(string keyword, Guid? group, int? page = 1, int? pageSize = 20)
        {
            var query = _repository.GetQueryable().Include("CustomerGroup").Where(c => !c.IsDeleted);

            if (group != null)
            {
                query = query.Where(c => c.GroupId == group.Value);
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(c => c.Name.Contains(keyword));
            }

            var paged = await _repository.FindPagedAsync(query,null,page.Value,pageSize.Value);

            return paged.ChangeType(CustomerItem.FromEntity);
        }

        public async Task UpdateCustomer(UpdateCustomerRequest request)
        {
            await _repository.UpdateAsync(request.ToEntity());
        }
    }
}
