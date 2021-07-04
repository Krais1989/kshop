using KShop.Products.Persistence;
using KShop.Shared.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Products.Persistence
{
    public static class DataSeedExtensions
    {
        public static void SeedData(this ModelBuilder builder)
        {
            const int attrs_count = 10;
            const int categs_count = 10;
            const int products_count = 22;
            const int positions_per_product = 1;
            const int attributes_per_product = 10;

            var attributes = new List<Attribute>();
            for (int i = 1; i <= attrs_count; i++)
            {
                attributes.Add(new Attribute() { ID = (uint)i, Title = $"Attribute #{i}" });
            }

            var categories = new List<Category>();
            for (int i = 1; i <= categs_count; i++)
            {
                categories.Add(new Category { ID = (uint)i, Name = $"Category #{i}" });
            }

            var products = new List<Product>();
            var product_positions = new List<ProductPosition>();
            var product_attributes = new List<ProductAttribute>();

            for (int p_i = 0; p_i < products_count; p_i++)
            {
                var p_id = (uint)p_i + 1;

                for (int pp_i = 0; pp_i < positions_per_product; pp_i++)
                {
                    var pp_id = (uint)(p_id * positions_per_product + pp_i);
                    product_positions.Add(
                        new ProductPosition() { ID = pp_id, ProductID = p_id, Quantity = 100, CreateDate = DateTime.UtcNow });
                }

                for (int pa_i = 0; pa_i < attributes_per_product; pa_i++)
                {
                    var pa_id = (uint)(pa_i % attrs_count) + 1;
                    product_attributes.Add(
                        new ProductAttribute { ProductID = p_id, AttributeID = pa_id, Value = $"Value of attribute {pa_id}" });
                }

                products.Add(
                    new Product()
                    {
                        ID = p_id,
                        Title = $"Product {p_id}",
                        Description = $"Description of product {p_id}",
                        CategoryID = categories[(p_i % categs_count)].ID,
                        Image = "https://cdn1.ozone.ru/multimedia/wc250/1022638096.jpg"
                    });
            }

            builder.Entity<Attribute>().HasData(attributes);
            builder.Entity<Category>().HasData(categories);
            builder.Entity<Product>(p =>
            {
                p.HasData(products);
                p.OwnsOne(e => e.Price).HasData(products.Select(p => new { ProductID = p.ID, Currency = "RUB", Price = 100m }));
            });
            builder.Entity<ProductAttribute>().HasData(product_attributes);
            builder.Entity<ProductPosition>().HasData(product_positions);
        }
    }
}
