package com.nutrihub.inventoryservice.application.handler.commandHandler;


import com.nutrihub.inventoryservice.application.command.CreateInventoryCommand;
import com.nutrihub.inventoryservice.domain.entity.InventoryItem;
import com.nutrihub.inventoryservice.domain.repository.InventoryRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

import java.util.UUID;

@Service
@RequiredArgsConstructor
public class CreateInventoryCommandHandler {

    private final InventoryRepository inventoryRepository;

    public UUID handle(CreateInventoryCommand command){
        InventoryItem item = InventoryItem.create(
                command.productId(),
                command.productName(),
                command.quantity());

        InventoryItem saved = inventoryRepository.save(item);
        return saved.getId();
    }
}
