using KShop.Catalogues.Persistence;
using KShop.Communications.Contracts.Orders;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Catalogues.Domain.Consumers
{
    public class OrderReservationCompensationConsumer : IConsumer<IOrderReserveCompensationEvent>
    {
        private readonly ILogger<OrderReservationConsumer> _logger;
        private readonly CatalogueContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderReservationCompensationConsumer(ILogger<OrderReservationConsumer> logger, CatalogueContext dbContext, IPublishEndpoint publishEndpoint)
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
