using Microsoft.EntityFrameworkCore;
using WebApiBoutique.Data;
using WebApiBoutique.Models;
using WebApiBoutique.Models.DTOs;

namespace WebApiBoutique.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            try
            {
                return await _context.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.Measurement)
                    .ThenInclude(m => m.Type)
                    .Select(o => new OrderDto
                    {
                        OrderId = o.OrderId,
                        CustomerId = o.CustomerId,
                        CustomerName = o.Customer.CustomerName ?? "",
                        Email = o.Customer.Email ?? "",
                        PhoneNo = o.Customer.PhoneNo ?? "",
                        MeasurementId = o.MeasurementId,
                        TypeName = o.Measurement.Type != null ? o.Measurement.Type.TypeName : "",
                        Status = o.Status,
                        Priority = o.Priority,
                        OrderDate = o.OrderDate,
                        EstimatedDeliveryDate = o.EstimatedDeliveryDate,
                        ActualDeliveryDate = o.ActualDeliveryDate,
                        Notes = o.Notes,
                        TotalAmount = o.TotalAmount,
                        PaidAmount = o.PaidAmount,
                        RemainingAmount = o.RemainingAmount,
                        IsActive = o.IsActive
                    })
                    .ToListAsync();
            }
            catch (Exception)
            {
                return new List<OrderDto>();
            }
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByBusinessIdAsync(int businessId)
        {
            try
            {
                return await _context.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.Measurement)
                    .ThenInclude(m => m.Type)
                    .Where(o => o.BusinessId == businessId)
                    .Select(o => new OrderDto
                    {
                        OrderId = o.OrderId,
                        CustomerId = o.CustomerId,
                        CustomerName = o.Customer.CustomerName ?? "",
                        Email = o.Customer.Email ?? "",
                        PhoneNo = o.Customer.PhoneNo ?? "",
                        MeasurementId = o.MeasurementId,
                        TypeName = o.Measurement.Type != null ? o.Measurement.Type.TypeName : "",
                        Status = o.Status,
                        Priority = o.Priority,
                        OrderDate = o.OrderDate,
                        EstimatedDeliveryDate = o.EstimatedDeliveryDate,
                        ActualDeliveryDate = o.ActualDeliveryDate,
                        Notes = o.Notes,
                        TotalAmount = o.TotalAmount,
                        PaidAmount = o.PaidAmount,
                        RemainingAmount = o.RemainingAmount,
                        IsActive = o.IsActive
                    })
                    .ToListAsync();
            }
            catch (Exception)
            {
                return new List<OrderDto>();
            }
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Measurement)
                .ThenInclude(m => m.Type)
                .Where(o => o.OrderId == id)
                .Select(o => new OrderDto
                {
                    OrderId = o.OrderId,
                    CustomerId = o.CustomerId,
                    CustomerName = o.Customer.CustomerName,
                    Email = o.Customer.Email,
                    PhoneNo = o.Customer.PhoneNo,
                    MeasurementId = o.MeasurementId,
                    TypeName = o.Measurement.Type.TypeName,
                    Status = o.Status,
                    Priority = o.Priority,
                    OrderDate = o.OrderDate,
                    EstimatedDeliveryDate = o.EstimatedDeliveryDate,
                    ActualDeliveryDate = o.ActualDeliveryDate,
                    Notes = o.Notes,
                    TotalAmount = o.TotalAmount,
                    PaidAmount = o.PaidAmount,
                    RemainingAmount = o.RemainingAmount,
                    IsActive = o.IsActive
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<OrderDto>> GetFilteredOrdersAsync(OrderFilterDto filter)
        {
            var query = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Measurement)
                .ThenInclude(m => m.Type)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.Status))
                query = query.Where(o => o.Status == filter.Status);

            if (!string.IsNullOrEmpty(filter.Priority))
                query = query.Where(o => o.Priority == filter.Priority);

            if (filter.CustomerId.HasValue)
                query = query.Where(o => o.CustomerId == filter.CustomerId);

            if (filter.StartDate.HasValue)
                query = query.Where(o => o.OrderDate >= filter.StartDate);

            if (filter.EndDate.HasValue)
                query = query.Where(o => o.OrderDate <= filter.EndDate);

            return await query
                .Select(o => new OrderDto
                {
                    OrderId = o.OrderId,
                    CustomerId = o.CustomerId,
                    CustomerName = o.Customer.CustomerName,
                    Email = o.Customer.Email,
                    PhoneNo = o.Customer.PhoneNo,
                    MeasurementId = o.MeasurementId,
                    TypeName = o.Measurement.Type.TypeName,
                    Status = o.Status,
                    Priority = o.Priority,
                    OrderDate = o.OrderDate,
                    EstimatedDeliveryDate = o.EstimatedDeliveryDate,
                    ActualDeliveryDate = o.ActualDeliveryDate,
                    Notes = o.Notes,
                    TotalAmount = o.TotalAmount,
                    PaidAmount = o.PaidAmount,
                    RemainingAmount = o.RemainingAmount,
                    IsActive = o.IsActive
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<OrderDto>> GetFilteredOrdersAsync(OrderFilterDto filter, int businessId)
        {
            var query = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Measurement)
                .ThenInclude(m => m.Type)
                .Where(o => o.BusinessId == businessId)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.Status))
                query = query.Where(o => o.Status == filter.Status);

            if (!string.IsNullOrEmpty(filter.Priority))
                query = query.Where(o => o.Priority == filter.Priority);

            if (filter.CustomerId.HasValue)
                query = query.Where(o => o.CustomerId == filter.CustomerId);

            if (filter.StartDate.HasValue)
                query = query.Where(o => o.OrderDate >= filter.StartDate);

            if (filter.EndDate.HasValue)
                query = query.Where(o => o.OrderDate <= filter.EndDate);

            return await query
                .Select(o => new OrderDto
                {
                    OrderId = o.OrderId,
                    CustomerId = o.CustomerId,
                    CustomerName = o.Customer.CustomerName,
                    Email = o.Customer.Email,
                    PhoneNo = o.Customer.PhoneNo,
                    MeasurementId = o.MeasurementId,
                    TypeName = o.Measurement.Type.TypeName,
                    Status = o.Status,
                    Priority = o.Priority,
                    OrderDate = o.OrderDate,
                    EstimatedDeliveryDate = o.EstimatedDeliveryDate,
                    ActualDeliveryDate = o.ActualDeliveryDate,
                    Notes = o.Notes,
                    TotalAmount = o.TotalAmount,
                    PaidAmount = o.PaidAmount,
                    RemainingAmount = o.RemainingAmount,
                    IsActive = o.IsActive
                })
                .ToListAsync();
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            var estimatedDelivery = CalculateEstimatedDelivery(createOrderDto.Priority);
            
            var order = new Order
            {
                CustomerId = createOrderDto.CustomerId,
                MeasurementId = createOrderDto.MeasurementId,
                Priority = createOrderDto.Priority,
                TotalAmount = createOrderDto.TotalAmount,
                Notes = createOrderDto.Notes,
                EstimatedDeliveryDate = estimatedDelivery
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return await GetOrderByIdAsync(order.OrderId) ?? throw new InvalidOperationException("Failed to create order");
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto, int businessId)
        {
            var estimatedDelivery = CalculateEstimatedDelivery(createOrderDto.Priority);
            
            var order = new Order
            {
                CustomerId = createOrderDto.CustomerId,
                MeasurementId = createOrderDto.MeasurementId,
                Priority = createOrderDto.Priority,
                TotalAmount = createOrderDto.TotalAmount,
                Notes = createOrderDto.Notes,
                EstimatedDeliveryDate = estimatedDelivery,
                BusinessId = businessId
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return await GetOrderByIdAsync(order.OrderId) ?? throw new InvalidOperationException("Failed to create order");
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusDto updateDto)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return false;

            order.Status = updateDto.Status;
            
            if (!string.IsNullOrEmpty(updateDto.Notes))
                order.Notes = updateDto.Notes;

            if (updateDto.Status == "Delivered")
                order.ActualDeliveryDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return false;

            order.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<OrderDto>> GetActiveOrdersAsync()
        {
            try
            {
                return await _context.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.Measurement)
                    .ThenInclude(m => m.Type)
                    .Where(o => o.IsActive && o.Status != "Delivered")
                    .Select(o => new OrderDto
                    {
                        OrderId = o.OrderId,
                        CustomerId = o.CustomerId,
                        CustomerName = o.Customer.CustomerName ?? "",
                        Email = o.Customer.Email ?? "",
                        PhoneNo = o.Customer.PhoneNo ?? "",
                        MeasurementId = o.MeasurementId,
                        TypeName = o.Measurement.Type != null ? o.Measurement.Type.TypeName : "",
                        Status = o.Status,
                        Priority = o.Priority,
                        OrderDate = o.OrderDate,
                        EstimatedDeliveryDate = o.EstimatedDeliveryDate,
                        ActualDeliveryDate = o.ActualDeliveryDate,
                        Notes = o.Notes,
                        TotalAmount = o.TotalAmount,
                        PaidAmount = o.PaidAmount,
                        RemainingAmount = o.RemainingAmount,
                        IsActive = o.IsActive
                    })
                    .ToListAsync();
            }
            catch (Exception)
            {
                return new List<OrderDto>();
            }
        }

        public async Task<IEnumerable<OrderDto>> GetActiveOrdersByBusinessIdAsync(int businessId)
        {
            try
            {
                return await _context.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.Measurement)
                    .ThenInclude(m => m.Type)
                    .Where(o => o.IsActive && o.Status != "Delivered" && o.BusinessId == businessId)
                    .Select(o => new OrderDto
                    {
                        OrderId = o.OrderId,
                        CustomerId = o.CustomerId,
                        CustomerName = o.Customer.CustomerName ?? "",
                        Email = o.Customer.Email ?? "",
                        PhoneNo = o.Customer.PhoneNo ?? "",
                        MeasurementId = o.MeasurementId,
                        TypeName = o.Measurement.Type != null ? o.Measurement.Type.TypeName : "",
                        Status = o.Status,
                        Priority = o.Priority,
                        OrderDate = o.OrderDate,
                        EstimatedDeliveryDate = o.EstimatedDeliveryDate,
                        ActualDeliveryDate = o.ActualDeliveryDate,
                        Notes = o.Notes,
                        TotalAmount = o.TotalAmount,
                        PaidAmount = o.PaidAmount,
                        RemainingAmount = o.RemainingAmount,
                        IsActive = o.IsActive
                    })
                    .ToListAsync();
            }
            catch (Exception)
            {
                return new List<OrderDto>();
            }
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByCustomerAsync(int customerId)
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Measurement)
                .ThenInclude(m => m.Type)
                .Where(o => o.CustomerId == customerId)
                .Select(o => new OrderDto
                {
                    OrderId = o.OrderId,
                    CustomerId = o.CustomerId,
                    CustomerName = o.Customer.CustomerName,
                    Email = o.Customer.Email,
                    PhoneNo = o.Customer.PhoneNo,
                    MeasurementId = o.MeasurementId,
                    TypeName = o.Measurement.Type.TypeName,
                    Status = o.Status,
                    Priority = o.Priority,
                    OrderDate = o.OrderDate,
                    EstimatedDeliveryDate = o.EstimatedDeliveryDate,
                    ActualDeliveryDate = o.ActualDeliveryDate,
                    Notes = o.Notes,
                    TotalAmount = o.TotalAmount,
                    PaidAmount = o.PaidAmount,
                    RemainingAmount = o.RemainingAmount,
                    IsActive = o.IsActive
                })
                .ToListAsync();
        }

        public async Task<PaginatedOrdersDto> GetPaginatedOrdersAsync(int page, int pageSize, string? status, string? priority)
        {
            var query = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Measurement)
                .ThenInclude(m => m.Type)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status))
                query = query.Where(o => o.Status == status);

            if (!string.IsNullOrEmpty(priority))
                query = query.Where(o => o.Priority == priority);

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var orders = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(o => new OrderDto
                {
                    OrderId = o.OrderId,
                    CustomerId = o.CustomerId,
                    CustomerName = o.Customer.CustomerName ?? "",
                    Email = o.Customer.Email ?? "",
                    PhoneNo = o.Customer.PhoneNo ?? "",
                    MeasurementId = o.MeasurementId,
                    TypeName = o.Measurement.Type != null ? o.Measurement.Type.TypeName : "",
                    Status = o.Status,
                    Priority = o.Priority,
                    OrderDate = o.OrderDate,
                    EstimatedDeliveryDate = o.EstimatedDeliveryDate,
                    ActualDeliveryDate = o.ActualDeliveryDate,
                    Notes = o.Notes,
                    TotalAmount = o.TotalAmount,
                    PaidAmount = o.PaidAmount,
                    RemainingAmount = o.RemainingAmount,
                    IsActive = o.IsActive
                })
                .ToListAsync();

            return new PaginatedOrdersDto
            {
                Orders = orders,
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            };
        }

        public async Task<PaginatedOrdersDto> GetPaginatedOrdersAsync(int page, int pageSize, string? status, string? priority, int businessId)
        {
            var query = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Measurement)
                .ThenInclude(m => m.Type)
                .Where(o => o.BusinessId == businessId)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status))
                query = query.Where(o => o.Status == status);

            if (!string.IsNullOrEmpty(priority))
                query = query.Where(o => o.Priority == priority);

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var orders = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(o => new OrderDto
                {
                    OrderId = o.OrderId,
                    CustomerId = o.CustomerId,
                    CustomerName = o.Customer.CustomerName ?? "",
                    Email = o.Customer.Email ?? "",
                    PhoneNo = o.Customer.PhoneNo ?? "",
                    MeasurementId = o.MeasurementId,
                    TypeName = o.Measurement.Type != null ? o.Measurement.Type.TypeName : "",
                    Status = o.Status,
                    Priority = o.Priority,
                    OrderDate = o.OrderDate,
                    EstimatedDeliveryDate = o.EstimatedDeliveryDate,
                    ActualDeliveryDate = o.ActualDeliveryDate,
                    Notes = o.Notes,
                    TotalAmount = o.TotalAmount,
                    PaidAmount = o.PaidAmount,
                    RemainingAmount = o.RemainingAmount,
                    IsActive = o.IsActive
                })
                .ToListAsync();

            return new PaginatedOrdersDto
            {
                Orders = orders,
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            };
        }

        public async Task<OrderTimelineDto?> GetOrderTimelineAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return null;

            var timeline = new OrderTimelineDto
            {
                OrderId = orderId,
                StatusHistory = new List<OrderStatusHistoryDto>
                {
                    new OrderStatusHistoryDto
                    {
                        Status = "Pending",
                        ChangedDate = order.OrderDate,
                        Notes = "Order created",
                        ChangedBy = "System"
                    }
                }
            };

            if (order.Status != "Pending")
            {
                timeline.StatusHistory.Add(new OrderStatusHistoryDto
                {
                    Status = order.Status,
                    ChangedDate = DateTime.UtcNow,
                    Notes = order.Notes,
                    ChangedBy = "Admin"
                });
            }

            return timeline;
        }

        private DateTime CalculateEstimatedDelivery(string priority)
        {
            var baseDays = priority switch
            {
                "Express" => 3,
                "Urgent" => 7,
                "Regular" => 14,
                _ => 14
            };
            return DateTime.UtcNow.AddDays(baseDays);
        }
    }
}