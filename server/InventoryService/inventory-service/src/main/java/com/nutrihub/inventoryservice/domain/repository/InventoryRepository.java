package com.nutrihub.inventoryservice.domain.repository;

import com.nutrihub.inventoryservice.domain.entity.InventoryItem;

import java.util.List;
import java.util.Optional;
import java.util.UUID;

public interface InventoryRepository {
    Optional<InventoryItem> findById(UUID id);
    Optional<InventoryItem> findByProductId(UUID productId);
    List<InventoryItem> getAll();
    InventoryItem save(InventoryItem item);
    void delete(UUID id);
}
