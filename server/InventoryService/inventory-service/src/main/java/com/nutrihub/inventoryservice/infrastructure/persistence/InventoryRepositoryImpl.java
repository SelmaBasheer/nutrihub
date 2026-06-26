package com.nutrihub.inventoryservice.infrastructure.persistence;

import com.nutrihub.inventoryservice.domain.entity.InventoryItem;
import com.nutrihub.inventoryservice.domain.repository.InventoryRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Optional;
import java.util.UUID;

@Service
@RequiredArgsConstructor
public class InventoryRepositoryImpl implements InventoryRepository {

    private final JpaInventoryRepository jpaInventoryRepository;

    @Override
    public Optional<InventoryItem> findById(UUID id) {
        return jpaInventoryRepository.findById(id);
    }

    @Override
    public Optional<InventoryItem> findByProductId(UUID productId) {
        return jpaInventoryRepository.findByProductId(productId);
    }

    @Override
    public List<InventoryItem> getAll() {
        return jpaInventoryRepository.findAll();
    }

    @Override
    public InventoryItem save(InventoryItem item) {
        return jpaInventoryRepository.save(item);
    }

    @Override
    public void delete(UUID id) {
        jpaInventoryRepository.deleteById(id);
    }
}
