package com.nutrihub.orderservice.domain.repository;

import com.nutrihub.orderservice.domain.entity.Order;

import java.util.List;
import java.util.Optional;
import java.util.UUID;

public interface OrderRepository {
    Optional<Order> findOrderById(UUID id);
    List<Order> findOrderByCustomerId(UUID customerId);
    List<Order> findAllOrders();
    Order save(Order order);

}
