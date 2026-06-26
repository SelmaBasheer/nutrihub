	package com.nutrihub.inventoryservice;

	import org.springframework.boot.SpringApplication;
	import org.springframework.boot.autoconfigure.SpringBootApplication;

	import java.awt.*;
	import java.net.URI;

	@SpringBootApplication
	public class InventoryServiceApplication {

		public static void main(String[] args) {

			SpringApplication.run(InventoryServiceApplication.class, args);

			try {
				Desktop.getDesktop().browse(new URI("http://localhost:5009/swagger-ui.html"));
			} catch (Exception e) {
				System.out.println("Open Swagger at: http://localhost:5009/swagger-ui.html");
			}
		}

	}
