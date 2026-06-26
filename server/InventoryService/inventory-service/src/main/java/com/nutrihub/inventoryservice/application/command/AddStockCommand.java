package com.nutrihub.inventoryservice.application.command;

import java.util.UUID;

public record AddStockCommand(
        UUID productId,
        int quantity
) {
}
