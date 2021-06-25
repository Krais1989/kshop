using KShop.Products.Persistence;
using KShop.Shared.Domain.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Attribute = KShop.Products.Persistence.Attribute;

namespace KShop.Products.WebApi
{
    public static class DataSeedExtensions
    {
        public static IApplicationBuilder AddKShopTestData(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ProductsContext>())
                {
                    context.Database.EnsureDeleted();
                    if (context.Database.GetPendingMigrations().Any())
                        context.Database.Migrate();

                    const int attrs_count = 10;
                    const int categs_count = 10;
                    const int products_count = 100;


                    var attrs = new List<Attribute>();
                    for (int i = 0; i < attrs_count; i++)
                    {
                        attrs.Add(new Attribute() { Title = $"Attribute {i + 1}" });
                    }
                    context.Attributes.AddRange(attrs);
                    context.SaveChanges();

                    var categories = new List<Category>();
                    for (int i = 0; i < categs_count; i++)
                    {
                        categories.Add(new Category { Name = $"Category #{i + 1}" });
                    }
                    context.Categories.AddRange(categories);
                    context.SaveChanges();


                    var products = new List<Product>();
                    for (int i = 0; i < products_count; i++)
                    {
                        var prodAttrs = new List<ProductAttribute>();
                        for (int j = 0; j < attrs_count; j++)
                        {
                            uint prodAttrId = attrs[j].ID;
                            prodAttrs.Add(
                                new ProductAttribute
                                {
                                    AttributeID = prodAttrId,
                                    Value = $"Value of attribute {prodAttrId}"
                                });
                        }
                        var prod = new Product()
                        {
                            Title = $"Product Title",
                            Money = new Money(100),
                            Positions = new List<ProductPosition> { new ProductPosition() { Quantity = 100 } },
                            Category = categories[i % categs_count],
                            ProductAttributes = prodAttrs
                        };
                        products.Add(prod);

                        var pos = string.Join(',', prod.Positions.Select(e => $"{e.ID} - {e.Quantity}").ToList());


                        Log.Information($"{prod.ID} {prod.Title} {prod.Money} Positions:({pos})");
                    }

                    context.Products.AddRange(products);
                    context.SaveChanges();
                }
            }

            return app;
        }
    }
}
