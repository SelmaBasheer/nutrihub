package com.nutrihub.orderservice.application.handler.commandHandler;

import com.nutrihub.orderservice.application.command.PlaceOrderCommand;
import com.nutrihub.orderservice.domain.entity.Order;
import com.nutrihub.orderservice.domain.repository.OrderRepository;
import com.nutrihub.orderservice.infrastructure.messaging.OrderEventPublisher;
import com.nutrihub.orderservice.infrastructure.messaging.events.OrderPlacedEvent;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

import java.util.UUID;

@Service
@RequiredArgsConstructor
public class PlaceOrderCommandHandler {

    private final OrderRepository orderRepository;
    private final OrderEventPublisher eventPublisher;


    public UUID handle(PlaceOrderCommand command){
        Order order = Order.create(command.customerId());

        for(var item : command.items()){
            order.addItem(
                    item.productId(),
                    item.productName(),
                    item.quantity(),
                    item.unitPrice()
            );
        }

        Order saved = orderRepository.save(order);

        // Publish OrderPlaced event to RabbitMQ
        OrderPlacedEvent event = new OrderPlacedEvent(
                saved.getId(),
                saved.getCustomerId(),
                saved.getItems().stream()
                        .map(item -> new OrderPlacedEvent.OrderItemEvent(
                                item.getProductId(),
                                item.getProductName(),
                                item.getQuantity(),
                                item.getUnitPrice().doubleValue()
                        )).toList(),
                saved.getTotalAmount().doubleValue(),
                saved.getCreatedAt()
        );

        eventPublisher.publicOrderPlaced(event);
        return saved.getId();
    }
}
