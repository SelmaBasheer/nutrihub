package com.nutrihub.inventoryservice.application.command;

import java.util.UUID;

public record ReduceStockCommand(
        UUID productId,
        int quantity
) {
}
