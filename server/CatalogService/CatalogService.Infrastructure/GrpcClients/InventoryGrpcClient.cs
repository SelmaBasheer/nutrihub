using CatalogService.Application.Contracts;
using Grpc.Net.Client;
using NutriHub.CatalogService.Grpc;


namespace CatalogService.Infrastructure.GrpcClients
{
    public class InventoryGrpcClient : IInventoryGrpcClient
    {
        private readonly InventoryGrpcService.InventoryGrpcServiceClient _client;

        public InventoryGrpcClient(string grpcAddress)
        {
            var channel = GrpcChannel.ForAddress(grpcAddress);
            _client = new InventoryGrpcService.InventoryGrpcServiceClient(channel);
        }

        public async Task<bool> CreateInventoryAsync(
        Guid productId,
        string productName,
        int quantity = 0)
        {
            var request = new CreateInventoryRequest
            {
                ProductId = productId.ToString(),
                ProductName = productName,
                Quantity = quantity
            };

            var response = await _client.CreateInventoryFromProductAsync(request);
            return response.Success;
        }
    }
}
