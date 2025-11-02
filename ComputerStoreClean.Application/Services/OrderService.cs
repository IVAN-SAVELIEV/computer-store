using AutoMapper;
using ComputerStoreClean.Application.DTOs;
using ComputerStoreClean.Application.Interfaces;
using ComputerStoreClean.Domain.Common;
using ComputerStoreClean.Domain.Entities;
using ComputerStoreClean.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerStoreClean.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public OrderService(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetOrdersWithItemsAndProductsAsync();
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetOrderWithDetailsAsync(id);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {id} not found.");

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            // Валидация продуктов и расчет общей суммы
            decimal totalAmount = 0;
            var orderItems = new List<OrderItem>();

            foreach (var itemDto in createOrderDto.OrderItems)
            {
                var product = await _productRepository.GetByIdAsync(itemDto.ProductId);
                if (product == null)
                    throw new ArgumentException($"Product with ID {itemDto.ProductId} not found.");

                if (product.StockQuantity < itemDto.Quantity)
                    throw new InvalidOperationException($"Insufficient stock for product '{product.Name}'. Available: {product.StockQuantity}, Requested: {itemDto.Quantity}");

                totalAmount += product.Price * itemDto.Quantity;

                orderItems.Add(new OrderItem
                {
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    UnitPrice = product.Price
                });

                // Обновляем количество на складе
                product.StockQuantity -= itemDto.Quantity;
                _productRepository.Update(product);
            }

            var order = new Order
            {
                OrderNumber = GenerateOrderNumber(),
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                Status = OrderStatus.Pending,
                CustomerName = createOrderDto.CustomerName,
                CustomerEmail = createOrderDto.CustomerEmail,
                CustomerPhone = createOrderDto.CustomerPhone,
                ShippingAddress = createOrderDto.ShippingAddress,
                CreatedAt = DateTime.UtcNow
            };

            foreach (var orderItem in orderItems)
            {
                order.OrderItems.Add(orderItem);
            }

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();

            return await GetOrderByIdAsync(order.Id);
        }

        public async Task UpdateOrderStatusAsync(int id, OrderStatus status)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {id} not found.");

            order.Status = status;
            order.UpdatedAt = DateTime.UtcNow;

            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _orderRepository.GetOrderWithDetailsAsync(id);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {id} not found.");

            // Восстанавливаем количество продуктов на складе
            foreach (var item in order.OrderItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.StockQuantity += item.Quantity;
                    _productRepository.Update(product);
                }
            }

            _orderRepository.Delete(order);
            await _orderRepository.SaveChangesAsync();
        }

        private string GenerateOrderNumber()
        {
            return $"ORD-{DateTime.UtcNow:yyyyMMdd-HHmmss}-{Guid.NewGuid().ToString()[..8].ToUpper()}";
        }
    }
}
