using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.ServiceBus
{
    public class RabbitConnection
    {
        public string HostName { get; set; }
        public string VirtualHost { get; set; }
        public ushort Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
