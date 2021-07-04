

using KShop.Products.Persistence;
using KShop.Shared.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Attribute = KShop.Products.Persistence.Attribute;

namespace MarketCLI
{

    public class ProductsCreateCommand : IBaseCommandAsync
    {
        private readonly ILogger<ProductsCreateCommand> _logger;
        private readonly ProductsContext _dbContext;

        public ProductsCreateCommand(ILogger<ProductsCreateCommand> logger, ProductsContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task ExecuteAsync(CancellationToken cancelToken)
        {
            var attrs = new List<Attribute>();
            for (int i = 0; i < 10; i++)
            {
                attrs.Add(new Attribute() { Title = $"Attribute {i}" });
            }
            await _dbContext.Attributes.AddRangeAsync(attrs);

            for (int i = 0; i < 100; i++)
            {
                var prodAttrs = new List<ProductAttribute>();
                for (int j = 0; j < prodAttrs.Count; j++)
                {
                    uint prodAttrId = (uint)(i % 10);
                    prodAttrs.Add(
                        new ProductAttribute { AttributeID = prodAttrId, Value = $"Value of Prod {i} - Attr {prodAttrId}" });
                }
                var prod = new Product()
                {
                    Title = $"Product",
                    Price = new Money(100),
                    Positions = new List<ProductPosition> { new ProductPosition() { Quantity = 100 } },
                    Category = new Category { Name = "Category #0" },
                    ProductAttributes = prodAttrs
                };
                await _dbContext.Products.AddAsync(prod, cancelToken);

                var pos = string.Join(',', prod.Positions.Select(e => $"{e.ID} - {e.Quantity}").ToList());
                _logger.LogInformation($"{prod.ID} {prod.Title} {prod.Price} Positions:({pos})");
            }
            await _dbContext.SaveChangesAsync(cancelToken);

        }
    }
}
