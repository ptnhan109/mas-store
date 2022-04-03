using Mas.Application.ManufactureServices.Dtos;
using Mas.Common;
using Mas.Core;
using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<PagedResult<ManufactureItem>> GetPaged(string keyword, Guid? group, int? page = 1, int? pageSize = 10)
        {
            var query = _repo.GetQueryable(new string[] { "Group" });
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(c => c.Name.Contains(keyword));
            }

            if(group != null)
            {
                query = query.Where(c => c.GroupId == group.Value);
            }

            var paged = await _repo.FindPagedAsync(query, null, page.Value, pageSize.Value);

            return paged.ChangeType<ManufactureItem>(c => new ManufactureItem(c));
        }
    }
}
