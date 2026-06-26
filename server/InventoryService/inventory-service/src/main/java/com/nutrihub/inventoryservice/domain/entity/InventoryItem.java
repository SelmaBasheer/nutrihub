package com.nutrihub.inventoryservice.domain.entity;

import jakarta.persistence.*;
import lombok.AccessLevel;
import lombok.Getter;
import lombok.NoArgsConstructor;

import java.time.LocalDateTime;
import java.util.UUID;

@Entity
@Table(name = "inventory_items")
@Getter
@NoArgsConstructor(access = AccessLevel.PROTECTED)
public class InventoryItem {

    @Id
    @GeneratedValue(strategy = GenerationType.UUID)
    private UUID id;

    @Column(name = "product_id", nullable = false)
    private UUID productId;

    @Column(name = "product_Name", nullable = false)
    private String productName;

    @Column(nullable = false)
    private int quantity;

    @Column(name = "last_Updated")
    private LocalDateTime lastUpdated;

    public static InventoryItem create(
            UUID productId,
            String productName,
            int quantity
    ){
        if(quantity < 0){
            throw new IllegalArgumentException("Quantity cannot be negative");
        }

        InventoryItem item =  new InventoryItem();
        item.productId = productId;
        item.productName = productName;
        item.quantity = quantity;
        item.lastUpdated = LocalDateTime.now();

        return item;
    }

    public void addStock(int quantity){
        if(quantity <= 0){
            throw new IllegalArgumentException("Quantity must be positive");
        }
        this.quantity += quantity;
        this.lastUpdated = LocalDateTime.now();
    }

    public void reduceStock(int quantity){
        if(quantity <= 0){
            throw new IllegalArgumentException("Quantity must be positive");
        }
        if(this.quantity < quantity){
            throw new IllegalStateException("Insufficient stock");
        }
        this.quantity -= quantity;
        this.lastUpdated = LocalDateTime.now();
    }
}
