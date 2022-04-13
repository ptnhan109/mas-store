using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mas.Application.CustomerServices.Dtos
{
    public class CustomerItem
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public DateTime? BirthDay { get; set; }

        public string Province { get; set; }

        public string Mail { get; set; }

        public Guid? GroupId { get; set; }

        public int Code { get; set; }

        public string Group { get; set; }


        public CustomerItem(Customer customer)
        {
            Id = customer.Id;
            Name = customer.Name;
            Phone = customer.Phone;
            Address = customer.Address;
            BirthDay = customer.BirthDay;
            Province = customer.Province;
            Mail = customer.Mail;
            GroupId = customer.GroupId;
            Code = customer.Code;
            Group = customer.CustomerGroup.Name;
        }

        public static CustomerItem FromEntity(Customer customer)
        {
            return new CustomerItem(customer);
        }
    }
}
