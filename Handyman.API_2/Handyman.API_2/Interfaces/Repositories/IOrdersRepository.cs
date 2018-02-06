using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.Repositories{
    public interface IOrdersRepository{
         Task<List<OrdersRecord>> GetAll();
         Task<OrdersRecord> GetById(Guid id);
         Task<Guid> CreateOrder(OrdersRecord request);
         Task<bool> UpdateOrder(Guid id, OrdersRecord request);

         Task<List<OrderCategory>> GetCategories();
    }
}