
using KShop.Products.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MarketCLI
{
    public class CatalogueTruncateCommand : IBaseCommandAsync
    {
        private readonly ILogger<CatalogueTruncateCommand> _logger;
        private readonly ProductsContext _dbContext;

        public CatalogueTruncateCommand(ILogger<CatalogueTruncateCommand> logger, ProductsContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task ExecuteAsync(CancellationToken cancelToken)
        {
            var tables = new List<string>() { "ProductPositions", "ProductReserves", "Products" };

            var sqlBuilder = new StringBuilder().AppendLine($"SET foreign_key_checks = 0;");

            foreach (var table in tables)
                sqlBuilder.AppendLine($"truncate table {table};");

            int count = _dbContext.Database.ExecuteSqlRaw(sqlBuilder.ToString());
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Result: {count}");
        }
    }
}
