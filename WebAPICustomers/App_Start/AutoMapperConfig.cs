using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAPICustomers.Models;
using WebAPICustomers.Models.Domain;

namespace WebAPICustomers.App_Start
{
    public static class AutoMapperConfig
    {
        public static void Init()
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<Customer, CustomerViewModel>().ReverseMap();
                cfg.CreateMap<Customer, CustomerBindingModel>().ReverseMap();
                cfg.CreateMap<Order, OrderBindingModel>().ReverseMap();
                cfg.CreateMap<Order, OrderViewModel>().ReverseMap();
            });
        }
    }
}