package com.nutrihub.orderservice.application.command;

import java.util.UUID;

public record CancelOrderCommand(
        UUID orderId,
        UUID customerId
) { }
