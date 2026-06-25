package com.nutrihub.orderservice.application.query;

import java.util.UUID;

public record GetOrderByIdQuery(
        UUID orderId
) { }
