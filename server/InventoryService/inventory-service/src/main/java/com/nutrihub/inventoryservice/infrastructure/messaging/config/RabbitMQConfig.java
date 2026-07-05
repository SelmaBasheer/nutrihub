package com.nutrihub.inventoryservice.infrastructure.messaging.config;

import com.nutrihub.inventoryservice.infrastructure.messaging.events.OrderCancelledEvent;
import com.nutrihub.inventoryservice.infrastructure.messaging.events.OrderPlacedEvent;
import org.springframework.amqp.core.Queue;
import org.springframework.amqp.rabbit.connection.ConnectionFactory;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.amqp.support.converter.DefaultClassMapper;
import org.springframework.amqp.support.converter.Jackson2JsonMessageConverter;
import org.springframework.amqp.support.converter.MessageConverter;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

import java.util.HashMap;
import java.util.Map;


@Configuration
public class RabbitMQConfig {

    public static final String INVENTORY_QUEUE = "inventory.orders";

    @Bean
    public Queue inventoryQueue() {
        return new Queue(INVENTORY_QUEUE, true);
    }

    @Bean
    public DefaultClassMapper classMapper() {
        DefaultClassMapper mapper = new DefaultClassMapper();
        Map<String, Class<?>> idClassMapping = new HashMap<>();
        idClassMapping.put("com.nutrihub.orderservice.infrastructure.messaging.events.OrderPlacedEvent", OrderPlacedEvent.class);
        idClassMapping.put("com.nutrihub.orderservice.infrastructure.messaging.events.OrderCancelledEvent", OrderCancelledEvent.class);
        mapper.setIdClassMapping(idClassMapping);
        return mapper;
    }

    @Bean
    public MessageConverter jsonMessageConverter() {
        Jackson2JsonMessageConverter converter = new Jackson2JsonMessageConverter();
        converter.setClassMapper(classMapper());
        return converter;
    }

    @Bean
    public RabbitTemplate rabbitTemplate(ConnectionFactory connectionFactory) {
        RabbitTemplate template = new RabbitTemplate(connectionFactory);
        template.setMessageConverter(jsonMessageConverter());
        return template;
    }
}
