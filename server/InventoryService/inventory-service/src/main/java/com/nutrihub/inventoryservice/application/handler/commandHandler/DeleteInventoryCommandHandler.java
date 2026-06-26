package com.nutrihub.inventoryservice.application.handler.commandHandler;

import com.nutrihub.inventoryservice.application.command.DeleteInventoryCommand;
import com.nutrihub.inventoryservice.domain.repository.InventoryRepository;
import jakarta.persistence.EntityNotFoundException;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

@Service
@RequiredArgsConstructor
public class DeleteInventoryCommandHandler {

    private final InventoryRepository inventoryRepository;

    public void handle(DeleteInventoryCommand command){
        var inventory = inventoryRepository.findById(command.id())
                .orElseThrow(() -> new EntityNotFoundException(
                        "Inventory not found: " + command.id()
                ));
        inventoryRepository.delete(inventory.getId());
    }
}
