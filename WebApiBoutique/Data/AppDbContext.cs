using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebApiBoutique.Models;
using Microsoft.Data.SqlClient; 
using WebApiBoutique.Models.DTOs;

namespace WebApiBoutique.Data
{
    // Database context class managing all entity models and database operations
    public class AppDbContext : DbContext
    {
        // HTTP context accessor for tracking current user in audit operations
        private readonly IHttpContextAccessor _httpContextAccessor;

        // Constructor to initialize database context with options and HTTP context
        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // DbSet for authenticated users
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        // Core business entities
        public DbSet<Customer> Customers { get; set; }                      // Customer records
        public DbSet<TypeModel> Types { get; set; }                         // Garment types
        public DbSet<Measurement> Measurements { get; set; }                // Measurement records
        public DbSet<CustomerMeasurement> CustomerMeasurements { get; set; }// Detailed measurements
        public DbSet<Delivery_Status> DeliveryStatuses { get; set; }       // Delivery tracking
        public DbSet<Payment> Payments { get; set; }                        // Payment transactions
        public DbSet<Order> Orders { get; set; }                            // Customer orders
        // Women's garment measurement tables
        public DbSet<W_KurtiWomen> KurtiWomen { get; set; }                // Women's Kurti measurements
        public DbSet<W_PayjamaWomen> PayjamaWomen { get; set; }            // Women's Payjama measurements
        public DbSet<W_Frock> Frocks { get; set; }                         // Frock measurements
        public DbSet<W_BlazerWomen> Blazer_Women { get; set; }             // Women's Blazer measurements
        public DbSet<W_TopWomen> TopWomen { get; set; }                    // Women's Top measurements
        
        // Men's garment measurement tables
        public DbSet<M_ShirtMen> ShirtMen { get; set; }                    // Men's Shirt measurements
        public DbSet<M_PantMen> PantMen { get; set; }                      // Men's Pant measurements
        public DbSet<M_TShirtMen> TShirtMen { get; set; }                  // Men's T-Shirt measurements
        public DbSet<M_KurtaMen> KurtaMen { get; set; }                    // Men's Kurta measurements
        public DbSet<M_PayjamaMen> PayjamaMen { get; set; }                // Men's Payjama measurements
        
