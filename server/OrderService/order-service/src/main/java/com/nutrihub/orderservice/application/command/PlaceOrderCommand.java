package com.nutrihub.orderservice.application.command;

import com.nutrihub.orderservice.application.dto.PlaceOrderRequest;

import java.util.List;
import java.util.UUID;

public record PlaceOrderCommand(
        UUID customerId,
        List<PlaceOrderRequest.OrderItemRequest> items
) { }
