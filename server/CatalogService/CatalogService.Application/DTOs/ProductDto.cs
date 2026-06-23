

namespace CatalogService.Application.DTOs
{
    public record ProductDto(
        Guid Id,
        string Name,
        string Description,
        decimal Price,
        int Stock,
        Guid CategoryId,
        List<string> ImageUrls,
        bool IsAvailable,
        DateTime CreatedAt,
        DateTime UpdatedAt
    );
}
