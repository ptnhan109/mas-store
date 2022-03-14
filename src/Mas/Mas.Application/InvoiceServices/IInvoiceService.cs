using Mas.Application.InvoiceServices.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mas.Application.InvoiceServices
{
    public interface IInvoiceService
    {
        Task AddAsync(AddInvoiceRequest request);
    }
}
