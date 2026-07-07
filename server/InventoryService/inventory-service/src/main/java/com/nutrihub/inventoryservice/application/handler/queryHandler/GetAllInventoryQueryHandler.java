package com.nutrihub.inventoryservice.application.handler.queryHandler;

import com.nutrihub.inventoryservice.application.dto.InventoryItemDto;
import com.nutrihub.inventoryservice.application.dto.InventoryMapper;
import com.nutrihub.inventoryservice.application.query.GetAllInventoryQuery;
import com.nutrihub.inventoryservice.domain.repository.InventoryRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
@RequiredArgsConstructor
public class GetAllInventoryQueryHandler {

    private final InventoryRepository inventoryRepository;

    public List<InventoryItemDto> handle(GetAllInventoryQuery query){

        return inventoryRepository.getAll()
                .stream()
                .map(InventoryMapper :: toInventoryDto)
                .toList();
    }
}