        // System user and audit tables
        public new DbSet<User> Users { get; set; }                         // System users
        public DbSet<PasswordHistory> PasswordHistories { get; set; }      // Password change history


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Role).HasMaxLength(20).IsRequired();
                entity.Property(e => e.ConfirmationToken).HasMaxLength(100);
                entity.Property(e => e.PasswordResetToken).HasMaxLength(100);
                entity.Property(e => e.OtpCode).HasMaxLength(6);
                entity.HasIndex(e => e.Email).IsUnique();
            });
            
            // Ignore system types that EF might try to include
            modelBuilder.Ignore<System.Reflection.CustomAttributeData>();
            modelBuilder.Ignore<System.Type>();
            modelBuilder.Ignore<System.Reflection.MemberInfo>();
            modelBuilder.Ignore<System.Reflection.PropertyInfo>();
            
            // Configure keyless entities for DTOs - exclude from migrations
            modelBuilder.Entity<CustomerMeasurementDTO>().HasNoKey().ToView(null);
            modelBuilder.Entity<CustomerDeliveryDTO>().HasNoKey().ToView(null);
            modelBuilder.Entity<CustomerPaymentDTO>().HasNoKey().ToView(null);
            modelBuilder.Entity<PasswordHistoryDTO>().HasNoKey().ToView(null);


            modelBuilder.Entity<W_BlazerWomen>()
                .HasOne(b => b.Measurement)
                .WithMany()
                .HasForeignKey(b => b.MeasurementId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<W_BlazerWomen>()
                .HasOne(b => b.Type)
                .WithMany()
                .HasForeignKey(b => b.TypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<W_Frock>()
                .HasOne(f => f.Type)
                .WithMany()
                .HasForeignKey(f => f.TypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<M_KurtaMen>()
                .HasOne(k => k.Type)
                .WithMany()
                .HasForeignKey(k => k.TypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<W_KurtiWomen>()
                .HasOne(k => k.Type)
                .WithMany()
                .HasForeignKey(k => k.TypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<M_PantMen>()
                .HasOne(p => p.Type)
                .WithMany()
                .HasForeignKey(p => p.TypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<M_PayjamaMen>()
                .HasOne(p => p.Type)
                .WithMany()
                .HasForeignKey(p => p.TypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<W_PayjamaWomen>()
                .HasOne(p => p.Type)
                .WithMany()
                .HasForeignKey(p => p.TypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<M_ShirtMen>()
                .HasOne(s => s.Type)
                .WithMany()
                .HasForeignKey(s => s.TypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<W_TopWomen>()
                .HasOne(s => s.Type)
                .WithMany()
                .HasForeignKey(s => s.TypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<M_TShirtMen>()
                .HasOne(s => s.Type)
                .WithMany()
                .HasForeignKey(s => s.TypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.PaymentId);
                entity.Property(e => e.Amount).HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(e => e.PaymentMethod).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Status).HasMaxLength(20).IsRequired();
                entity.Property(e => e.TransactionId).HasMaxLength(100);
                entity.Property(e => e.Notes).HasMaxLength(500);
                
                entity.HasOne(e => e.Order)
                    .WithMany(o => o.Payments)
                    .HasForeignKey(e => e.OrderId)
                    .OnDelete(DeleteBehavior.Restrict);
                    
                entity.HasIndex(e => e.OrderId);
                entity.HasIndex(e => e.PaymentDate);
            });

            // CustomerMeasurement configuration with validation constraints
            modelBuilder.Entity<CustomerMeasurement>(entity =>
            {
                entity.HasKey(e => e.MeasurementId);
                entity.Property(e => e.Gender).HasMaxLength(1).IsRequired();
                entity.Property(e => e.MeasurementType).HasMaxLength(50).IsRequired();
                entity.Property(e => e.MeasurementValue).HasColumnType("decimal(5,2)").IsRequired();
                entity.Property(e => e.Unit).HasMaxLength(10).HasDefaultValue("inches");
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
                
                entity.HasOne(e => e.Customer)
                    .WithMany()
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasCheckConstraint("CK_Gender", "Gender IN ('M', 'F')");
                entity.HasCheckConstraint("CK_MenChest", "NOT (Gender = 'M' AND MeasurementType = 'Chest' AND (MeasurementValue < 34 OR MeasurementValue > 52))");
                entity.HasCheckConstraint("CK_MenWaist", "NOT (Gender = 'M' AND MeasurementType = 'Waist' AND (MeasurementValue < 28 OR MeasurementValue > 44))");
                entity.HasCheckConstraint("CK_MenHips", "NOT (Gender = 'M' AND MeasurementType = 'Hips' AND (MeasurementValue < 34 OR MeasurementValue > 48))");
                entity.HasCheckConstraint("CK_MenShoulder", "NOT (Gender = 'M' AND MeasurementType = 'Shoulder' AND (MeasurementValue < 15 OR MeasurementValue > 22))");
                entity.HasCheckConstraint("CK_MenSleeveLength", "NOT (Gender = 'M' AND MeasurementType = 'Sleeve Length' AND (MeasurementValue < 22 OR MeasurementValue > 27))");
                entity.HasCheckConstraint("CK_MenNeck", "NOT (Gender = 'M' AND MeasurementType = 'Neck' AND (MeasurementValue < 14 OR MeasurementValue > 20))");
                entity.HasCheckConstraint("CK_WomenBust", "NOT (Gender = 'F' AND MeasurementType = 'Bust' AND (MeasurementValue < 30 OR MeasurementValue > 46))");
                entity.HasCheckConstraint("CK_WomenWaist", "NOT (Gender = 'F' AND MeasurementType = 'Waist' AND (MeasurementValue < 24 OR MeasurementValue > 40))");
                entity.HasCheckConstraint("CK_WomenHips", "NOT (Gender = 'F' AND MeasurementType = 'Hips' AND (MeasurementValue < 32 OR MeasurementValue > 48))");
                entity.HasCheckConstraint("CK_WomenShoulder", "NOT (Gender = 'F' AND MeasurementType = 'Shoulder' AND (MeasurementValue < 13 OR MeasurementValue > 19))");
                entity.HasCheckConstraint("CK_WomenUpperArm", "NOT (Gender = 'F' AND MeasurementType = 'Upper Arm' AND (MeasurementValue < 10 OR MeasurementValue > 16))");
                
                entity.HasIndex(e => new { e.CustomerId, e.Gender });
            });

        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is ApplicationUser &&
                            (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var user = (ApplicationUser)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    user.CreatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    user.UpdatedAt = DateTime.UtcNow;
                    user.UpdatedBy = GetCurrentUser();
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        private string GetCurrentUser()
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "system";
        }

        //stored procedure for all customer
        public async Task<List<CustomerMeasurementDTO>> GetCustomerMeasurementDetailsAsync()
        {
            return await Set<CustomerMeasurementDTO>()
                .FromSqlRaw("EXEC GetCustomerMeasurementDetails")
                .ToListAsync();
        }

        //stored procedure by Name
        public async Task<List<CustomerMeasurementDTO>> GetCustomerMeasurementDetailsByNameAsync(string name)
        {
            var result = await Set<CustomerMeasurementDTO>()
                .FromSqlInterpolated($"EXEC GetCustomerMeasurementDetailsByName @CustomerName = {name}")
                .ToListAsync();

            Console.WriteLine($"Returned {result.Count} rows");
            return result;
        }

        //stored procedure for GetCustomerDeliveryDetails
        public async Task<List<CustomerDeliveryDTO>> GetCustomerDeliveryDetailsAsync()
        {
            return await Set<CustomerDeliveryDTO>()
                .FromSqlRaw("EXEC GetCustomerDeliveryDetails")
                .ToListAsync();
        }

        //stored procedure for GetCustomerDeliveryDetailsByName
        public async Task<List<CustomerDeliveryDTO>> GetCustomerDeliveryDetailsByNameAsync(string name)
        {
            return await Set<CustomerDeliveryDTO>()
                .FromSqlInterpolated($"EXEC GetCustomerDeliveryDetailsByName @CustomerName = {name}")
                .ToListAsync();
        }

    
   
    }
}
