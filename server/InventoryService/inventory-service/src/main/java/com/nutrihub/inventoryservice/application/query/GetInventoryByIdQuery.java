package com.nutrihub.inventoryservice.application.query;

import java.util.UUID;

public record GetInventoryByIdQuery(
        UUID id
) {
}
