using ComputerStoreClean.Application.DTOs;
using ComputerStoreClean.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerStoreClean.Application.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        Task<OrderDto> GetOrderByIdAsync(int id);
        Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto);
        Task UpdateOrderStatusAsync(int id, OrderStatus status);
        Task DeleteOrderAsync(int id);
    }
}
