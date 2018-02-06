namespace WebAPI.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using WebAPI.Schema.Context;
    using WebAPI.Models;
    using WebAPI.Interfaces.Repositories;
    using Handyman.API_2.Data;

    public class OrdersRepostiory : IOrdersRepository
    {
        private ApplicationDbContext _context;
        private readonly ILogger<OrdersRepostiory> _logger;

        public OrdersRepostiory(ApplicationDbContext context, ILogger<OrdersRepostiory> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<OrdersRecord>> GetAll()
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

        public async Task<OrdersRecord> GetById(Guid id)
        {
            _logger.LogInformation("START Get ORDER by id");
            _logger.LogInformation("INPUT ID {0}", id);

            var orderRecord = await _context.Orders
                //.Include(inc => inc.OrderContactOptions)
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

        public async Task<Guid> CreateOrder(OrdersRecord record)
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

        public async Task<bool> UpdateOrder(Guid id, OrdersRecord record)
        {
            _logger.LogInformation("START Update Order");
            _logger.LogInformation("INPUT parameters Id:{0} Object{1}", id, record);

            bool response = false;
            using (ApplicationDbContext context = _context)
            {
                try
                {
                    record.OrderGuid = id;
                    context.Entry(record).State = EntityState.Modified;
                    int result = await context.SaveChangesAsync();

                    if (result != 0)
                    {
                        response = true;
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