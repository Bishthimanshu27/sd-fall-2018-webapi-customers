using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNet.Identity;
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
    [Authorize]
    public class CustomerController : ApiController
    {
        private ApplicationDbContext Context;

        public CustomerController()
        {
            Context = new ApplicationDbContext();
        }

        [Authorize(Roles = "Admin")]
        public IHttpActionResult Get()
        {
            var userId = User.Identity.GetUserId();

            var model = Context
                .Customers
                .ProjectTo<CustomerViewModel>()
                .ToList();

            return Ok(model);
        }

        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var customer = Context
                .Customers
                .ProjectTo<CustomerViewModel>()
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
                .ProjectTo<CustomerViewModel>()
                .ToList();

            return Ok(customers);
        }

        public IHttpActionResult Post(CustomerBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = Mapper.Map<Customer>(model);
            
            Context.Customers.Add(customer);
            Context.SaveChanges();

            var url = Url.Link("DefaultApi",
                new { Controller = "Customer", Id = customer.Id });

            //return Ok();

            var customerModel = Mapper.Map<CustomerViewModel>(customer);

            return Created(url, customerModel);
        }

        [Route("{id}")]
        public IHttpActionResult Put(int id, CustomerBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = Context.Customers.FirstOrDefault(p => p.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            Mapper.Map(model, customer);

            Context.SaveChanges();

            var customerModel = Mapper.Map<CustomerViewModel>(customer);

            return Ok(customerModel);
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
