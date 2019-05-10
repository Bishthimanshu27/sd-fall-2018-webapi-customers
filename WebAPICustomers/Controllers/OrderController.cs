using AutoMapper;
using AutoMapper.QueryableExtensions;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
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

        /// <summary>
        /// Returns all orders in the system
        /// </summary>
        /// <returns>Returns a list of orders</returns>
        [HttpGet]
        [Route("get-all")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<OrderViewModel>))]
        public IHttpActionResult GetAll()
        {
            var model = Context
                .Orders
                .ProjectTo<OrderViewModel>()
                .ToList();

            return Ok(model);
        }

        /// <summary>
        /// Get an order by ID
        /// </summary>
        /// <param name="id">The id of the order</param>
        /// <returns>The order</returns>
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

        

        [HttpPost]
        [Route("create")]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(OrderViewModel))]
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
                return BadRequest(ModelState);

                //var error = new ErrorResult();
                //error.Property = "CustomerId";
                //error.ErrorMessage = "Invalid customer ID";

                //var response = Request.CreateResponse(
                //        HttpStatusCode.BadRequest, error);

                //return ResponseMessage(response);
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
