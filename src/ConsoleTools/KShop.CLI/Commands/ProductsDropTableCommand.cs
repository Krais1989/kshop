
using KShop.Products.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MarketCLI
{

    public class ProductsDropTableCommand : IBaseCommandAsync
    {
        private readonly ILogger<ProductsDropTableCommand> _logger;
        private readonly ProductsContext _dbContext;

        public ProductsDropTableCommand(ILogger<ProductsDropTableCommand> logger, ProductsContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task ExecuteAsync(CancellationToken cancelToken)
        {
            var tables = new List<string>() { "ProductPositions", "ProductReserves", "Products" };

            foreach (var table in tables)
            {
                var sql = $"drop table {table};";
                _logger.LogInformation(sql);
                int count = _dbContext.Database.ExecuteSqlRaw(sql);
                _logger.LogInformation($"Result: {count}");
            }
            //await _dbContext.SaveChangesAsync(cancelToken);
        }
    }
}
