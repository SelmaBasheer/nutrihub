

namespace CatalogService.Application.Contracts
{
    public interface IInventoryGrpcClient
    {
        Task<bool> CreateInventoryAsync(Guid productId, string productName, int quantity = 0);
    }
}
