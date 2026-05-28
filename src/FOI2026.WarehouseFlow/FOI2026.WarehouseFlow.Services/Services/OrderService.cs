using FOI2026.WarehouseFlow.Infrastructure.Data.Models;
using FOI2026.WarehouseFlow.Services.Repository_Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FOI2026.WarehouseFlow.Services.Services
{
    public class OrderService
    {

        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {

            var orders = await _orderRepository.GetAllAsync();
            return orders;

        }

        public async Task<Order?> GetByIdAsync(int id)
        {

            if (id < 0)
                return null;

            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
                return null;

            return order;
        }

        public async Task AddOrderAsync (Order order)
        {
            if(order == null)
                return;
          await _orderRepository.AddAsync(order);
        }

        public async Task UpdateOrderAsync(Order order) 
        { 
            if (order == null)
                return;
            await _orderRepository.UpdateAsync(order);
        }

        public async Task DeleteOrderAsync(int id)
        { 
            if(id < 0)
                return;
            await _orderRepository.DeleteAsync(id);

        }

    }
}
