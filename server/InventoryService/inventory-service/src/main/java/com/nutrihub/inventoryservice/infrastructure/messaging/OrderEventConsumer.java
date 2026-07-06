package com.nutrihub.inventoryservice.infrastructure.messaging;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.nutrihub.inventoryservice.domain.repository.InventoryRepository;
import com.nutrihub.inventoryservice.infrastructure.messaging.config.RabbitMQConfig;
import com.nutrihub.inventoryservice.infrastructure.messaging.events.OrderCancelledEvent;
import com.nutrihub.inventoryservice.infrastructure.messaging.events.OrderPlacedEvent;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.amqp.core.Message;
import org.springframework.amqp.rabbit.annotation.RabbitListener;
import org.springframework.stereotype.Service;

@Slf4j
@Service
@RequiredArgsConstructor
public class OrderEventConsumer {

    private final InventoryRepository inventoryRepository;
    private final ObjectMapper objectMapper;

    @RabbitListener(queues = RabbitMQConfig.INVENTORY_QUEUE)
    public void handleMessage(Message message) throws Exception {
        Object typeIdHeader = message.getMessageProperties().getHeaders().get("__TypeId__");
        String typeId;
        if (typeIdHeader instanceof byte[]) {
            typeId = new String((byte[]) typeIdHeader);
        } else {
            typeId = typeIdHeader.toString();
        }

        String json = new String(message.getBody());

        if (typeId.contains("OrderPlacedEvent")) {
            OrderPlacedEvent event = objectMapper.readValue(json, OrderPlacedEvent.class);
            handleOrderPlaced(event);
        } else if (typeId.contains("OrderCancelledEvent")) {
            OrderCancelledEvent event = objectMapper.readValue(json, OrderCancelledEvent.class);
            handleOrderCancelled(event);
        }
    }

    private void handleOrderPlaced(OrderPlacedEvent event){
        log.info("Received OrderPlacedEvent for orderId: {}", event.orderId());

        event.items().forEach(item -> {
            inventoryRepository.findByProductId(item.productId())
                    .ifPresentOrElse(
                            inventoryItem -> {
                                inventoryItem.reduceStock(item.quantity());
                                inventoryRepository.save(inventoryItem);
                                log.info("Stock reduced for product: {} new quantity: {}",
                                        item.productName(), inventoryItem.getQuantity());
                            },
                            () -> log.warn("Product not found in inventory: {}", item.productId().toString())
                    );
        });
    }

    private void handleOrderCancelled(OrderCancelledEvent event) {
        log.info("Received OrderCancelledEvent for orderId: {}", event.orderId());
        event.items().forEach(item -> {
            inventoryRepository.findByProductId(item.productId())
                    .ifPresentOrElse(
                            inventoryItem -> {
                                inventoryItem.addStock(item.quantity());
                                inventoryRepository.save(inventoryItem);
                                log.info("Stock restored for product: {} new quantity: {}",
                                        item.productName(), inventoryItem.getQuantity());
                            },
                            () -> log.warn("Product not found in inventory: {}", item.productId())
                    );
        });
    }
}
