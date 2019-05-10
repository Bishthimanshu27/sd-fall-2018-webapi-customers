﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    [Authorize]
    [RoutePrefix("api/orders")]
    public class OrderController : ApiController
    {
        private ApplicationDbContext Context { get; set; }

        public OrderController()
        {
            Context = new ApplicationDbContext();
        }

        [HttpGet]
        [Route("get-all")]
        public IHttpActionResult GetAll()
        {
            var model = Context
                .Orders
                .ProjectTo<OrderViewModel>()
                .ToList();

            return Ok(model);
        }

        [HttpGet]
        [Route("get-by-id/{id:int}", Name = "GetOrderById")]
        public IHttpActionResult GetById(int id)
        {
            var model = Context
                .Orders
                .ProjectTo<OrderViewModel>()
                .FirstOrDefault(p => p.Id == id);

            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }

        public class ErroResult
        {
            public string Property { get; set; }
            public string ErrorMessage { get; set; }
        }

        [HttpPost]
        [Route("create")]
        public IHttpActionResult Create(OrderBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerExist = Context
                .Customers
                .Any(p => p.Id == model.CustomerId);

            if (!customerExist)
            {
                ModelState.AddModelError("CustomerId", "Invalid customer ID");
                //var error = new ErroResult();
                //error.Property = "CustomerId";
                //error.ErrorMessage = "Invalid customer ID";
                //return BadRequest(error);
                return BadRequest(ModelState);
            }

            var order = Mapper.Map<Order>(model);
            order.OrderDate = DateTime.Now;

            Context.Orders.Add(order);
            Context.SaveChanges();

            var result = Mapper.Map<OrderViewModel>(order);

            //return Ok(order);
            return Created(
                Url.Link("GetOrderById", new { id = order.Id }),
                result);
        }
    }
}