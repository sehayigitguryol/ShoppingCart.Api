using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Infrastructure.Configurations
{
    public class MongoDbConfigurations
    {
        public string Database { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string MasterDatabaseName{ get; set; }
    }
}
