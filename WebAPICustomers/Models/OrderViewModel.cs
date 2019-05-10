using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPICustomers.Models
{
    public class OrderViewModel
    {
        /// <summary>
        /// The id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The amount
        /// </summary>
        public decimal Amount { get; set; }
        public DateTime OrderDate { get; set; }
    }
}