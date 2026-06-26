package com.nutrihub.inventoryservice.application.handler.queryHandler;

import com.nutrihub.inventoryservice.application.dto.InventoryItemDto;
import com.nutrihub.inventoryservice.application.dto.InventoryMapper;
import com.nutrihub.inventoryservice.application.query.GetInventoryByProductIdQuery;
import com.nutrihub.inventoryservice.domain.repository.InventoryRepository;
import jakarta.persistence.EntityNotFoundException;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

@Service
@RequiredArgsConstructor
public class GetInventoryByProductIdQueryHandler {

    private final InventoryRepository inventoryRepository;

    public InventoryItemDto handle(GetInventoryByProductIdQuery query){

        var item = inventoryRepository.findByProductId(query.productId())
                .orElseThrow(() -> new EntityNotFoundException(
                        "Product not found: " + query.productId()));

        return InventoryMapper.toInventoryDto(item);
    }
}
