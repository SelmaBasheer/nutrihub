package com.nutrihub.orderservice.infrastructure.client;

import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;
import org.springframework.web.client.RestTemplate;
import java.util.HashMap;
import java.util.Map;
import java.util.UUID;

@Service
@RequiredArgsConstructor
public class InventoryClient {
    private final RestTemplate restTemplate;
    private static final String INVENTORY_BASE_URL = "http://localhost:5009/api/inventory";

    public void reduceStock(UUID productId, int quantity) {
        Map<String, Object> requestBody = new HashMap<>();
        requestBody.put("productId", productId);
        requestBody.put("quantity", quantity);
        restTemplate.put(INVENTORY_BASE_URL + "/reduceStock", requestBody);
    }

    public void addStock(UUID productId, int quantity) {
        Map<String, Object> requestBody = new HashMap<>();
        requestBody.put("productId", productId);
        requestBody.put("quantity", quantity);
        restTemplate.put(INVENTORY_BASE_URL + "/addStock", requestBody);
    }
}