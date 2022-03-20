using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mas.Application.CustomerServices.Dtos
{
    public class AddCustomerRequest
    {
        public string Name { get; set; }

        public string Phone { get; set; }

        public string Mail { get; set; }

        public string Address { get; set; }

        public DateTime? BirthDay { get; set; }

        public string Province { get; set; }

        public Guid? GroupId { get; set; }

        public virtual Customer ToEntity() => new Customer()
        {
            Address = Address,
            BirthDay = BirthDay,
            Code = 0,
            GroupId = GroupId,
            Id = Guid.NewGuid(),
            Name = Name,
            Phone = Phone,
            Mail = Mail,
            Province = Province,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
    }

    public class UpdateCustomerRequest : AddCustomerRequest
    {
        public Guid Id { get; set; }

        public override Customer ToEntity()
        {
            var customer = base.ToEntity();
            customer.Id = Id;

            return customer;
        }
    }
}
