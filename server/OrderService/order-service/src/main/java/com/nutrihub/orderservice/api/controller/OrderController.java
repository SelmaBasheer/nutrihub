package com.nutrihub.orderservice.api.controller;

import com.nutrihub.orderservice.application.command.CancelOrderCommand;
import com.nutrihub.orderservice.application.command.PlaceOrderCommand;
import com.nutrihub.orderservice.application.command.UpdateOrderStatusCommand;
import com.nutrihub.orderservice.application.dto.OrderDto;
import com.nutrihub.orderservice.application.dto.PlaceOrderRequest;
import com.nutrihub.orderservice.application.handler.commandHandler.CancelOrderCommandHandler;
import com.nutrihub.orderservice.application.handler.commandHandler.PlaceOrderCommandHandler;
import com.nutrihub.orderservice.application.handler.commandHandler.UpdateOrderStatusCommandHandler;
import com.nutrihub.orderservice.application.handler.queryHandler.GetAllOrdersQueryHandler;
import com.nutrihub.orderservice.application.handler.queryHandler.GetCustomerOrdersQueryHandler;
import com.nutrihub.orderservice.application.handler.queryHandler.GetOrderByIdQueryHandler;
import com.nutrihub.orderservice.application.query.GetAllOrdersQuery;
import com.nutrihub.orderservice.application.query.GetCustomerOrdersQuery;
import com.nutrihub.orderservice.application.query.GetOrderByIdQuery;
import com.nutrihub.orderservice.domain.enums.OrderStatus;
import jakarta.validation.Valid;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.UUID;

@RestController
@RequestMapping("/api/orders")
@RequiredArgsConstructor
public class OrderController {

    private final PlaceOrderCommandHandler placeOrderCommandHandler;
    private final CancelOrderCommandHandler cancelOrderCommandHandler;
    private final UpdateOrderStatusCommandHandler updateOrderStatusCommandHandler;
    private final GetOrderByIdQueryHandler getOrderByIdQueryHandler;
    private final GetCustomerOrdersQueryHandler getCustomerOrdersQueryHandler;
    private final GetAllOrdersQueryHandler getAllOrdersQueryHandler;

    // POST /api/orders — place order
    @PostMapping
    public ResponseEntity<UUID> placeOrder(@Valid @RequestBody PlaceOrderRequest request){
        var command = new PlaceOrderCommand(
                request.customerId(),
                request.items()
        );
        UUID orderId = placeOrderCommandHandler.handle(command);
        return ResponseEntity.status(HttpStatus.CREATED).body(orderId);
    }

    // GET /api/orders/customer/{customerId} — get customer orders
    @GetMapping("/customer/{cutomerId}")
    public ResponseEntity<List<OrderDto>> getCustomerOrders(@PathVariable UUID customerId){
        var query = new GetCustomerOrdersQuery(customerId);
        return ResponseEntity.ok(getCustomerOrdersQueryHandler.handle(query));
    }

    // GET /api/orders/{id} — get order by id
    @GetMapping("/{id}")
    public ResponseEntity<OrderDto> getOrderById(@PathVariable UUID id){
        var query = new GetOrderByIdQuery(id);
        return ResponseEntity.ok(getOrderByIdQueryHandler.handle(query));
    }

    // GET /api/orders/all — get all orders (Admin only)
    @GetMapping("/all")
    public ResponseEntity<List<OrderDto>> getAllOrders(){
        var query = new GetAllOrdersQuery();
        return ResponseEntity.ok(getAllOrdersQueryHandler.handle(query));
    }

    // PUT /api/orders/{id}/cancel — cancel order
    @PutMapping("/{id}/cancel")
    public ResponseEntity<String> cancelOrder(@PathVariable UUID id,
                                              @RequestParam UUID customerId){
        var command = new CancelOrderCommand(id, customerId);
        cancelOrderCommandHandler.handle(command);
        return ResponseEntity.ok("Order cancelled successfully");
    }

    // PUT /api/orders/{id}/status — update status (Admin only)
    @PutMapping("/{id}/status")
    public ResponseEntity<String> updateStatus(@PathVariable UUID id,
                                               @RequestParam OrderStatus newStatus) {
        var command = new UpdateOrderStatusCommand(id, newStatus);
        updateOrderStatusCommandHandler.handle(command);
        return ResponseEntity.ok("Order status updated to " + newStatus);
    }
}
