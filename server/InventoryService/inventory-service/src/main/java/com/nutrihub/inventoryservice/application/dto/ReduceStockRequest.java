package com.nutrihub.inventoryservice.application.dto;

import java.util.UUID;

public record ReduceStockRequest(
        UUID productId,
        int quantity
) {
}
