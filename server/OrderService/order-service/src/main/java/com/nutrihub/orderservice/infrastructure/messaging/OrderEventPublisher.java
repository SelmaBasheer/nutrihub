package com.nutrihub.orderservice.infrastructure.messaging;

import com.nutrihub.orderservice.infrastructure.messaging.config.RabbitMQConfig;
import com.nutrihub.orderservice.infrastructure.messaging.events.OrderCancelledEvent;
import com.nutrihub.orderservice.infrastructure.messaging.events.OrderPlacedEvent;
import lombok.RequiredArgsConstructor;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.stereotype.Service;

@Service
@RequiredArgsConstructor
public class OrderEventPublisher {

    private final RabbitTemplate rabbitTemplate;

    public void publicOrderPlaced(OrderPlacedEvent event){
        rabbitTemplate.convertAndSend(
                RabbitMQConfig.EXCHANGE_NAME,
                "",
                event
        );
    }

    public void publishOrderCancelled(OrderCancelledEvent event) {
        rabbitTemplate.convertAndSend(
                RabbitMQConfig.EXCHANGE_NAME,
                "",
                event
        );
    }
}
