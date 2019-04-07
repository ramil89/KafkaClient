using System;
using System.Collections.Generic;
using System.Text;

namespace KafkaClient.Tests.Models
{
    public class TestOrder
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }

        public List<string> OrderDetails { get; set; }

        public double Total { get; set; }

        public double DeliveryPrice { get; set; }

        public double PaymentPrice { get; set; }

        public int UserId { get; set; }
    }
}
