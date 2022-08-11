using Mas.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mas.Core.AppDbContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions _options) : base(_options)
        {

        }

        public DbSet<Price> Prices { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<CustomerGroup> CustomerGroups { get; set; }

        public DbSet<Manufacture> Manufactures { get; set; }

        public DbSet<ManufactureGroup> ManufactureGroups { get; set; }

        public DbSet<Invoice> Invoices { get; set; }

        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }

        public DbSet<InventoryItem> InventoryItems { get; set; }

        public DbSet<Destruction> Destructions { get; set; }

        public DbSet<DestructionDetail> DestructionDetails { get; set; }

        public DbSet<Import> Imports { get; set; }

        public DbSet<ImportDetail> ImportDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
