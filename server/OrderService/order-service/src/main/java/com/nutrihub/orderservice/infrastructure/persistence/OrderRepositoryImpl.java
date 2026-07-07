package com.nutrihub.orderservice.infrastructure.persistence;

import com.nutrihub.orderservice.domain.entity.Order;
import com.nutrihub.orderservice.domain.repository.OrderRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Optional;
import java.util.UUID;

@Service
@RequiredArgsConstructor
public class OrderRepositoryImpl implements OrderRepository {

    private final JpaOrderRepository jpaOrderRepository;


    @Override
    public Optional<Order> findOrderById(UUID id) {
        return jpaOrderRepository.findById(id);
    }

    @Override
    public List<Order> findOrderByCustomerId(UUID customerId) {
        return jpaOrderRepository.findOrderByCustomerId(customerId);
    }

    @Override
    public List<Order> findAllOrders() {
        return jpaOrderRepository.findAll();
    }

    @Override
    public Order save(Order order) {
        return jpaOrderRepository.save(order);
    }
}
