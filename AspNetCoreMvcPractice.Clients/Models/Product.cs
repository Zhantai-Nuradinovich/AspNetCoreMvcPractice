using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreMvcPractice.Clients.Models
{
    public class Product
    {
        public string ProductName { get; set; }

        public string QuantityPerUnit { get; set; }

        public decimal UnitPrice { get; set; }

        public Category Category { get; set; }
    }
}
