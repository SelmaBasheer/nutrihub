package com.nutrihub.orderservice.infrastructure.messaging.events;

import java.io.Serializable;
import java.time.LocalDateTime;
import java.util.List;
import java.util.UUID;

public record OrderCancelledEvent(
        UUID orderId,
        UUID customerId,
        List<OrderItemEvent> items,
        LocalDateTime cancelledAt
) implements Serializable {
    public record OrderItemEvent(
            UUID productId,
            String productName,
            int quantity
    ) implements Serializable {}
}
