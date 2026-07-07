package com.nutrihub.orderservice.application.handler.queryHandler;

import com.nutrihub.orderservice.application.dto.OrderDto;
import com.nutrihub.orderservice.application.dto.OrderMapper;
import com.nutrihub.orderservice.application.query.GetOrderByIdQuery;
import com.nutrihub.orderservice.domain.repository.OrderRepository;
import jakarta.persistence.EntityNotFoundException;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

@Service
@RequiredArgsConstructor
public class GetOrderByIdQueryHandler {
    private final OrderRepository orderRepository;

    public OrderDto handle(GetOrderByIdQuery query){
        var order = orderRepository.findOrderById(query.orderId())
                .orElseThrow(() -> new EntityNotFoundException(
                        "Order not found: " + query.orderId()
                ));
        return OrderMapper.toDto(order);
    }
}
