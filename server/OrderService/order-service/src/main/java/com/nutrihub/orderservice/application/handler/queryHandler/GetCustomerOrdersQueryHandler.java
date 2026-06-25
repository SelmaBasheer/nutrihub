package com.nutrihub.orderservice.application.handler.queryHandler;

import com.nutrihub.orderservice.application.dto.OrderDto;
import com.nutrihub.orderservice.application.dto.OrderMapper;
import com.nutrihub.orderservice.application.query.GetCustomerOrdersQuery;
import com.nutrihub.orderservice.domain.repository.OrderRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
@RequiredArgsConstructor
public class GetCustomerOrdersQueryHandler {

    private final OrderRepository orderRepository;
    public List<OrderDto> handle(GetCustomerOrdersQuery query){
        return orderRepository.findOrderByCustomerId(query.customerId())
                .stream().map(OrderMapper :: toDto)
                .toList();
    }
}
