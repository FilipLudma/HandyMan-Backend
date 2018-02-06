using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Mail;

using AutoMapper;
using HandyManAPI.Inputs;
using HandyManAPI.Interfaces.Repositories;
using HandyManAPI.Models;

using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace HandyManAPI.Controllers
{

    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    public class OrdersController : Controller
    {

        #region Private properties

        private IOrderRepository _orderRepository;
        private ILogger _logger;

        #endregion Private properties

        #region Public Constructors

        public OrdersController(IOrderRepository orderRepository, ILogger<OrdersController> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        #endregion Public Constructors

        #region Public Methods

        public IActionResult Index()
        {
            return View();
        }

        // GET api/values
        [HttpGet]
        public async Task<List<OrderRecord>> Orders()
        {
            _logger.LogInformation("Get order");
            return await _orderRepository.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<OrderDto> Order(Guid id)
        {
            _logger.LogInformation("Get orders");
            var orderDto = new OrderDto();
            try
            {
                var orderRecord = await _orderRepository.GetById(id);

                if (orderRecord != null)
                {
                    orderDto = Mapper.Map<OrderDto>(orderRecord);

                    if (orderRecord.ImgAttachments != null)
                    {
                        orderDto.ImgAttachments = new List<ImageAttachment>();
                        foreach (var attachment in orderRecord.ImgAttachments)
                        {
                            var imgAttachment = new ImageAttachment();
                            imgAttachment.ImgBase64 = Convert.ToBase64String(attachment.ImageBlob);
                            imgAttachment.FileName = attachment.FileName;

                            orderDto.ImgAttachments.Add(imgAttachment);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return orderDto;
        }

        [HttpGet]
        public async Task<List<OrderCategory>> GetCategories()
        {
            _logger.LogInformation("Get categories");
            var categories = await _orderRepository.GetCategories();

            return categories;
        }

        [HttpPost]
        public async Task<Guid> CreateOrder()
        {
            _logger.LogInformation("Create order");
            OrderRecord order = new OrderRecord();
            order.OrderGuid = Guid.NewGuid();

            Guid orderId = await _orderRepository.CreateOrder(order);
            _logger.LogInformation("New ORDER id {0}", orderId);

            return orderId;
        }

        [HttpPost]
        public async Task<Guid> CreateOrderWithBody([FromBody]OrderDto request)
        {
            _logger.LogInformation("Create order");
            _logger.LogDebug("Request", request);

            OrderRecord order = Mapper.Map<OrderRecord>(request);
            order.OrderGuid = Guid.NewGuid();

            Guid orderId = await _orderRepository.CreateOrder(order);
            _logger.LogInformation("New ORDER id {0}", orderId);

            return orderId;
        }

        [HttpPut("{id}")]
        public async Task<bool> UpdateOrder(Guid id, [FromBody]OrderDto request)
        {
            _logger.LogInformation("UPDATE order");

            var response = false;
            var orderRecord = Mapper.Map<OrderRecord>(request);

            if (orderRecord != null)
            {
                if (request.ImgAttachments != null && request.ImgAttachments.Count > 0)
                {
                    foreach (var attachment in request.ImgAttachments)
                    {
                        ImageModel image = new ImageModel();
                        image.ImageBlob = Convert.FromBase64String(attachment.ImgBase64);
                        image.FileName = attachment.FileName;
                        image.ImageModelID = Guid.NewGuid();

                        orderRecord.ImgAttachments.Add(image);
                    }
                }

                _logger.LogInformation("Order record {0}", orderRecord);

                return await _orderRepository.UpdateOrder(id, orderRecord);
            }

            _logger.LogInformation("Response {0}", response);
            return response;
        }

        [HttpDelete("{id}")]
        public void DeleteOrder(Guid id)
        {
            _logger.LogInformation("START Delete Order");
        }

        #endregion Public Methods
    }
}