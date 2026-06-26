package com.nutrihub.inventoryservice.application.dto;

import com.nutrihub.inventoryservice.domain.entity.InventoryItem;


public class InventoryMapper {
    //DRY Principle - Don't Repeat Yourself
    public static InventoryItemDto toInventoryDto(InventoryItem item){
        return new InventoryItemDto(
                item.getId(),
                item.getProductId(),
                item.getProductName(),
                item.getQuantity(),
                item.getLastUpdated()
        );
    }

    private InventoryMapper() { }
}
