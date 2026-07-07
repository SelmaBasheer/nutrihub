package com.nutrihub.inventoryservice.application.handler.queryHandler;

import com.nutrihub.inventoryservice.application.dto.InventoryItemDto;
import com.nutrihub.inventoryservice.application.dto.InventoryMapper;
import com.nutrihub.inventoryservice.application.query.GetInventoryByIdQuery;
import com.nutrihub.inventoryservice.domain.repository.InventoryRepository;
import jakarta.persistence.EntityNotFoundException;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
@RequiredArgsConstructor
public class GetInventoryByIdQueryHandler {

    private final InventoryRepository inventoryRepository;

    public InventoryItemDto handle(GetInventoryByIdQuery query){

        var inventory = inventoryRepository.findById(query.id())
                .orElseThrow(() -> new EntityNotFoundException(
                        "Inventory not found: " + query.id()
                ));
        return InventoryMapper.toInventoryDto(inventory);
    }
}
