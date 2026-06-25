package com.nutrihub.orderservice.application.handler.commandHandler;

import com.nutrihub.orderservice.application.command.CancelOrderCommand;
import com.nutrihub.orderservice.domain.repository.OrderRepository;
import jakarta.persistence.EntityNotFoundException;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

@Service
@RequiredArgsConstructor
public class CancelOrderCommandHandler {

    private final OrderRepository orderRepository;

    public void handle(CancelOrderCommand command){
        var order = orderRepository.findOrderById(command.orderId())
                .orElseThrow(() -> new EntityNotFoundException(
                        "Order not found: " + command.orderId()
                ));

        order.cancel();
        orderRepository.save(order);
    }
}
