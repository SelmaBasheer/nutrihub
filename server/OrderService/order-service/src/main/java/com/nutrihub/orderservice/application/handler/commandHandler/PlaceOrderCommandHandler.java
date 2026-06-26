package com.nutrihub.orderservice.application.handler.commandHandler;

import com.nutrihub.orderservice.application.command.PlaceOrderCommand;
import com.nutrihub.orderservice.domain.entity.Order;
import com.nutrihub.orderservice.domain.repository.OrderRepository;
import com.nutrihub.orderservice.infrastructure.client.InventoryClient;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

import java.util.UUID;

@Service
@RequiredArgsConstructor
public class PlaceOrderCommandHandler {

    private final OrderRepository orderRepository;
    private final InventoryClient inventoryClient;

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
        for (var item : command.items()) {
            inventoryClient.reduceStock(item.productId(), item.quantity());
        }
        return saved.getId();
    }
}
