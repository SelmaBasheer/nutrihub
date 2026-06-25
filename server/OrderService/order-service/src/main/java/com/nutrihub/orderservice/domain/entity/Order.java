package com.nutrihub.orderservice.domain.entity;

import com.nutrihub.orderservice.domain.enums.OrderStatus;
import jakarta.persistence.*;
import lombok.AccessLevel;
import lombok.Getter;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
import java.util.UUID;

@Entity
@Table(name = "orders")
@Getter
@NoArgsConstructor(access = AccessLevel.PROTECTED)
public class Order {

    @Id
    @GeneratedValue(strategy = GenerationType.UUID)
    private UUID id;

    @Column(name = "customer_id", nullable = false)
    private UUID customerId;

    @OneToMany(mappedBy = "order", cascade = CascadeType.ALL,
                orphanRemoval = true, fetch = FetchType.LAZY)
    @Getter(AccessLevel.NONE)
    private List<OrderItem> items = new ArrayList<>();

    @Column(name = "total_amount", nullable = false, precision = 10, scale = 2)
    private BigDecimal totalAmount;

    @Enumerated(EnumType.STRING)
    @Column(nullable = false)
    private OrderStatus status;

    @Column(name = "created_at", nullable = false, updatable = false)
    private LocalDateTime createdAt;

    public List<OrderItem> getItems(){
        return Collections.unmodifiableList(items);
    }

    //Factory Method
    public static Order create(UUID customerId){
        if(customerId == null){
            throw new IllegalArgumentException("CustomerId cannot be null.");
        }

        Order order = new Order();
        order.customerId = customerId;
        order.status = OrderStatus.PENDING;
        order.totalAmount = BigDecimal.ZERO;
        order.createdAt = LocalDateTime.now();

        return order;
    }

    //Items
    public void addItem(UUID productId, String productName, int quantity, BigDecimal unitPrice){
        if(this.status != OrderStatus.PENDING){
            throw new IllegalStateException("Items can only be added to a PENDING order.");
        }
        OrderItem item = OrderItem.create(this, productId, productName, quantity, unitPrice);
        this.items.add(item);
        recalculateTotal();
    }

    private void recalculateTotal(){
        this.totalAmount = items.stream()
                .map(OrderItem::getSubTotal)
                .reduce(BigDecimal.ZERO, BigDecimal::add);
    }

    //Status Transitions
    public void confirm(){
        if(this.status != OrderStatus.PENDING){
            throw new IllegalStateException("Cannot confirm an order that is " + this.status);
        }
        if(this.items.isEmpty()){
            throw new IllegalStateException("Cannot confirm an order with no items.");
        }
        this.status = OrderStatus.CONFIRMED;
    }

    public void ship(){
        if(this.status != OrderStatus.CONFIRMED){
            throw new IllegalStateException("Cannot ship an order that is " + this.status);
        }
        this.status = OrderStatus.SHIPPED;
    }

    public void deliver(){
        if (this.status != OrderStatus.SHIPPED){
            throw new IllegalStateException("Cannot deliver an order that is " + this.status);
        }
        this.status = OrderStatus.DELIVERED;
    }

    public void cancel(){
        if (this.status != OrderStatus.PENDING &&
                this.status != OrderStatus.CONFIRMED ){
            throw new IllegalStateException("Order can only be cancelled before it is shipped.");
        }
        this.status = OrderStatus.CANCELLED;
    }

    public void requestReturn(){
        if (this.status != OrderStatus.DELIVERED){
            throw new IllegalStateException("Return can only be requested for DELIVERED orders.");
        }
        this.status = OrderStatus.RETURN_REQUESTED;
    }

    public void approveReturn() {
        if (this.status != OrderStatus.RETURN_REQUESTED){
            throw new IllegalStateException("Can only approve a RETURN_REQUESTED order.");
        }
        this.status = OrderStatus.RETURN_APPROVED;
    }

    public void rejectReturn() {
        if (this.status != OrderStatus.RETURN_REQUESTED){
            throw new IllegalStateException("Can only reject a RETURN_REQUESTED order.");
        }
        this.status = OrderStatus.RETURN_REJECTED;
    }

    public void initiateRefund() {
        if (this.status != OrderStatus.RETURN_APPROVED){
            throw new IllegalStateException("Refund can only be initiated after return is approved.");
        }
        this.status = OrderStatus.REFUND_INITIATED;
    }

    public void completeRefund() {
        if (this.status != OrderStatus.REFUND_INITIATED){
            throw new IllegalStateException("Refund must be initiated before completing.");
        }
        this.status = OrderStatus.REFUNDED;
    }
}
