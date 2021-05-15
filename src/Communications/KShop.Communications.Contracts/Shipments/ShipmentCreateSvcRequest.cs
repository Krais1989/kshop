using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Communications.Contracts.Shipments
{
    public class ShipmentCreateSvcRequest
    {
        public Guid OrderID { get; set; }
        public IDictionary<int,int> OrderPositions { get; set; }
    }

    public class ShipmentCreateSvcResponse
    {
        public Guid? ShipmentID { get; set; }
    }
}
