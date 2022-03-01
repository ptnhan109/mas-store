using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mas.Application.ProductServices.Dtos
{
    public class AddProductModel
    {
        public string BarCode { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public double DefaultSellPrice { get; set; }

        public double DefaultImportPrice { get; set; }

        public int Inventory { get; set; }

        public Guid CategoryId { get; set; }

        public int? CloseToDate { get; set; }

        public int UnitId { get; set; }


        public int? ParentUnitId { get; set; }

        public int? TransferQuantity { get; set; }

        public double? ParentImportPrice { get; set; }

        public double? ParentSellPrice { get; set; }

        public Price ToPrice(Guid productId) => new Price()
        {
            Id = Guid.NewGuid(),
            ParentUnitId = ParentUnitId.Value,
            TransferQuantity = TransferQuantity.Value,
            ParentImportPrice = ParentImportPrice.Value,
            ParentSellPrice = ParentSellPrice.Value,
            UpdatedAt = DateTime.Now,
            CreatedAt = DateTime.Now,
            ProductId = productId
        };

        public Product ToProduct() => new Product()
        {
            BarCode = BarCode,
            Name = Name,
            IsActive = IsActive,
            DefaultSellPrice = DefaultSellPrice,
            DefaultImportPrice = DefaultImportPrice,
            Inventory = Inventory,
            Id = Guid.NewGuid(),
            CategoryId = CategoryId,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            CloseToDate = CloseToDate,
            UnitId = UnitId
        };
        
    }
}
