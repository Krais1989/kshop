using MassTransit.Courier;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KShop.Orders.Domain.RoutingSlips
{

    public class InvoiceCreateCourierArguments
    {
        public Guid OrderID { get; set; }
        public int CustomerID { get; set; }
    }

    public class InvoiceCreateCourierLog
    {
        public Guid InvoiceID { get; set; }
    }

    public class InvoiceCreateCourierActivity : IActivity<InvoiceCreateCourierArguments, InvoiceCreateCourierLog>
    {
        private readonly ILogger<InvoiceCreateCourierActivity> _logger;

        public InvoiceCreateCourierActivity(ILogger<InvoiceCreateCourierActivity> logger)
        {
            _logger = logger;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<InvoiceCreateCourierArguments> context)
        {
            //throw new Exception("TEST EXCEPTION");
            return context.Faulted(new Exception("=>> TEST FAILURE <<="));

            var invoiceId = Guid.NewGuid();
            _logger.LogInformation($"EXECUTE> OrderID: {context.Arguments.OrderID} InvoiceID:{invoiceId}");
            return context.Completed(new { InvoiceID = invoiceId });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<InvoiceCreateCourierLog> context)
        {
            _logger.LogInformation($"COMPENSATE> InvoiceID: {context.Log.InvoiceID}");
            return context.Compensated();
        }
    }

}
