

namespace NotificationService.Application.Events
{
    public record OrderCancelledEvent(
        Guid OrderId,
        Guid CustomerId,
        List<OrderCancelledItemEvent> Items,
        DateTime CancelledAt
    );

    public record OrderCancelledItemEvent(
        Guid ProductId,
        string ProductName,
        int Quantity
    );
}
