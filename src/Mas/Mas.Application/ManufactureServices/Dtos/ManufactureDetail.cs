using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mas.Application.ManufactureServices.Dtos
{
    public class ManufactureDetail
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Mail { get; set; }

        public string Phone { get; set; }

        public Guid? GroupId { get; set; }

        public string Address { get; set; }

        public string Province { get; set; }

        public string Note { get; set; }

        public string Code { get; set; }

        public string TaxCode { get; set; }

        public ManufactureDetail(Manufacture entity)
        {
            Name = entity.Name;
            Mail = entity.Mail;
            Phone = entity.Phone;
            GroupId = entity.GroupId;
            Address = entity.Address;
            Province = entity.Province;
            Note = entity.Note;
            Code = entity.Code;
            Id = entity.Id;
            GroupId = entity.GroupId;
            TaxCode = entity.TaxCode;
        }
    }
}
