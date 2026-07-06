using NotificationService.Application.Events;

namespace NotificationService.Application.Contracts
{
    public interface INotificationHandler
    {
        Task HandleOrderPlacedAsync(OrderPlacedEvent orderPlacedEvent);
        Task HandleOrderCancelledAsync(OrderCancelledEvent orderCancelledEvent);
    }
}
