using System.Text;
using System.Text.Json;

namespace Boutique.Client.Services
{
    // Order management service for handling order operations and status tracking
    public class OrderService
    {
        // HTTP client for API communication with order endpoints
        private readonly HttpClient _httpClient;
        // JSON serialization options for case-insensitive property matching
        private readonly JsonSerializerOptions _jsonOptions;

        // Constructor with dependency injection for HTTP client
        public OrderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            // Configure JSON options for API compatibility
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        // Get all orders from the system
        public async Task<List<OrderDto>> GetAllOrdersAsync()
        {
            var response = await _httpClient.GetAsync("api/order");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<OrderDto>>(json, _jsonOptions) ?? new List<OrderDto>();
            }
            return new List<OrderDto>();
        }

        // Get only active orders (not completed or cancelled)
        public async Task<List<OrderDto>> GetActiveOrdersAsync()
        {
            var response = await _httpClient.GetAsync("api/order/active");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<OrderDto>>(json, _jsonOptions) ?? new List<OrderDto>();
            }
            return new List<OrderDto>();
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/order/{id}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<OrderDto>(json, _jsonOptions);
            }
            return null;
        }

        public async Task<List<OrderDto>> GetFilteredOrdersAsync(OrderFilterDto filter)
        {
            var json = JsonSerializer.Serialize(filter, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/order/filter", content);
            
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<OrderDto>>(responseJson, _jsonOptions) ?? new List<OrderDto>();
            }
            return new List<OrderDto>();
        }

        // Create new order in the system
        public async Task<OrderDto?> CreateOrderAsync(CreateOrderDto createOrder)
        {
            var json = JsonSerializer.Serialize(createOrder, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/order", content);
            
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<OrderDto>(responseJson, _jsonOptions);
            }
            return null;
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusDto updateDto)
        {
            var json = JsonSerializer.Serialize(updateDto, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/order/{orderId}/status", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<PaginatedOrdersDto> GetPaginatedOrdersAsync(int page = 1, int pageSize = 10, string? status = null, string? priority = null)
        {
            var queryParams = new List<string> { $"page={page}", $"pageSize={pageSize}" };
            if (!string.IsNullOrEmpty(status)) queryParams.Add($"status={status}");
            if (!string.IsNullOrEmpty(priority)) queryParams.Add($"priority={priority}");
            
            var query = string.Join("&", queryParams);
            var response = await _httpClient.GetAsync($"api/order/paginated?{query}");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<PaginatedOrdersDto>(json, _jsonOptions) ?? new PaginatedOrdersDto();
            }
            return new PaginatedOrdersDto();
        }

        public async Task<List<OrderDto>> GetOrdersByCustomerAsync(int customerId)
        {
            var response = await _httpClient.GetAsync($"api/order/customer/{customerId}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<OrderDto>>(json, _jsonOptions) ?? new List<OrderDto>();
            }
            return new List<OrderDto>();
        }

        public async Task<OrderTimelineDto?> GetOrderTimelineAsync(int orderId)
        {
            var response = await _httpClient.GetAsync($"api/order/{orderId}/timeline");
            if (response.IsSuccessStatusCode)
            {
            var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<OrderTimelineDto>(json, _jsonOptions);
            }
            return null;
        }
    }

    // Data Transfer Object for order information with customer and payment details
    public class OrderDto
    {
        // Unique order identifier
        public int OrderId { get; set; }
        // Customer who placed the order
        public int CustomerId { get; set; }
        // Customer name for display
        public string CustomerName { get; set; } = string.Empty;
        // Customer email for communication
        public string Email { get; set; } = string.Empty;
        // Customer phone for contact
        public string PhoneNo { get; set; } = string.Empty;
        // Associated measurement record
        public int? MeasurementId { get; set; }
        // Garment type name
        public string TypeName { get; set; } = string.Empty;
        // Order status (Pending, InProgress, ReadyForDelivery, Delivered)
        public string Status { get; set; } = string.Empty;
        // Order priority (Regular, Urgent, Express)
        public string Priority { get; set; } = string.Empty;
        // When order was placed
        public DateTime OrderDate { get; set; }
        // Expected delivery date
        public DateTime EstimatedDeliveryDate { get; set; }
        // Actual delivery date (if completed)
        public DateTime? ActualDeliveryDate { get; set; }
        // Additional order notes
        public string? Notes { get; set; }
        // Total order amount
        public decimal TotalAmount { get; set; }
        // Amount already paid
        public decimal PaidAmount { get; set; }
        // Outstanding balance
        public decimal RemainingAmount { get; set; }
        // Whether order is still active
        public bool IsActive { get; set; }
        // Type of garment being made
        public string? GarmentType { get; set; }
        // Fabric specifications
        public string? FabricDetails { get; set; }
    }

    public class CreateOrderDto
    {
        public int CustomerId { get; set; }
        public int? MeasurementId { get; set; }
        public string Priority { get; set; } = "Regular";
        public decimal TotalAmount { get; set; }
        public string? Notes { get; set; }
    }

    public class UpdateOrderStatusDto
    {
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }

    public class OrderFilterDto
    {
        public string? Status { get; set; }
        public string? Priority { get; set; }
        public int? CustomerId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class PaginatedOrdersDto
    {
        public List<OrderDto> Orders { get; set; } = new();
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }

    public class OrderTimelineDto
    {
        public int OrderId { get; set; }
        public List<OrderStatusHistoryDto> StatusHistory { get; set; } = new();
    }

    public class OrderStatusHistoryDto
    {
        public string Status { get; set; } = string.Empty;
        public DateTime ChangedDate { get; set; }
        public string? Notes { get; set; }
        public string ChangedBy { get; set; } = string.Empty;
    }
}