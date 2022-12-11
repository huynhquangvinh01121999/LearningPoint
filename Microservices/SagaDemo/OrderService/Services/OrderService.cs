﻿using Saga.Services.OrderService.Entities;
using Saga.Services.OrderService.Repositories;

namespace Saga.Services.OrderService.Services;


public interface IOrderService
{
    Order GetById(Guid orderId);
    Order CreateOrder(Order order);
    Order AcceptOrder(Guid id);
    Order RejectOrder(Guid id);
    Order SetOrderAsFailed(Guid id);
    IEnumerable<Order> GetAll();
}
public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<OrderService> _logger;
    public OrderService(IOrderRepository orderRepository,ILogger<OrderService> logger)
    {
        _orderRepository = orderRepository;
        _logger = logger;
    }

    public Order GetById(Guid orderId)
    {
        _logger.LogInformation($"Retrieve order information for #{orderId}");
        var result = _orderRepository.Get(orderId);

        if (result is null)
        {
            _logger.LogInformation($"OrderId #{orderId} not found");
            throw new Exception($"OrderId #{orderId} not found");
        }

        return result;
    }
    public IEnumerable<Order> GetAll()
    {
        _logger.LogInformation($"Retrieving all orders");
        return _orderRepository.GetAll();
    }
    public Order AcceptOrder(Guid id)
    {
        _logger.LogInformation($"Preparing to accept Order #{id}");
        return UpdateOrder(id, OrderState.Accepted);
    }

    private Order UpdateOrder(Guid id, OrderState state)
    {
        var order = _orderRepository.Get(id);

        if (order is null)
        {
            throw new ArgumentOutOfRangeException("Order not found");
        }
        order.State = state;
        return _orderRepository.Update(order) ?? throw new InvalidOperationException("Unable to update order");
    }

    public Order CreateOrder(Order order)
    {
        _logger.LogInformation("Inserting new Order");
        order.State = OrderState.Pending;
        return _orderRepository.Insert(order);
    }

    public Order RejectOrder(Guid id)
    {
        _logger.LogInformation($"Preparing to reject Order #{id}");
        return UpdateOrder(id, OrderState.Rejected);
    }

    public Order SetOrderAsFailed(Guid id)
    {
        _logger.LogInformation($"Order (#{id}) Creation Failed.");
        return UpdateOrder(id, OrderState.Failed);
    }
}
