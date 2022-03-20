using Mas.Application.CustomerServices.Dtos;
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
            if(entity != null)
            {
                entity.IsDeleted = true;
                await _repository.UpdateAsync(entity);
            }
        }

        public async Task<List<Customer>> GetCustomers(string keyword, Guid? group)
        {
            Expression<Func<Customer, bool>> filter = c => c.Name.Contains(keyword) && !c.IsDeleted;
            var query = _repository.GetQueryable();
            if(group != null)
            {
                query = query.Where(c => c.GroupId == group.Value);
            }

            return await query.ToListAsync();
        }

        public async Task UpdateCustomer(UpdateCustomerRequest request)
        {
            await _repository.UpdateAsync(request.ToEntity());
        }
    }
}
