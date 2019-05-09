using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPICustomers.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime OrderDate { get; set; }
    }
}