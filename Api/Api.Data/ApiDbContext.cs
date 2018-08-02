﻿namespace Api.Data
{
    using Api.Domain.Entities;
    using Configurations;
    using Microsoft.EntityFrameworkCore;

    public class ApiDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<ProductOrder> ProductOrders { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<DeliveryData> DeliveryData { get; set; }

        public DbSet<CustomerData> CustomerData { get; set; }

        public DbSet<HomeDeliveryData> HomeDeliveryData { get; set; }

        public DbSet<OfficeDeliveryData> OfficeDeliveryData { get; set; }

        public DbSet<InvoiceData> InvoiceData { get; set; }

        public DbSet<Numerator> Numerator { get; set; }

        public DbSet<OrderLog> OrderLogs { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<News> News { get; set; }

        public DbSet<HomeContent> HomeContent { get; set; }

        public DbSet<CategoryProduct> CategoryProducts { get; set; }

        public DbSet<CarouselItem> CarouselItems { get; set; }

        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfiguration());

            builder.ApplyConfiguration(new ProductConfiguration());

            builder.ApplyConfiguration(new OrderConfiguration());

            builder.ApplyConfiguration(new ProductOrderConfiguration());

            builder.ApplyConfiguration(new CategoryConfiguration());

            builder.ApplyConfiguration(new CategoryProductConfiguration());

            base.OnModelCreating(builder);
        }
    }
}
