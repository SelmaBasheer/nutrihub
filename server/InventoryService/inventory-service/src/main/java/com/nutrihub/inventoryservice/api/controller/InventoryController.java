package com.nutrihub.inventoryservice.api.controller;

import com.nutrihub.inventoryservice.application.command.AddStockCommand;
import com.nutrihub.inventoryservice.application.command.CreateInventoryCommand;
import com.nutrihub.inventoryservice.application.command.DeleteInventoryCommand;
import com.nutrihub.inventoryservice.application.command.ReduceStockCommand;
import com.nutrihub.inventoryservice.application.dto.AddStockRequest;
import com.nutrihub.inventoryservice.application.dto.CreateInventoryRequest;
import com.nutrihub.inventoryservice.application.dto.InventoryItemDto;
import com.nutrihub.inventoryservice.application.dto.ReduceStockRequest;
import com.nutrihub.inventoryservice.application.handler.commandHandler.AddStockCommandHandler;
import com.nutrihub.inventoryservice.application.handler.commandHandler.CreateInventoryCommandHandler;
import com.nutrihub.inventoryservice.application.handler.commandHandler.DeleteInventoryCommandHandler;
import com.nutrihub.inventoryservice.application.handler.commandHandler.ReduceStockCommandHandler;
import com.nutrihub.inventoryservice.application.handler.queryHandler.GetAllInventoryQueryHandler;
import com.nutrihub.inventoryservice.application.handler.queryHandler.GetInventoryByIdQueryHandler;
import com.nutrihub.inventoryservice.application.handler.queryHandler.GetInventoryByProductIdQueryHandler;
import com.nutrihub.inventoryservice.application.query.GetAllInventoryQuery;
import com.nutrihub.inventoryservice.application.query.GetInventoryByIdQuery;
import com.nutrihub.inventoryservice.application.query.GetInventoryByProductIdQuery;
import jakarta.validation.Valid;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.UUID;

@RestController
@RequestMapping("/api/inventory")
@RequiredArgsConstructor
public class InventoryController {

    private final CreateInventoryCommandHandler createInventoryHandler;
    private final AddStockCommandHandler addStockHandler;
    private final ReduceStockCommandHandler reduceStockHandler;
    private final DeleteInventoryCommandHandler deleteInventoryHandler;
    private final GetAllInventoryQueryHandler getAllInventoryHandler;
    private final GetInventoryByIdQueryHandler getInventoryByIdHandler;
    private final GetInventoryByProductIdQueryHandler getInventoryByProductIdHandler;

    //POST /api/inventory
    @PostMapping
    public ResponseEntity<UUID> createInventory(@Valid @RequestBody CreateInventoryRequest request){
        var command = new CreateInventoryCommand(
                request.productId(),
                request.productName(),
                request.quantity()
        );
        UUID inventoryId = createInventoryHandler.handle(command);
        return ResponseEntity.status(HttpStatus.CREATED).body(inventoryId);
    }

    //PUT /api/inventory/addStock
    @PutMapping("/addStock")
    public ResponseEntity<String> addStock(@Valid @RequestBody AddStockRequest request){
        var command = new AddStockCommand(
            request.productId(),
            request.quantity()
        );
        addStockHandler.handle(command);
        return ResponseEntity.ok("Stock increased by " + request.quantity());
    }

    //PUT /api/inventory/reduceStock
    @PutMapping("/reduceStock")
    public ResponseEntity<String> reduceStock(@Valid @RequestBody ReduceStockRequest request){
        var command = new ReduceStockCommand(
                request.productId(),
                request.quantity()
        );
        reduceStockHandler.handle(command);
        return ResponseEntity.ok("Stock deducted by " + request.quantity());
    }

    //Delete /api/inventory/{id}
    @DeleteMapping("/{id}")
    public ResponseEntity<String> deleteInventory(@PathVariable UUID id){
        var command = new DeleteInventoryCommand(id);
        deleteInventoryHandler.handle(command);
        return ResponseEntity.ok("Inventory deleted.");
    }

    // GET /api/inventory/all — get all inventory
    @GetMapping("/all")
    public ResponseEntity<List<InventoryItemDto>> getAll(){
        var query = new GetAllInventoryQuery();
        return ResponseEntity.ok(getAllInventoryHandler.handle(query));
    }

    // GET /api/inventory/{id} — get inventory by id
    @GetMapping("/{id}")
    public ResponseEntity<InventoryItemDto> getById(@PathVariable UUID id){
        var query = new GetInventoryByIdQuery(id);
        return ResponseEntity.ok(getInventoryByIdHandler.handle(query));
    }

    // GET /api/inventory/product/{productId} — get inventory by productId
    @GetMapping("/product/{productId}")
    public ResponseEntity<InventoryItemDto> getByProductId(@PathVariable UUID productId){
        var query = new GetInventoryByProductIdQuery(productId);
        return ResponseEntity.ok(getInventoryByProductIdHandler.handle(query));
    }
}
