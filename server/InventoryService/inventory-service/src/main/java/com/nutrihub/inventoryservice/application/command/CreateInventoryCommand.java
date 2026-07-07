package com.nutrihub.inventoryservice.application.command;

import java.util.UUID;

public record CreateInventoryCommand(
        UUID productId,
        String productName,
        int quantity
) {
}
