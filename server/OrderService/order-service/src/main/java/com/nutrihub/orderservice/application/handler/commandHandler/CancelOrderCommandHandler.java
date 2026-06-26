package com.nutrihub.orderservice.application.handler.commandHandler;

import com.nutrihub.orderservice.application.command.CancelOrderCommand;
import com.nutrihub.orderservice.application.dto.OrderDto;
import com.nutrihub.orderservice.application.dto.OrderMapper;
import com.nutrihub.orderservice.domain.repository.OrderRepository;
import com.nutrihub.orderservice.infrastructure.client.InventoryClient;
import jakarta.persistence.EntityNotFoundException;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

@Service
@RequiredArgsConstructor
public class CancelOrderCommandHandler {

    private final OrderRepository orderRepository;
    private final InventoryClient inventoryClient;

    public OrderDto handle(CancelOrderCommand command){
        var order = orderRepository.findOrderById(command.orderId())
                .orElseThrow(() -> new EntityNotFoundException(
                        "Order not found: " + command.orderId()
                ));

        order.cancel();
        orderRepository.save(order);
        for (var item : order.getItems()) {
            inventoryClient.addStock(item.getProductId(), item.getQuantity());
        }
        return OrderMapper.toDto(order);
    }
}
