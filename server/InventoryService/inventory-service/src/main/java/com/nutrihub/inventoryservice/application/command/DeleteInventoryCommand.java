package com.nutrihub.inventoryservice.application.command;

import java.util.UUID;

public record DeleteInventoryCommand(
        UUID id
) {
}
