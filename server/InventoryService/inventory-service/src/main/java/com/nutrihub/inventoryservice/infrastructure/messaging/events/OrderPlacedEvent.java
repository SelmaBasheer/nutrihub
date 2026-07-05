package com.nutrihub.inventoryservice.infrastructure.messaging.events;

import java.time.LocalDateTime;
import java.util.List;
import java.util.UUID;

public record OrderPlacedEvent(
        UUID orderId,
        UUID customerId,
        List<OrderItemEvent> items,
        double totalAmount,
        LocalDateTime placedAt
) {
    public record OrderItemEvent(
            UUID productId,
            String productName,
            int quantity,
            double unitPrice
    ) {}
}
