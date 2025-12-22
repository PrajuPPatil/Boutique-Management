using Microsoft.JSInterop;

namespace Boutique.Client.Services
{
    // Client-side notification service for toast messages and alerts
    public class NotificationService
    {
        // JavaScript runtime for browser API calls
        private readonly IJSRuntime _jsRuntime;
        // In-memory list of active notifications
        private readonly List<NotificationDto> _notifications = new();
        // Event for notifying UI components of notification changes
        public event Action? OnNotificationsChanged;

        // Constructor with dependency injection for JavaScript runtime
        public NotificationService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        // Show toast notification with specified type and duration
        public async Task ShowToastAsync(string message, NotificationType type = NotificationType.Info, int duration = 3000)
        {
            // Create notification object
            var notification = new NotificationDto
            {
                Id = Guid.NewGuid().ToString(),
                Message = message,
                Type = type,
                Timestamp = DateTime.Now,
                Duration = duration
            };

            // Add to notifications list and notify UI
            _notifications.Add(notification);
            OnNotificationsChanged?.Invoke();

            // Auto-remove notification after specified duration
            _ = Task.Delay(duration).ContinueWith(_ => RemoveNotification(notification.Id));

            // Call JavaScript function to display toast
            await _jsRuntime.InvokeVoidAsync("showToast", message, type.ToString().ToLower(), duration);
        }

        // Convenience methods for different notification types
        public async Task ShowSuccessAsync(string message) => await ShowToastAsync(message, NotificationType.Success);
        public async Task ShowErrorAsync(string message) => await ShowToastAsync(message, NotificationType.Error, 5000);
        public async Task ShowWarningAsync(string message) => await ShowToastAsync(message, NotificationType.Warning, 4000);
        public async Task ShowInfoAsync(string message) => await ShowToastAsync(message, NotificationType.Info);

        public void RemoveNotification(string id)
        {
            _notifications.RemoveAll(n => n.Id == id);
            OnNotificationsChanged?.Invoke();
        }

        public List<NotificationDto> GetNotifications() => _notifications.ToList();

        // Check for orders with approaching due dates and show warnings
        public async Task CheckDueDatesAsync(List<OrderDto> orders)
        {
            // Find orders due within 2 days or overdue
            var dueSoon = orders.Where(o => 
                o.Status != "Delivered" && 
                o.EstimatedDeliveryDate <= DateTime.Now.AddDays(2) &&
                o.EstimatedDeliveryDate >= DateTime.Now.AddDays(-1)
            ).ToList();

            // Show warning notification for each due/overdue order
            foreach (var order in dueSoon)
            {
                var daysUntilDue = (order.EstimatedDeliveryDate - DateTime.Now).Days;
                var message = daysUntilDue <= 0 
                    ? $"Order #{order.OrderId} for {order.CustomerName} is overdue!"
                    : $"Order #{order.OrderId} for {order.CustomerName} is due in {daysUntilDue} day(s)";
                
                await ShowWarningAsync(message);
            }
        }
    }

    // Notification data model for toast messages
    public class NotificationDto
    {
        // Unique identifier for notification
        public string Id { get; set; } = "";
        // Message text to display
        public string Message { get; set; } = "";
        // Type of notification for styling
        public NotificationType Type { get; set; }
        // When notification was created
        public DateTime Timestamp { get; set; }
        // How long to display notification (milliseconds)
        public int Duration { get; set; }
    }

    // Enumeration of notification types for styling and behavior
    public enum NotificationType
    {
        Success,  // Green - successful operations
        Error,    // Red - error messages
        Warning,  // Yellow - warnings and alerts
        Info      // Blue - informational messages
    }
}