using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BasicDelivery.Domain.Entities
{
    public partial class deliveryDbContext : DbContext
    {
        public deliveryDbContext()
        {
        }

        public deliveryDbContext(DbContextOptions<deliveryDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Driver> Drivers { get; set; } = null!;
        public virtual DbSet<DriverDetail> DriverDetails { get; set; } = null!;
        public virtual DbSet<DriverToken> DriverTokens { get; set; } = null!;
        public virtual DbSet<History> Histories { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<StatusOrder> StatusOrders { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserToken> UserTokens { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=deliveryDb;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Driver>(entity =>
            {
                entity.HasIndex(e => e.Email, "UQ__Drivers__AB6E61642760FB05")
                    .IsUnique();

                entity.Property(e => e.DriverId).HasColumnName("driverId");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Address)
                    .HasMaxLength(300)
                    .HasColumnName("address");

                entity.Property(e => e.Avatar).HasColumnName("avatar");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createDate");

                entity.Property(e => e.Email)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FullName)
                    .HasMaxLength(200)
                    .HasColumnName("fullName");

                entity.Property(e => e.LastLogin)
                    .HasColumnType("datetime")
                    .HasColumnName("lastLogin");

                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("passwordHash");

                entity.Property(e => e.Phone)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("phone");

                entity.Property(e => e.ReviewRate).HasColumnName("reviewRate");

                entity.Property(e => e.Role)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("role");

                entity.Property(e => e.Salt)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("salt");
            });

            modelBuilder.Entity<DriverDetail>(entity =>
            {
                entity.ToTable("DriverDetail");

                entity.Property(e => e.DriverDetailId).HasColumnName("driverDetailId");

                entity.Property(e => e.Back).HasColumnName("back");

                entity.Property(e => e.DriverId).HasColumnName("driverId");

                entity.Property(e => e.Font).HasColumnName("font");

                entity.Property(e => e.LicenseNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("licenseNumber");

                entity.Property(e => e.VehicleModel)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("vehicleModel");

                entity.HasOne(d => d.Driver)
                    .WithMany(p => p.DriverDetails)
                    .HasForeignKey(d => d.DriverId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DriverDeta__back__29572725");
            });

            modelBuilder.Entity<DriverToken>(entity =>
            {
                entity.ToTable("DriverToken");

                entity.Property(e => e.DriverTokenId).HasColumnName("driverTokenId");

                entity.Property(e => e.AccessToken)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("accessToken");

                entity.Property(e => e.CodeRefreshToken)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("codeRefreshToken");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate");

                entity.Property(e => e.DriverId).HasColumnName("driverId");

                entity.Property(e => e.ExpiredDateAccessToken)
                    .HasColumnType("datetime")
                    .HasColumnName("expiredDateAccessToken");

                entity.Property(e => e.ExpiredRefreshToken)
                    .HasColumnType("datetime")
                    .HasColumnName("expiredRefreshToken");

                entity.Property(e => e.RefreshToken)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("refreshToken");
            });

            modelBuilder.Entity<History>(entity =>
            {
                entity.Property(e => e.HistoryId).HasColumnName("historyId");

                entity.Property(e => e.ChangeDate)
                    .HasColumnType("datetime")
                    .HasColumnName("changeDate");

                entity.Property(e => e.DriverId).HasColumnName("driverId");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("datetime")
                    .HasColumnName("orderDate");

                entity.Property(e => e.OrderId).HasColumnName("orderId");

                entity.Property(e => e.Reason)
                    .HasMaxLength(200)
                    .HasColumnName("reason");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Histories)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Histories__chang__36B12243");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderId).HasColumnName("orderId");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.CompleteAt)
                    .HasColumnType("datetime")
                    .HasColumnName("completeAt");

                entity.Property(e => e.DeliveryNote)
                    .HasMaxLength(200)
                    .HasColumnName("deliveryNote");

                entity.Property(e => e.DriverAcceptAt)
                    .HasColumnType("datetime")
                    .HasColumnName("driverAcceptAt");

                entity.Property(e => e.DriverId).HasColumnName("driverId");

                entity.Property(e => e.EstimatedDeliveryDate)
                    .HasColumnType("datetime")
                    .HasColumnName("estimatedDeliveryDate");

                entity.Property(e => e.FailedDeliveryMoney).HasColumnName("failedDeliveryMoney");

                entity.Property(e => e.HeightPackage).HasColumnName("heightPackage");

                entity.Property(e => e.ImagesPackages).HasColumnName("imagesPackages");

                entity.Property(e => e.Location)
                    .HasMaxLength(200)
                    .HasColumnName("location");

                entity.Property(e => e.LongPackage).HasColumnName("longPackage");

                entity.Property(e => e.PaymentMethod).HasColumnName("paymentMethod");

                entity.Property(e => e.ReceiverAddress)
                    .HasMaxLength(500)
                    .HasColumnName("receiverAddress");

                entity.Property(e => e.ReceiverDistrict)
                    .HasMaxLength(200)
                    .HasColumnName("receiverDistrict");

                entity.Property(e => e.ReceiverName)
                    .HasMaxLength(100)
                    .HasColumnName("receiverName");

                entity.Property(e => e.ReceiverPhone)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("receiverPhone");

                entity.Property(e => e.ReceiverWard)
                    .HasMaxLength(200)
                    .HasColumnName("receiverWard");

                entity.Property(e => e.ShipCost).HasColumnName("shipCost");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.StatusDetail)
                    .HasMaxLength(100)
                    .HasColumnName("statusDetail");

                entity.Property(e => e.TotalCod).HasColumnName("totalCod");

                entity.Property(e => e.TotalGamPackage).HasColumnName("totalGamPackage");

                entity.Property(e => e.TotalMoney).HasColumnName("totalMoney");

                entity.Property(e => e.TotalPriceProduct).HasColumnName("totalPriceProduct");

                entity.Property(e => e.UserAddress)
                    .HasMaxLength(500)
                    .HasColumnName("userAddress");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("userName");

                entity.Property(e => e.UserNote)
                    .HasMaxLength(200)
                    .HasColumnName("userNote");

                entity.Property(e => e.UserPhone)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("userPhone");

                entity.Property(e => e.WidePackage).HasColumnName("widePackage");

                entity.HasOne(d => d.Driver)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.DriverId)
                    .HasConstraintName("FK_Order_Id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Orders__totalCod__2C3393D0");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetail");

                entity.Property(e => e.OrderDetailId).HasColumnName("orderDetailId");

                entity.Property(e => e.Gam).HasColumnName("gam");

                entity.Property(e => e.ImagesProduct).HasColumnName("imagesProduct");

                entity.Property(e => e.OrderId).HasColumnName("orderId");

                entity.Property(e => e.ProductId).HasColumnName("productId");

                entity.Property(e => e.ProductName)
                    .HasMaxLength(200)
                    .HasColumnName("productName");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderDeta__order__33D4B598");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_OrderDetail");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasIndex(e => e.NameProduct, "UQ__Products__3AE90233EE0111D0")
                    .IsUnique();

                entity.Property(e => e.ProductId).HasColumnName("productId");

                entity.Property(e => e.CodeProduct)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("codeProduct");

                entity.Property(e => e.ImagesProduct).HasColumnName("imagesProduct");

                entity.Property(e => e.NameProduct)
                    .HasMaxLength(200)
                    .HasColumnName("nameProduct");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.ProductVolume).HasColumnName("productVolume");
            });

            modelBuilder.Entity<StatusOrder>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("StatusOrder");

                entity.Property(e => e.Status)
                    .HasMaxLength(100)
                    .HasColumnName("status");

                entity.Property(e => e.StatusInt).HasColumnName("statusInt");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email, "UQ__Users__AB6E616421824B57")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Address)
                    .HasMaxLength(300)
                    .HasColumnName("address");

                entity.Property(e => e.Avatar).HasColumnName("avatar");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createDate");

                entity.Property(e => e.Email)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FullName)
                    .HasMaxLength(200)
                    .HasColumnName("fullName");

                entity.Property(e => e.LastLogin)
                    .HasColumnType("datetime")
                    .HasColumnName("lastLogin");

                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("passwordHash");

                entity.Property(e => e.Phone)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("phone");

                entity.Property(e => e.Role)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("role");

                entity.Property(e => e.Salt)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("salt");
            });

            modelBuilder.Entity<UserToken>(entity =>
            {
                entity.Property(e => e.UserTokenId).HasColumnName("userTokenId");

                entity.Property(e => e.AccessToken)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("accessToken");

                entity.Property(e => e.CodeRefreshToken)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("codeRefreshToken");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate");

                entity.Property(e => e.ExpiredDateAccessToken)
                    .HasColumnType("datetime")
                    .HasColumnName("expiredDateAccessToken");

                entity.Property(e => e.ExpiredRefreshToken)
                    .HasColumnType("datetime")
                    .HasColumnName("expiredRefreshToken");

                entity.Property(e => e.RefreshToken)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("refreshToken");

                entity.Property(e => e.UserId).HasColumnName("userId");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
