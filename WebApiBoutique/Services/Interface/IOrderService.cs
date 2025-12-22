using WebApiBoutique.Models;
using WebApiBoutique.Models.DTOs;

namespace WebApiBoutique.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        Task<IEnumerable<OrderDto>> GetOrdersByBusinessIdAsync(int businessId);
        Task<OrderDto?> GetOrderByIdAsync(int id);
        Task<IEnumerable<OrderDto>> GetFilteredOrdersAsync(OrderFilterDto filter);
        Task<IEnumerable<OrderDto>> GetFilteredOrdersAsync(OrderFilterDto filter, int businessId);
        Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto);
        Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto, int businessId);
        Task<bool> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusDto updateDto);
        Task<bool> DeleteOrderAsync(int id);
        Task<IEnumerable<OrderDto>> GetActiveOrdersAsync();
        Task<IEnumerable<OrderDto>> GetActiveOrdersByBusinessIdAsync(int businessId);
        Task<IEnumerable<OrderDto>> GetOrdersByCustomerAsync(int customerId);
        Task<PaginatedOrdersDto> GetPaginatedOrdersAsync(int page, int pageSize, string? status, string? priority);
        Task<PaginatedOrdersDto> GetPaginatedOrdersAsync(int page, int pageSize, string? status, string? priority, int businessId);
        Task<OrderTimelineDto?> GetOrderTimelineAsync(int orderId);
    }
}