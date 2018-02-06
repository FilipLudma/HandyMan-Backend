namespace HandyManAPI.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using HandyManAPI.Models;
    using HandyManAPI.Interfaces.Repositories;
    using HandyManAPI.Schema.Context;

    public class OrderRepostiory : IOrderRepository
    {
        private HandyManContext _context;
        private readonly ILogger<OrderRepostiory> _logger;

        public OrderRepostiory(HandyManContext context, ILogger<OrderRepostiory> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<OrderRecord>> GetAll()
        {
            _logger.LogInformation("START Get all ORDERS");

            var orders = await _context.Orders
                //.Include(inc => inc.OrderContactOptions)
                .AsNoTracking()
                .ToListAsync();

            if (orders == null)
            {
                _logger.LogError("Orders not found");
                return null;
            }

            _logger.LogInformation("END Get all ORDERS");
            return orders;
        }

        public async Task<OrderRecord> GetById(Guid id)
        {
            _logger.LogInformation("START Get ORDER by id");
            _logger.LogInformation("INPUT ID {0}", id);

            var orderRecord = await _context.Orders
                .Include(inc => inc.ImgAttachments)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.OrderGuid == id);

            if (orderRecord == null)
            {
                _logger.LogInformation("Order record with id:{0} does not exist", id);
                return null;
            }

            _logger.LogInformation("END Get ORDER by id");
            return orderRecord;
        }

        public async Task<Guid> CreateOrder(OrderRecord record)
        {
            _logger.LogInformation("START Create ORDER");
            _logger.LogInformation("INPUT object {0}", record);

            Guid response;
            using (var db = _context)
            {
                try
                {
                    var order = record;
                    db.Orders.Add(order);
                    await db.SaveChangesAsync();

                    response = order.OrderGuid;
                }
                catch (Exception ex)
                {
                    _logger.LogError("Save New Order error {0}", ex);
                }
            }

            _logger.LogInformation("END Create ORDER");
            return response;
        }

        public async Task<bool> UpdateOrder(Guid id, OrderRecord record)
        {
            _logger.LogInformation("START Update Order");
            _logger.LogInformation("INPUT parameters Id:{0} Object{1}", id, record);

            bool response = false;
            record.OrderGuid = id;

            using (HandyManContext context = _context)
            {
                try
                {
                    var existingOrder = await context.Orders
                        .Include(inc => inc.ImgAttachments)
                        .FirstOrDefaultAsync(ord => ord.OrderGuid == id);


                    if (existingOrder != null)
                    {
                        // Update parent
                        context.Entry(existingOrder).CurrentValues.SetValues(record);

                        // Delete ImgAttachment
                        foreach (var existingAttachment in existingOrder.ImgAttachments.ToList())
                        {
                            if (!record.ImgAttachments.Any(c => c.ImageModelID == existingAttachment.ImageModelID))
                                context.ImageModel.Remove(existingAttachment);
                        }

                        // Update and Insert children
                        foreach (var attachment in record.ImgAttachments)
                        {
                            var existingAttachment = existingOrder.ImgAttachments
                                .Where(c => c.ImageModelID == attachment.ImageModelID)
                                .SingleOrDefault();

                            if (existingAttachment != null)
                                // Update child
                                context.Entry(existingAttachment).CurrentValues.SetValues(attachment);
                            else
                            {
                                existingOrder.ImgAttachments.Add(attachment);
                            }
                        }

                         return await context.SaveChangesAsync() > 0;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogInformation("Update order ERROR {0}", ex);
                }
            }

            _logger.LogInformation("END Update Order");
            return response;
        }

        public async Task<List<OrderCategory>> GetCategories()
        {
            _logger.LogInformation("START Get all Categories");

            var categories = await _context.OrderCategory
                .Include(inc => inc.OrderSubCategories)
                .AsNoTracking()
                .ToListAsync();

            if (categories == null)
            {
                _logger.LogError("Categories not found");
                return null;
            }

            _logger.LogInformation("END Get all ORDERS");
            return categories;
        }
    }
}