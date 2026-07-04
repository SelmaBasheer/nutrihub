package com.nutrihub.inventoryservice.api.grpc;

import com.nutrihub.inventoryservice.application.command.CreateInventoryCommand;
import com.nutrihub.inventoryservice.application.handler.commandHandler.CreateInventoryCommandHandler;
import com.nutrihub.inventoryservice.grpc.InventoryGrpcServiceGrpc;
import com.nutrihub.inventoryservice.grpc.InventoryProto;
import io.grpc.stub.StreamObserver;
import lombok.RequiredArgsConstructor;
import net.devh.boot.grpc.server.service.GrpcService;

import java.util.UUID;

@GrpcService
@RequiredArgsConstructor
public class InventoryGrpcService
        extends InventoryGrpcServiceGrpc.InventoryGrpcServiceImplBase {

    private final CreateInventoryCommandHandler createInventoryHandler;

    public void createInventoryFromProduct(
            InventoryProto.CreateInventoryRequest request,
            StreamObserver<InventoryProto.CreateInventoryResponse> responseObserver ){
        try{
            CreateInventoryCommand command = new CreateInventoryCommand(
                    UUID.fromString(request.getProductId()),
                    request.getProductName(),
                    request.getQuantity()
            );
            UUID inventoryId = createInventoryHandler.handle(command);

            InventoryProto.CreateInventoryResponse response = InventoryProto.CreateInventoryResponse.newBuilder()
                    .setInventoryId(inventoryId.toString())
                    .setSuccess(true)
                    .setMessage("Inventory created successfully")
                    .build();

            responseObserver.onNext(response);
            responseObserver.onCompleted();
        } catch (Exception e) {
            InventoryProto.CreateInventoryResponse response = InventoryProto.CreateInventoryResponse.newBuilder()
                    .setSuccess(false)
                    .setMessage(e.getMessage())
                    .build();

            responseObserver.onNext(response);
            responseObserver.onCompleted();
        }
    }
}
