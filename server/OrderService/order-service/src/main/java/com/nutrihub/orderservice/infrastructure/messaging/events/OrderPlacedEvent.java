package com.nutrihub.orderservice.infrastructure.messaging.events;

import java.io.Serializable;
import java.time.LocalDateTime;
import java.util.List;
import java.util.UUID;

public record OrderPlacedEvent(
        UUID orderId,
        UUID customerId,
        List<OrderItemEvent> items,
        double totalAmount,
        LocalDateTime placedAt
) implements Serializable {
    public record OrderItemEvent(
            UUID productId,
            String productName,
            int quantity,
            double unitPrice
    ) implements Serializable { }
}
