package com.nutrihub.inventoryservice.application.dto;

import java.util.UUID;

public record CreateInventoryRequest(
        UUID productId,
        String productName,
        int quantity
) {
}
