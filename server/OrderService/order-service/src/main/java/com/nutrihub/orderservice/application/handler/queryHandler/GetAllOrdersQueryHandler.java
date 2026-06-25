package com.nutrihub.orderservice.application.handler.queryHandler;

import com.nutrihub.orderservice.application.dto.OrderDto;
import com.nutrihub.orderservice.application.dto.OrderMapper;
import com.nutrihub.orderservice.application.query.GetAllOrdersQuery;
import com.nutrihub.orderservice.domain.repository.OrderRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
@RequiredArgsConstructor
public class GetAllOrdersQueryHandler {

    private final OrderRepository orderRepository;

    public List<OrderDto> handle(GetAllOrdersQuery query) {
        return orderRepository.findAllOrders()
                .stream()
                .map(OrderMapper :: toDto)
                .toList();
    }
}
