package com.nutrihub.inventoryservice.infrastructure.persistence;

import com.nutrihub.inventoryservice.domain.entity.InventoryItem;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.Optional;
import java.util.UUID;

public interface JpaInventoryRepository extends JpaRepository<InventoryItem, UUID>{
    Optional<InventoryItem> findByProductId(UUID productId);
}
