package com.nutrihub.inventoryservice.application.dto;

import java.time.LocalDateTime;
import java.util.UUID;

public record InventoryItemDto(
        UUID id,
        UUID productId,
        String productName,
        int quantity,
        LocalDateTime lastUpdated
) {
}
