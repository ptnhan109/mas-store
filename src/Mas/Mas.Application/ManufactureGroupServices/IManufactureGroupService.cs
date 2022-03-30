using Mas.Application.ManufactureGroupServices.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mas.Application.ManufactureGroupServices
{
    public interface IManufactureGroupService
    {
        Task AddAsync(AddManufactureGroup request);

        Task UpdateAsync(UpdateManufactureGroup request);

        Task<IEnumerable<ManufactureGroupItem>> GetAllAsync();

        Task DeleteAsync(Guid id);

    }
}
