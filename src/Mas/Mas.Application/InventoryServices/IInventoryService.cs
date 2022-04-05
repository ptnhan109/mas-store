using Mas.Application.InventoryServices.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mas.Application.InventoryServices
{
    public interface IInventoryService
    {
        Task AddInventoryItem(AddInventoryItem item);
    }
}
