using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPICustomers.Models;
using WebAPICustomers.Models.Domain;

namespace WebAPICustomers.Controllers
{
    [RoutePrefix("api/customer")]
    public class CustomerController : ApiController
    {
        private ApplicationDbContext Context;

        public CustomerController()
        {
            Context = new ApplicationDbContext();
        }

        public IHttpActionResult Get()
        {
            return Ok(Context.Customers.ToList());
        }

        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var customer = Context
                .Customers
                .FirstOrDefault(p => p.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [Route("{search}")]
        public IHttpActionResult Get(string search)
        {
            var customers = Context
                .Customers
                .Where(p => p.FirstName.Contains(search) ||
                p.LastName.Contains(search))
                .ToList();

            return Ok(customers);
        }

        public IHttpActionResult Post(Customer customer)
        {
            Context.Customers.Add(customer);
            Context.SaveChanges();

            var url = Url.Link("DefaultApi",
                new { Controller = "Customer", Id = customer.Id });

            //return Ok();
            return Created(url, customer);
        }

        public IHttpActionResult Put(int id, Customer formData)
        {
            var customer = Context.Customers.FirstOrDefault(p => p.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            customer.FirstName = formData.FirstName;
            customer.LastName = formData.LastName;
            customer.Email = formData.Email;
            customer.PhoneNumber = formData.PhoneNumber;

            Context.SaveChanges();

            return Ok(customer);
        }

        public IHttpActionResult Delete(int id)
        {
            var customer = Context.Customers.FirstOrDefault(p => p.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            Context.Customers.Remove(customer);
            Context.SaveChanges();

            return Ok();
        }
    }
}
