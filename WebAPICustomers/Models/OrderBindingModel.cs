using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAPICustomers.Models
{
    public class OrderBindingModel
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}