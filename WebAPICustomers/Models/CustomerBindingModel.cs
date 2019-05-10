using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAPICustomers.Models
{
    /// <summary>
    /// This is class is used to receive paramters to create customers
    /// </summary>
    public class CustomerBindingModel
    {
        /// <summary>
        /// This is the first name of the Customer.
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}