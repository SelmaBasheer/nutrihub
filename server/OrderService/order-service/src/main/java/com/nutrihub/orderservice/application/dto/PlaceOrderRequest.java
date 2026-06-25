package com.nutrihub.orderservice.application.dto;

import jakarta.validation.constraints.Min;
import jakarta.validation.constraints.NotEmpty;
import jakarta.validation.constraints.NotNull;

import java.math.BigDecimal;
import java.util.List;
import java.util.UUID;

public record PlaceOrderRequest(
        @NotNull(message = "Customer ID is required")
        UUID customerId,

        @NotEmpty(message = "Order must have at least one item")
        List<OrderItemRequest> items
) {
    public record OrderItemRequest(
            @NotNull(message = "Product ID is required")
            UUID productId,

            @NotNull(message = "Product name is required")
            String productName,

            @Min(value = 1, message = "Quantity must be at least 1")
            int quantity,

            @NotNull(message = "Unit price is required")
            @Min(value = 0, message = "Unit price cannot be negative")
            BigDecimal unitPrice
    ) { }
}
