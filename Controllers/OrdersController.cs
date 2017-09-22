namespace WebAPI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.Logging;

    using WebAPI.Inputs;
    using WebAPI.Interfaces.Repositories;
    using WebAPI.Models;

    [Authorize]
    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    public class OrdersController : Controller
    {
        private IOrdersRepository _ordersRepository;
        private ILogger _logger;

        public OrdersController(IOrdersRepository ordersRepository, ILogger<OrdersController> logger)
        {
            _ordersRepository = ordersRepository;
            _logger = logger;
        }

        // GET api/values
        [HttpGet]
        public async Task<List<OrdersRecord>> Order()
        {
            _logger.LogInformation("Get order");
            return await _ordersRepository.GetAll();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<OrdersRecord> Orders(Guid id)
        {
            _logger.LogInformation("Get orders");
            return await _ordersRepository.GetById(id);
        }

        // GET api/values
        [HttpGet]
        public async Task<List<OrderCategory>> GetCategories()
        {
            _logger.LogInformation("Get categories");
            var categories = await _ordersRepository.GetCategories();

            return categories;
        }

        // POST api/values
        [HttpPost]
        public async Task<Guid> CreateOrder()
        {
            _logger.LogInformation("Create order");
            OrdersRecord order = new OrdersRecord();
            order.OrderGuid = Guid.NewGuid();
            
            Guid orderId = await _ordersRepository.CreateOrder(order);
            _logger.LogInformation("New ORDER id {0}", orderId);

            return orderId;
        }

        [HttpPost]
        public async Task<Guid> CreateOrderWithBody([FromBody]OrderDto request)
        {
            _logger.LogInformation("Create order");
            _logger.LogDebug("Request", request);

            OrdersRecord order = Mapper.Map<OrdersRecord>(request);
            order.OrderGuid = Guid.NewGuid();

            Guid orderId = await _ordersRepository.CreateOrder(order);
            _logger.LogInformation("New ORDER id {0}", orderId);

            return orderId;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<bool> UpdateOrder(Guid id, [FromBody]OrderDto request)
        {
            _logger.LogInformation("UPDATE order");

            var response = false;
            var orderRecord = Mapper.Map<OrdersRecord>(request);

            if (orderRecord != null)
            {
                _logger.LogInformation("Order record {0}", orderRecord);
                response = await _ordersRepository.UpdateOrder(id, orderRecord);
            }

            _logger.LogInformation("Response {0}", response);
            return response;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void DeleteOrder(Guid id)
        {
            _logger.LogInformation("START Delete Order");
        }
    }
}
