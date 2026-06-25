package com.nutrihub.orderservice.application.command;

import com.nutrihub.orderservice.domain.enums.OrderStatus;

import java.util.UUID;

public record UpdateOrderStatusCommand(
        UUID orderId,
        OrderStatus newStatus
) { }
