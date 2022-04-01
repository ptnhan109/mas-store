using Mas.Application.ManufactureServices.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mas.Application.ManufactureServices
{
    public interface IManufactureService
    {
        Task Add(AddManufacture request);
    }
}
