package com.nutrihub.inventoryservice.application.handler.commandHandler;

import com.nutrihub.inventoryservice.application.command.AddStockCommand;
import com.nutrihub.inventoryservice.domain.repository.InventoryRepository;
import jakarta.persistence.EntityNotFoundException;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

@Service
@RequiredArgsConstructor
public class AddStockCommandHandler {

    private final InventoryRepository inventoryRepository;

    public void handle(AddStockCommand command){
        var item = inventoryRepository.findByProductId(command.productId())
                .orElseThrow(() -> new EntityNotFoundException(
                        "Product not found: " + command.productId()));
        item.addStock(command.quantity());
        inventoryRepository.save(item);
    }
}
