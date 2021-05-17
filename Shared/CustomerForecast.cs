using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace CustomerData.Shared
{
    public class CustomerForecast
    {
        public int customer_id { get; set; }
        public string name { get; set; }
        public string street { get; set; }
        public string zip { get; set; }
        public string city { get; set; }
    }
}
