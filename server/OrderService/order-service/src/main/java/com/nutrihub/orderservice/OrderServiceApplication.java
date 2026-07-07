package com.nutrihub.orderservice;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

import java.awt.*;
import java.net.URI;

@SpringBootApplication
public class OrderServiceApplication {

	public static void main(String[] args) {
		SpringApplication.run(OrderServiceApplication.class, args);

		try {
			Desktop.getDesktop().browse(new URI("http://localhost:5003/swagger-ui.html"));
		} catch (Exception e) {
			System.out.println("Open Swagger at: http://localhost:5003/swagger-ui.html");
		}
	}

}
