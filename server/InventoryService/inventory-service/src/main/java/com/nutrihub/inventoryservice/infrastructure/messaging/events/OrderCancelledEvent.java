package com.nutrihub.inventoryservice.infrastructure.messaging.events;

import java.time.LocalDateTime;
import java.util.List;
import java.util.UUID;

public record OrderCancelledEvent(
        UUID orderId,
        UUID customerId,
        List<OrderItemEvent> items,
        LocalDateTime cancelledAt
) {
    public record OrderItemEvent(
            UUID productId,
            String productName,
            int quantity
    ) {}
}