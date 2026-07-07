package com.nutrihub.orderservice.application.dto;

import com.nutrihub.orderservice.domain.enums.OrderStatus;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.List;
import java.util.UUID;

public record OrderDto(
        UUID id,
        UUID customerId,
        List<OrderItemDto> items,
        BigDecimal totalAmount,
        OrderStatus status,
        LocalDateTime createdAt
) { }
