package com.nutrihub.orderservice.application.handler.commandHandler;

import com.nutrihub.orderservice.application.command.CancelOrderCommand;
import com.nutrihub.orderservice.application.dto.OrderDto;
import com.nutrihub.orderservice.application.dto.OrderMapper;
import com.nutrihub.orderservice.domain.repository.OrderRepository;
import com.nutrihub.orderservice.infrastructure.messaging.OrderEventPublisher;
import com.nutrihub.orderservice.infrastructure.messaging.events.OrderCancelledEvent;import jakarta.persistence.EntityNotFoundException;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

@Service
@RequiredArgsConstructor
public class CancelOrderCommandHandler {

    private final OrderRepository orderRepository;
    private final OrderEventPublisher eventPublisher;

    public OrderDto handle(CancelOrderCommand command){
        var order = orderRepository.findOrderById(command.orderId())
                .orElseThrow(() -> new EntityNotFoundException(
                        "Order not found: " + command.orderId()
                ));

        order.cancel();
        orderRepository.save(order);

        OrderCancelledEvent event = new OrderCancelledEvent(
                order.getId(),
                order.getCustomerId(),
                order.getItems().stream()
                        .map(orderItem -> new OrderCancelledEvent.OrderItemEvent(
                                orderItem.getProductId(),
                                orderItem.getProductName(),
                                orderItem.getQuantity()
                        )).toList(),
                java.time.LocalDateTime.now()
        );
        eventPublisher.publishOrderCancelled(event);
        return OrderMapper.toDto(order);
    }
}
