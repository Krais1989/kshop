using KShop.Products.Persistence;
using KShop.Communications.Contracts.Orders;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using KShop.Communications.Contracts.Products;

namespace KShop.Products.Domain.Consumers
{
    public class ProductsReserveCompensationConsumer : IConsumer<ProductsReserveCompensation_BusEvent>
    {
        private readonly ILogger<ProductsReserveConsumer> _logger;
        private readonly ProductsContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;

        public ProductsReserveCompensationConsumer(ILogger<ProductsReserveConsumer> logger, ProductsContext dbContext, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _dbContext = dbContext;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<ProductsReserveCompensation_BusEvent> context)
        {
            var msg = context.Message;
            _logger.LogInformation($"Compensate Reservation: {msg.OrderID}");
            var reservation = _dbContext.ProductReserves.Where(e => e.OrderID == msg.OrderID);
            _dbContext.ProductReserves.RemoveRange(reservation);
            await _dbContext.SaveChangesAsync();
        }
    }
}
