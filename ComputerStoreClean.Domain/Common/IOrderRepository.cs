using ComputerStoreClean.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerStoreClean.Domain.Common
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersWithItemsAndProductsAsync();
        Task<Order> GetOrderWithDetailsAsync(int id);
    }
}
