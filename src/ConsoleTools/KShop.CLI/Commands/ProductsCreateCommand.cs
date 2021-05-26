
using KShop.Communications.Contracts.ValueObjects;
using KShop.Products.Persistence;
using KShop.Products.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

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
            for (int i = 0; i < 100; i++)
            {
                var prod = new Product()
                {
                    Title = $"Product",
                    Money = new Money(100),
                    Positions = new List<ProductPosition> { new ProductPosition() { Quantity = 100 } }
                };
                await _dbContext.Products.AddAsync(prod, cancelToken);

                var pos = string.Join(',', prod.Positions.Select(e => $"{e.ID} - {e.Quantity}").ToList());
                _logger.LogInformation($"{prod.ID} {prod.Title} {prod.Money} Positions:({pos})");
            }
            await _dbContext.SaveChangesAsync(cancelToken);

        }
    }
}
