package com.nutrihub.orderservice.application.dto;

import com.nutrihub.orderservice.domain.entity.Order;
import com.nutrihub.orderservice.domain.entity.OrderItem;

import java.util.List;

public class OrderMapper {
    public static OrderDto toDto(Order order){
        List<OrderItemDto> itemDtos = order.getItems().stream()
                .map(OrderMapper::toItemDto).toList();

        return new OrderDto(
                order.getId(),
                order.getCustomerId(),
                itemDtos,
                order.getTotalAmount(),
                order.getStatus(),
                order.getCreatedAt()
        );
    }

    private static OrderItemDto toItemDto(OrderItem item){
        return new OrderItemDto(
                item.getId(),
                item.getProductId(),
                item.getProductName(),
                item.getQuantity(),
                item.getUnitPrice(),
                item.getSubTotal()
        );
    }

    private OrderMapper() { }
}
