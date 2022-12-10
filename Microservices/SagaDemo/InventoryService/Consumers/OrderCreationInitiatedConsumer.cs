﻿using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Saga.Services.InventoryService.Dtos;
using Saga.Services.InventoryService.Services;
using Saga.Shared.Contracts.Events;

namespace Saga.Services.InventoryService.Consumers;

public class OrderCreationInitiatedConsumer : IConsumer<OrderCreationInitiated>
{
    private readonly IInventoryService _inventoryService;
    private readonly IPublishEndpoint _publishEndPoint;
    public OrderCreationInitiatedConsumer([FromServices]IInventoryService inventoryService,
                                          [FromServices] IPublishEndpoint publishEndPoint)
    {
        _inventoryService = inventoryService;
        _publishEndPoint = publishEndPoint;
    }
    public Task Consume(ConsumeContext<OrderCreationInitiated> context)
    {
        var orderInfo = new OrderDto
        {
            OrderId = context.Message.OrderId,
            Items = context.Message.OrderItems.ToDictionary(x => x.ItemId, y => y.Qty)
        };

        try
        {
            _inventoryService.ReserveStock(orderInfo);
        }
        catch(Exception ex)
        {
            _publishEndPoint.Publish(new OrderCreationFailed
            {
                OrderId = context.Message.OrderId
            });
        }

        return Task.CompletedTask;
    }
}
