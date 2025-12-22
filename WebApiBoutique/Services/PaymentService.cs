using Microsoft.EntityFrameworkCore;
using WebApiBoutique.Data;
using WebApiBoutique.Models;
using WebApiBoutique.Models.DTOs;

namespace WebApiBoutique.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly AppDbContext _context;

        public PaymentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync()
        {
            return await _context.Payments
                .Include(p => p.Order)
                .ThenInclude(o => o.Customer)
                .Select(p => new PaymentDto
                {
                    PaymentId = p.PaymentId,
                    OrderId = p.OrderId,
                    OrderDescription = $"Order #{p.OrderId}",
                    CustomerName = p.Order.Customer.CustomerName ?? "",
                    Amount = p.Amount,
                    PaymentMethod = p.PaymentMethod,
                    Status = p.Status,
                    TransactionId = p.TransactionId,
                    PaymentDate = p.PaymentDate,
                    Notes = p.Notes,
                    CreatedAt = p.CreatedAt
                })
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<PaymentDto>> GetPaymentsByBusinessIdAsync(int businessId)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .ThenInclude(o => o.Customer)
                .Where(p => p.BusinessId == businessId)
                .Select(p => new PaymentDto
                {
                    PaymentId = p.PaymentId,
                    OrderId = p.OrderId,
                    OrderDescription = $"Order #{p.OrderId}",
                    CustomerName = p.Order.Customer.CustomerName ?? "",
                    Amount = p.Amount,
                    PaymentMethod = p.PaymentMethod,
                    Status = p.Status,
                    TransactionId = p.TransactionId,
                    PaymentDate = p.PaymentDate,
                    Notes = p.Notes,
                    CreatedAt = p.CreatedAt
                })
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<PaymentDto?> GetPaymentByIdAsync(int id)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .ThenInclude(o => o.Customer)
                .Where(p => p.PaymentId == id)
                .Select(p => new PaymentDto
                {
                    PaymentId = p.PaymentId,
                    OrderId = p.OrderId,
                    OrderDescription = $"Order #{p.OrderId}",
                    CustomerName = p.Order.Customer.CustomerName ?? "",
                    Amount = p.Amount,
                    PaymentMethod = p.PaymentMethod,
                    Status = p.Status,
                    TransactionId = p.TransactionId,
                    PaymentDate = p.PaymentDate,
                    Notes = p.Notes,
                    CreatedAt = p.CreatedAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PaymentDto>> GetPaymentsByOrderAsync(int orderId)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .ThenInclude(o => o.Customer)
                .Where(p => p.OrderId == orderId)
                .Select(p => new PaymentDto
                {
                    PaymentId = p.PaymentId,
                    OrderId = p.OrderId,
                    OrderDescription = $"Order #{p.OrderId}",
                    CustomerName = p.Order.Customer.CustomerName ?? "",
                    Amount = p.Amount,
                    PaymentMethod = p.PaymentMethod,
                    Status = p.Status,
                    TransactionId = p.TransactionId,
                    PaymentDate = p.PaymentDate,
                    Notes = p.Notes,
                    CreatedAt = p.CreatedAt
                })
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<PaymentDto>> GetPaymentsByCustomerAsync(int customerId)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .ThenInclude(o => o.Customer)
                .Where(p => p.Order.CustomerId == customerId)
                .Select(p => new PaymentDto
                {
                    PaymentId = p.PaymentId,
                    OrderId = p.OrderId,
                    OrderDescription = $"Order #{p.OrderId}",
                    CustomerName = p.Order.Customer.CustomerName ?? "",
                    Amount = p.Amount,
                    PaymentMethod = p.PaymentMethod,
                    Status = p.Status,
                    TransactionId = p.TransactionId,
                    PaymentDate = p.PaymentDate,
                    Notes = p.Notes,
                    CreatedAt = p.CreatedAt
                })
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<PaymentDto> CreatePaymentAsync(CreatePaymentDto createDto)
        {
            var order = await _context.Orders.FindAsync(createDto.OrderId);
            if (order == null)
                throw new InvalidOperationException("Order not found");

            if (createDto.Amount > order.RemainingAmount)
                throw new InvalidOperationException("Payment amount exceeds remaining balance");

            var payment = new Payment
            {
                OrderId = createDto.OrderId,
                Amount = createDto.Amount,
                PaymentMethod = createDto.PaymentMethod,
                Status = "Completed",
                TransactionId = createDto.TransactionId,
                PaymentDate = createDto.PaymentDate,
                Notes = createDto.Notes,
                CreatedAt = DateTime.UtcNow
            };

            _context.Payments.Add(payment);
            order.PaidAmount += createDto.Amount;
            await _context.SaveChangesAsync();

            return await GetPaymentByIdAsync(payment.PaymentId) ?? throw new InvalidOperationException("Failed to create payment");
        }

        public async Task<PaymentDto> CreatePaymentAsync(CreatePaymentDto createDto, int businessId)
        {
            var order = await _context.Orders.FindAsync(createDto.OrderId);
            if (order == null)
                throw new InvalidOperationException("Order not found");

            if (createDto.Amount > order.RemainingAmount)
                throw new InvalidOperationException("Payment amount exceeds remaining balance");

            var payment = new Payment
            {
                OrderId = createDto.OrderId,
                Amount = createDto.Amount,
                PaymentMethod = createDto.PaymentMethod,
                Status = "Completed",
                TransactionId = createDto.TransactionId,
                PaymentDate = createDto.PaymentDate,
                Notes = createDto.Notes,
                CreatedAt = DateTime.UtcNow,
                BusinessId = businessId
            };

            _context.Payments.Add(payment);
            order.PaidAmount += createDto.Amount;
            await _context.SaveChangesAsync();

            return await GetPaymentByIdAsync(payment.PaymentId) ?? throw new InvalidOperationException("Failed to create payment");
        }

        public async Task<bool> UpdatePaymentAsync(int id, UpdatePaymentDto updateDto)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null) return false;

            if (updateDto.Amount.HasValue) payment.Amount = updateDto.Amount.Value;
            if (!string.IsNullOrEmpty(updateDto.PaymentMethod)) payment.PaymentMethod = updateDto.PaymentMethod;
            if (!string.IsNullOrEmpty(updateDto.Status)) payment.Status = updateDto.Status;
            if (!string.IsNullOrEmpty(updateDto.TransactionId)) payment.TransactionId = updateDto.TransactionId;
            if (updateDto.PaymentDate.HasValue) payment.PaymentDate = updateDto.PaymentDate.Value;
            if (updateDto.Notes != null) payment.Notes = updateDto.Notes;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePaymentAsync(int id)
        {
            var payment = await _context.Payments.Include(p => p.Order).FirstOrDefaultAsync(p => p.PaymentId == id);
            if (payment == null) return false;

            payment.Order.PaidAmount -= payment.Amount;
            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PaymentSummaryDto> GetPaymentSummaryAsync(int customerId)
        {
            var orders = await _context.Orders
                .Where(o => o.CustomerId == customerId)
                .ToListAsync();

            return new PaymentSummaryDto
            {
                TotalOrders = orders.Count,
                TotalAmount = orders.Sum(o => o.TotalAmount),
                PaidAmount = orders.Sum(o => o.PaidAmount),
                PendingBalance = orders.Sum(o => o.RemainingAmount),
                PaidOrders = orders.Count(o => o.RemainingAmount == 0),
                PendingOrders = orders.Count(o => o.RemainingAmount > 0)
            };
        }
    }
}