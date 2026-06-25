package com.nutrihub.orderservice.application.handler.commandHandler;

import com.nutrihub.orderservice.application.command.UpdateOrderStatusCommand;
import com.nutrihub.orderservice.domain.repository.OrderRepository;
import jakarta.persistence.EntityNotFoundException;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

@Service
@RequiredArgsConstructor
public class UpdateOrderStatusCommandHandler {

    private final OrderRepository orderRepository;

    public void handle(UpdateOrderStatusCommand command){
        var order = orderRepository.findOrderById(command.orderId())
                .orElseThrow(() -> new EntityNotFoundException(
                        "Order not found: " + command.orderId()
                ));

        switch (command.newStatus()){
            case CONFIRMED -> order.confirm();
            case SHIPPED          -> order.ship();
            case DELIVERED        -> order.deliver();
            case CANCELLED        -> order.cancel();
            case RETURN_REQUESTED -> order.requestReturn();
            case RETURN_APPROVED  -> order.approveReturn();
            case RETURN_REJECTED  -> order.rejectReturn();
            case REFUND_INITIATED -> order.initiateRefund();
            case REFUNDED         -> order.completeRefund();
            default -> throw new IllegalArgumentException(
                    "Unhandled status: " + command.newStatus());
        }

        orderRepository.save(order);
    }
}
