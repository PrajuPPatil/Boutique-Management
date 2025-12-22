namespace WebApiBoutique.Models.DTOs
{
    // Data Transfer Object for order information with customer details
    public class OrderDto
    {
        public int OrderId { get; set; }                    // Unique order identifier
        public int CustomerId { get; set; }                 // Customer who placed the order
        public string CustomerName { get; set; } = string.Empty;  // Customer's name
        public string Email { get; set; } = string.Empty;         // Customer's email
        public string PhoneNo { get; set; } = string.Empty;       // Customer's phone
        public int? MeasurementId { get; set; }             // Associated measurement record
        public string TypeName { get; set; } = string.Empty;      // Garment type name
        public string Status { get; set; } = string.Empty;        // Order status
        public string Priority { get; set; } = string.Empty;      // Priority level
        public DateTime OrderDate { get; set; }             // When order was placed
        public DateTime EstimatedDeliveryDate { get; set; } // Expected delivery date
        public DateTime? ActualDeliveryDate { get; set; }   // Actual delivery date
        public string? Notes { get; set; }                  // Special instructions
        public decimal TotalAmount { get; set; }            // Total order value
        public decimal PaidAmount { get; set; }             // Amount already paid
        public decimal RemainingAmount { get; set; }        // Outstanding balance
        public bool IsActive { get; set; }                  // Order active status
    }

    // DTO for creating new orders
    public class CreateOrderDto
    {
        public int CustomerId { get; set; }                 // Customer placing the order
        public int? MeasurementId { get; set; }             // Optional measurement reference
        public string Priority { get; set; } = "Regular";   // Priority level (Regular/Urgent/Express)
        public decimal TotalAmount { get; set; }            // Total order cost
        public string? Notes { get; set; }                  // Optional special instructions
    }

    // DTO for updating order status
    public class UpdateOrderStatusDto
    {
        public string Status { get; set; } = string.Empty;  // New status (Pending/InProgress/ReadyForDelivery/Delivered)
        public string? Notes { get; set; }                  // Optional status change notes
    }

    // DTO for filtering orders with multiple criteria
    public class OrderFilterDto
    {
        public string? Status { get; set; }      // Filter by order status
        public string? Priority { get; set; }    // Filter by priority level
        public int? CustomerId { get; set; }     // Filter by specific customer
        public DateTime? StartDate { get; set; } // Filter by date range start
        public DateTime? EndDate { get; set; }   // Filter by date range end
    }

    // DTO for paginated order results
    public class PaginatedOrdersDto
    {
        public List<OrderDto> Orders { get; set; } = new();  // Orders for current page
        public int CurrentPage { get; set; }                 // Current page number
        public int PageSize { get; set; }                    // Items per page
        public int TotalItems { get; set; }                  // Total number of orders
        public int TotalPages { get; set; }                  // Total number of pages
    }

    // DTO for order status timeline tracking
    public class OrderTimelineDto
    {
        public int OrderId { get; set; }                                    // Order being tracked
        public List<OrderStatusHistoryDto> StatusHistory { get; set; } = new();  // Status change history
    }

    // DTO for individual status change records
    public class OrderStatusHistoryDto
    {
        public string Status { get; set; } = string.Empty;   // Status at this point
        public DateTime ChangedDate { get; set; }            // When status changed
        public string? Notes { get; set; }                   // Notes about the change
        public string ChangedBy { get; set; } = string.Empty; // Who made the change
    }
}