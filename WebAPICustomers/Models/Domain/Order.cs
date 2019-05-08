using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPICustomers.Models.Domain
{
    public class Order
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public DateTime OrderDate { get; set; }

        public virtual Customer Customer { get; set; }
        public int CustomerId { get; set; }
    }
}