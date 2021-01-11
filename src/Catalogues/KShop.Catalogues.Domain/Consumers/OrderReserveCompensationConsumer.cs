using KShop.Catalogues.Persistence;
using KShop.Communications.Contracts.Orders;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Catalogues.Domain.Consumers
{
    public class OrderReserveCompensationConsumer : IConsumer<IOrderReserveCompensationEvent>
    {
        private readonly ILogger<OrderReserveConsumer> _logger;
        private readonly CatalogueContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderReserveCompensationConsumer(ILogger<OrderReserveConsumer> logger, CatalogueContext dbContext, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _dbContext = dbContext;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<IOrderReserveCompensationEvent> context)
        {
            var msg = context.Message;
            _logger.LogInformation($"Compensate Reservation: {msg.OrderID}");
            var reservation = _dbContext.ProductReserves.Where(e => e.OrderID == msg.OrderID);
            _dbContext.ProductReserves.RemoveRange(reservation);
            await _dbContext.SaveChangesAsync();
        }
    }
}
