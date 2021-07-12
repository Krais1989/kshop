using KShop.Shared.Domain.Contracts;
using System;

namespace KShop.Shared.Integration.Contracts
{
    public class OrderSetStatusReservedSvcRequest
    {
        public OrderSetStatusReservedSvcRequest()
        {
        }

        public OrderSetStatusReservedSvcRequest(uint userID, Guid orderID, string comment)
        {
            UserID = userID;
            OrderID = orderID;
            Comment = comment;
        }

        public uint UserID { get; private set; }
        public Guid OrderID { get; private set; }
        public string Comment { get; private set; }
    }

    public class OrderSetStatusPayedSvcRequest
    {
        public OrderSetStatusPayedSvcRequest()
        {
        }

        public OrderSetStatusPayedSvcRequest(uint userID, Guid orderID, string comment)
        {
            UserID = userID;
            OrderID = orderID;
            Comment = comment;
        }

        public uint UserID { get; private set; }
        public Guid OrderID { get; private set; }
        public string Comment { get; private set; }
    }

    public class OrderSetStatusShippedSvcRequest
    {
        public OrderSetStatusShippedSvcRequest()
        {
        }

        public OrderSetStatusShippedSvcRequest(uint userID, Guid orderID, string comment)
        {
            UserID = userID;
            OrderID = orderID;
            Comment = comment;
        }

        public uint UserID { get; private set; }
        public Guid OrderID { get; private set; }
        public string Comment { get; private set; }
    }

    public class OrderSetStatusFaultedSvcRequest
    {
        public OrderSetStatusFaultedSvcRequest()
        {
        }

        public OrderSetStatusFaultedSvcRequest(uint userID, Guid orderID, string comment)
        {
            UserID = userID;
            OrderID = orderID;
            Comment = comment;
        }

        public uint UserID { get; private set; }
        public Guid OrderID { get; private set; }
        public string Comment { get; private set; }
    }

    public class OrderSetStatusRefundedSvcRequest
    {
        public OrderSetStatusRefundedSvcRequest()
        {
        }

        public OrderSetStatusRefundedSvcRequest(uint userID, Guid orderID, string comment)
        {
            UserID = userID;
            OrderID = orderID;
            Comment = comment;
        }

        public uint UserID { get; private set; }
        public Guid OrderID { get; private set; }
        public string Comment { get; private set; }
    }
    public class OrderSetStatusCancelledSvcRequest
    {
        public OrderSetStatusCancelledSvcRequest()
        {
        }

        public OrderSetStatusCancelledSvcRequest(uint userID, Guid orderID, string comment)
        {
            UserID = userID;
            OrderID = orderID;
            Comment = comment;
        }

        public uint UserID { get; private set; }
        public Guid OrderID { get; private set; }
        public string Comment { get; private set; }
    }


    public class OrderSetStatusSvcResponse : BaseResponse
    {
    }

}

