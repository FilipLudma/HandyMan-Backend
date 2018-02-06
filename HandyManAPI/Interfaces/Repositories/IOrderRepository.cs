using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HandyManAPI.Models;

namespace HandyManAPI.Interfaces.Repositories{
    public interface IOrderRepository{
         Task<List<OrderRecord>> GetAll();
         Task<OrderRecord> GetById(Guid id);
         Task<Guid> CreateOrder(OrderRecord request);
         Task<bool> UpdateOrder(Guid id, OrderRecord request);
         Task<List<OrderCategory>> GetCategories();
    }
}