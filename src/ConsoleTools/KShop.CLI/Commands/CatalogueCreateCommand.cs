using KShop.Catalogues.Persistence;
using KShop.Catalogues.Persistence.Entities;
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

    public class CatalogueCreateCommand : IBaseCommandAsync
    {
        private readonly ILogger<CatalogueCreateCommand> _logger;
        private readonly CatalogueContext _dbContext;

        public CatalogueCreateCommand(ILogger<CatalogueCreateCommand> logger, CatalogueContext dbContext)
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
                    Price = 50,
                    Positions = new List<ProductPosition> { new ProductPosition() { Quantity = 100 } }
                };
                await _dbContext.Products.AddAsync(prod, cancelToken);

                var pos = string.Join(',', prod.Positions.Select(e => $"{e.ID} - {e.Quantity}").ToList());
                _logger.LogInformation($"{prod.ID} {prod.Title} {prod.Price} Positions:({pos})");
            }
            await _dbContext.SaveChangesAsync(cancelToken);

        }
    }
}
