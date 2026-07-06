

namespace NotificationService.Application.Events
{
    public record OrderPlacedEvent(
        Guid OrderId,
        Guid CustomerId,
        List<OrderItemEvent> Items,
        double TotalAmount,
        DateTime PlacedAt
    );

    public record OrderItemEvent(
    Guid ProductId,
    string ProductName,
    int Quantity,
    double UnitPrice
);

}
