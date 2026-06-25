package com.nutrihub.orderservice.infrastructure.persistence;

import com.nutrihub.orderservice.domain.entity.Order;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;
import java.util.UUID;

public interface JpaOrderRepository extends JpaRepository<Order, UUID> {
    List<Order> findOrderByCustomerId(UUID customerId);
}
