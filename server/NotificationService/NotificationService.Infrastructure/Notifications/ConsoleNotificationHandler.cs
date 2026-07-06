

using NotificationService.Application.Contracts;
using NotificationService.Application.Events;

namespace NotificationService.Infrastructure.Notifications
{
    public class ConsoleNotificationHandler : INotificationHandler
    {
        public Task HandleOrderPlacedAsync(OrderPlacedEvent orderPlacedEvent)
        {
            Console.WriteLine("=== ORDER PLACED NOTIFICATION ===");
            Console.WriteLine($"Order ID: {orderPlacedEvent.OrderId}");
            Console.WriteLine($"Customer ID: {orderPlacedEvent.CustomerId}");
            Console.WriteLine($"Total Amount: {orderPlacedEvent.TotalAmount}");
            Console.WriteLine("Items:");
            foreach (var item in orderPlacedEvent.Items)
            {
                Console.WriteLine($"  - {item.ProductName} x{item.Quantity} @ {item.UnitPrice}");
            }
            Console.WriteLine("=================================");

            return Task.CompletedTask;
        }

        public Task HandleOrderCancelledAsync(OrderCancelledEvent orderCancelledEvent)
        {
            Console.WriteLine("=== ORDER CANCELLED NOTIFICATION ===");
            Console.WriteLine($"Order ID: {orderCancelledEvent.OrderId}");
            Console.WriteLine($"Customer ID: {orderCancelledEvent.CustomerId}");
            Console.WriteLine("Items to restore:");
            foreach (var item in orderCancelledEvent.Items)
            {
                Console.WriteLine($"  - {item.ProductName} x{item.Quantity}");
            }
            Console.WriteLine("====================================");
            
            return Task.CompletedTask;
        }

    }
}
