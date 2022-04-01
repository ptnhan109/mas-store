using Mas.Application.ManufactureServices.Dtos;
using Mas.Core;
using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mas.Application.ManufactureServices
{
    public class ManufactureService : IManufactureService
    {
        private readonly IAsyncRepository<Manufacture> _repo;

        public ManufactureService(IAsyncRepository<Manufacture> repository)
        {
            _repo = repository;
        }

        public async Task Add(AddManufacture request)
        {
            await _repo.AddAsync(request.ToEntity());
        }
    }
}
