using Mas.Application.InventoryServices.Dtos;
using Mas.Core;
using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mas.Application.InventoryServices
{
    public class InventoryService : IInventoryService
    {
        private readonly IAsyncRepository<InventoryItem> _repository;

        public InventoryService(IAsyncRepository<InventoryItem> repository)
        {
            _repository = repository;
        }
        public async Task AddInventoryItem(AddInventoryItem item)
        {
            await _repository.AddAsync(item.ToEntity());
        }
    }
}
