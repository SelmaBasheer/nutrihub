package com.nutrihub.inventoryservice.application.handler.commandHandler;

import com.nutrihub.inventoryservice.application.command.ReduceStockCommand;
import com.nutrihub.inventoryservice.domain.repository.InventoryRepository;
import jakarta.persistence.EntityNotFoundException;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

@Service
@RequiredArgsConstructor
public class ReduceStockCommandHandler {

    private final InventoryRepository inventoryRepository;

    public void handle(ReduceStockCommand command){
        var item = inventoryRepository.findByProductId(command.productId())
                .orElseThrow(() -> new EntityNotFoundException(
                        "Product not found: "+ command.productId()
                ));
        item.reduceStock(command.quantity());
        inventoryRepository.save(item);
    }
}
