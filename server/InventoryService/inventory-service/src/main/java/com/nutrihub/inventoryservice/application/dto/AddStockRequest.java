package com.nutrihub.inventoryservice.application.dto;

import java.util.UUID;

public record AddStockRequest(
        UUID productId,
        int quantity
) {
}
