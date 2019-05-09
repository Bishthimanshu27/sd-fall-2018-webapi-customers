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
    [RoutePrefix("api/customer-custom")]
    public class CustomerCustomRoutesController : ApiController
    {
        private ApplicationDbContext Context;

        public CustomerCustomRoutesController()
        {
            Context = new ApplicationDbContext();
        }

        [HttpGet]
        [Route("get-all")]
        public IHttpActionResult GetAll()
        {
            return Ok(Context.Customers.ToList());
        }

        [HttpGet]
        [Route("get-by-id/{id:int}", Name = "GetCustomerById")]
        public IHttpActionResult GetById(int id)
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

        [Route("search/{search}")]
        [HttpGet]
        public IHttpActionResult Search(string search)
        {
            var customers = Context
                .Customers
                .Where(p => p.FirstName.Contains(search) ||
                p.LastName.Contains(search))
                .ToList();

            return Ok(customers);
        }

        [HttpPost]
        [Route("create")]
        public IHttpActionResult Create(CustomerBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = new Customer();
            customer.FirstName = model.FirstName;
            customer.LastName = model.LastName;
            customer.Email = model.Email;
            customer.PhoneNumber = model.PhoneNumber;

            Context.Customers.Add(customer);
            Context.SaveChanges();

            var url = Url.Link("GetCustomerById",
                new { Id = customer.Id });

            //return Ok();
            var customerModel = new CustomerViewModel();
            customerModel.Id = customer.Id;
            customerModel.FirstName = customer.FirstName;
            customerModel.LastName = customer.LastName;
            customerModel.PhoneNumber = customer.PhoneNumber;
            customerModel.Email = customer.Email;

            return Created(url, customerModel);
        }

        [HttpPut]
        [Route("update/{id:int}")]
        public IHttpActionResult Update(int id, Customer formData)
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

        [HttpDelete]
        [Route("delete/{id:int}")]
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
