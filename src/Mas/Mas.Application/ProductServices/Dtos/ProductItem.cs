﻿using Mas.Common;
using Mas.Core.Contants;
using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mas.Application.ProductServices.Dtos
{
    public class ProductItem
    {
        public Guid Id { get; set; }

        public string BarCode { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public string Category { get; set; }

        public Unit Unit { get; set; }

        public string DefaultSell { get; set; }

        public string DefaultImport { get; set; }

        public ProductItem(Product entity)
        {
            var price = entity?.Prices?.FirstOrDefault(c => c.IsDefault);

            Id = entity.Id;
            BarCode = entity.BarCode;
            Name = entity.Name;
            Category = entity.Category.Name;
            DefaultSell = price?.SellPrice.ToCurrencyFormat();
            DefaultImport = price?.ImportPrice.ToCurrencyFormat();
            Unit = GetUnits(price.UnitId);
        }

        public static ProductItem FromEntity(Product entity) => new ProductItem(entity);
        private Unit GetUnits(int defaultUnit) => ContantsUnit.Units().FirstOrDefault(c => c.Id == defaultUnit);
    }
}