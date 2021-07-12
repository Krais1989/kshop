using FluentValidation;
using KShop.Orders.Persistence;
using KShop.Shared.Domain.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Orders.Domain
{

    public class GetCustomerOrdersResponse : BaseResponse
    {
        public GetCustomerOrdersResponse(List<OrderDetails> orders)
        {
            Orders = orders;
        }

        public List<OrderDetails> Orders { get; private set; }
    }
    public class GetCustomerOrdersRequest : IRequest<GetCustomerOrdersResponse>
    {
        public GetCustomerOrdersRequest(ulong userID)
        {
            UserID = userID;
        }

        public ulong UserID { get; private set; }
    }
    public class GetCustomerOrdersMediatorHandler : IRequestHandler<GetCustomerOrdersRequest, GetCustomerOrdersResponse>
    {
        private readonly ILogger<GetCustomerOrdersMediatorHandler> _logger;
        private readonly OrderContext _orderContext;

        public GetCustomerOrdersMediatorHandler(
            ILogger<GetCustomerOrdersMediatorHandler> logger,
            OrderContext orderContext)
        {
            _logger = logger;
            _orderContext = orderContext;
        }

        public async Task<GetCustomerOrdersResponse> Handle(GetCustomerOrdersRequest request, CancellationToken cancellationToken)
        {
            var rawResult = await _orderContext.Orders.AsNoTracking()
                .Include(e => e.Logs)
                .Include(e => e.Positions)
                .Where(e => e.CustomerID == request.UserID)
                .Select(e => new OrderDetails
                {
                    ID = e.ID,
                    CreateDate = e.CreateDate,
                    Status = e.Status,
                    Price = e.Price,
                    Logs = e.Logs.Select(l => new OrderDetails.Log { Date = l.StatusDate, Status = l.NewStatus }),
                    Positions = e.Positions.Select(pos => new OrderDetails.Position { ProductID = pos.ProductID, Quantity = pos.Quantity })
                })
                .OrderByDescending(e => e.CreateDate)
                .ToListAsync();


            //var result = rawResult.Select(e => new OrderDetails()
            //{
            //       ID = e.ID,
            //       Status = e.Status,
            //       CreateTime = e.CreateTime,
            //       Logs = e.Logs.ToList(),
            //       Positions
            //});

            return new GetCustomerOrdersResponse(rawResult);
        }
    }
}
