using Mas.Application.ManufactureGroupServices.Dtos;
using Mas.Core;
using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mas.Application.ManufactureGroupServices
{
    public class ManufactureGroupService : IManufactureGroupService
    {
        private readonly IAsyncRepository<ManufactureGroup> _repository;

        public ManufactureGroupService(IAsyncRepository<ManufactureGroup> repository)
        {
            _repository = repository;
        }
        public async Task AddAsync(AddManufactureGroup request)
        {
            await _repository.AddAsync(request.ToEntity());
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ManufactureGroupItem>> GetAllAsync()
        {
            var items = await _repository.FindAllAsync(null, new string[] { "Manufactures" });

            return items.Select(c => new ManufactureGroupItem(c));
        }

        public async Task UpdateAsync(UpdateManufactureGroup request)
        {
            await _repository.UpdateAsync(request.ToEntity());
        }
    }
}
