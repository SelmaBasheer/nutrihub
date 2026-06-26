package com.nutrihub.inventoryservice.infrastructure.config;

import org.springframework.context.annotation.Configuration;
import org.springframework.data.jpa.repository.config.EnableJpaRepositories;

@Configuration
@EnableJpaRepositories(
        basePackages = "com.nutrihub.inventoryservice.infrastructure.persistence"
)
public class JpaConfig {
}
