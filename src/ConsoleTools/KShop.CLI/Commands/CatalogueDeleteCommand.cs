
using KShop.Products.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MarketCLI
{
    public class CatalogueDeleteCommand : IBaseCommandAsync
    {
        private readonly ILogger<CatalogueDeleteCommand> _logger;
        private readonly ProductsContext _dbContext;

        public CatalogueDeleteCommand(ILogger<CatalogueDeleteCommand> logger, ProductsContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task ExecuteAsync(CancellationToken cancelToken)
        {
            var tables = new List<string>() { "ProductPositions", "ProductReserves", "Products" };

            foreach (var table in tables)
            {
                var sql = $"delete from {table};";
                _logger.LogInformation(sql);
                int count = _dbContext.Database.ExecuteSqlRaw(sql);
                _logger.LogInformation($"Result: {count}");
            }
            //await _dbContext.SaveChangesAsync(cancelToken);
        }
    }
}
