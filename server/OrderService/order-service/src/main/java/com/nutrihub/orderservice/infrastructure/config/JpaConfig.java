package com.nutrihub.orderservice.infrastructure.config;

import org.springframework.context.annotation.Configuration;
import org.springframework.data.jpa.repository.config.EnableJpaRepositories;

@Configuration
@EnableJpaRepositories(
        basePackages = "com.nutrihub.orderservice.infrastructure.persistence"
)
public class JpaConfig {
}
