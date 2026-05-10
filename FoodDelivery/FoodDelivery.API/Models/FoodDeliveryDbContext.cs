using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.API.Models;

public partial class FoodDeliveryDbContext : DbContext
{
    public FoodDeliveryDbContext()
    {
    }

    public FoodDeliveryDbContext(DbContextOptions<FoodDeliveryDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Coupon> Coupons { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<DeliveryAddress> DeliveryAddresses { get; set; }

    public virtual DbSet<DeliveryDriver> DeliveryDrivers { get; set; }

    public virtual DbSet<MenuItem> MenuItems { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Restaurant> Restaurants { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=foodservice;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Coupon>(entity =>
        {
            entity.HasKey(e => e.CouponId).HasName("PK__Coupons__58CF638927335FC9");

            entity.HasIndex(e => e.CouponCode, "UQ__Coupons__ADE5CBB7FF3234C9").IsUnique();

            entity.Property(e => e.CouponId)
                .ValueGeneratedNever()
                .HasColumnName("coupon_id");
            entity.Property(e => e.CouponCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("coupon_code");
            entity.Property(e => e.DiscountAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("discount_amount");
            entity.Property(e => e.ExpiryDate).HasColumnName("expiry_date");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__CD65CB85B6B0F5E1");

            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.CustomerEmail)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("customer_email");
            entity.Property(e => e.CustomerHashedPassword)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("customer_hashed_password");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("customer_name");
            entity.Property(e => e.CustomerPhone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("customer_phone");
            entity.Property(e => e.CustomerUnhashedPassword)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("customer_unhashed_password");
            entity.Property(e => e.RoleId)
                .HasDefaultValue(2)
                .HasColumnName("role_id");

            entity.HasOne(d => d.Role).WithMany(p => p.Customers)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Customers_Roles");
        });

        modelBuilder.Entity<DeliveryAddress>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__Delivery__CAA247C8AAB4D02E");

            entity.Property(e => e.AddressId)
                .ValueGeneratedNever()
                .HasColumnName("address_id");
            entity.Property(e => e.AddressLine1)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("address_line1");
            entity.Property(e => e.AddressLine2)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("address_line2");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("city");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.PostalCode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("postal_code");
            entity.Property(e => e.State)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("state");

            entity.HasOne(d => d.Customer).WithMany(p => p.DeliveryAddresses)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__DeliveryA__custo__5AEE82B9");
        });

        modelBuilder.Entity<DeliveryDriver>(entity =>
        {
            entity.HasKey(e => e.DriverId).HasName("PK__Delivery__A411C5BD833CC034");

            entity.Property(e => e.DriverId).HasColumnName("driver_id");
            entity.Property(e => e.DriverEmail)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("driver_email");
            entity.Property(e => e.DriverName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("driver_name");
            entity.Property(e => e.DriverPhone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("driver_phone");
            entity.Property(e => e.DriverUnhashedPassword)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("driver_unhashed_password");
            entity.Property(e => e.DriverVehicle)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("driver_vehicle");
        });

        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__MenuItem__52020FDD0847168E");

            entity.Property(e => e.ItemId)
                .ValueGeneratedNever()
                .HasColumnName("item_id");
            entity.Property(e => e.ItemDescription)
                .HasColumnType("text")
                .HasColumnName("item_description");
            entity.Property(e => e.ItemName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("item_name");
            entity.Property(e => e.ItemPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("item_price");
            entity.Property(e => e.RestaurantId).HasColumnName("restaurant_id");

            entity.HasOne(d => d.Restaurant).WithMany(p => p.MenuItems)
                .HasForeignKey(d => d.RestaurantId)
                .HasConstraintName("FK__MenuItems__resta__4D94879B");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__46596229A40EB1F9");

            entity.Property(e => e.OrderId)
                .ValueGeneratedNever()
                .HasColumnName("order_id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.DeliveryDriverId).HasColumnName("delivery_driver_id");
            entity.Property(e => e.OrderDate)
                .HasColumnType("datetime")
                .HasColumnName("order_date");
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("order_status");
            entity.Property(e => e.RestaurantId).HasColumnName("restaurant_id");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Orders__customer__52593CB8");

            entity.HasOne(d => d.DeliveryDriver).WithMany(p => p.Orders)
                .HasForeignKey(d => d.DeliveryDriverId)
                .HasConstraintName("FK__Orders__delivery__5441852A");

            entity.HasOne(d => d.Restaurant).WithMany(p => p.Orders)
                .HasForeignKey(d => d.RestaurantId)
                .HasConstraintName("FK__Orders__restaura__534D60F1");

            entity.HasMany(d => d.Coupons).WithMany(p => p.Orders)
                .UsingEntity<Dictionary<string, object>>(
                    "OrdersCoupon",
                    r => r.HasOne<Coupon>().WithMany()
                        .HasForeignKey("CouponId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__OrdersCou__coupo__619B8048"),
                    l => l.HasOne<Order>().WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__OrdersCou__order__60A75C0F"),
                    j =>
                    {
                        j.HasKey("OrderId", "CouponId").HasName("PK__OrdersCo__C3D5941129BC13AC");
                        j.ToTable("OrdersCoupons");
                        j.IndexerProperty<int>("OrderId").HasColumnName("order_id");
                        j.IndexerProperty<int>("CouponId").HasColumnName("coupon_id");
                    });
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("PK__OrderIte__3764B6BCFA060F32");

            entity.Property(e => e.OrderItemId)
                .ValueGeneratedNever()
                .HasColumnName("order_item_id");
            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Item).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__OrderItem__item___5812160E");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderItem__order__571DF1D5");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.RatingId).HasName("PK__Ratings__D35B278B66275B27");

            entity.Property(e => e.RatingId)
                .ValueGeneratedNever()
                .HasColumnName("rating_id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Rating1).HasColumnName("rating");
            entity.Property(e => e.RestaurantId).HasColumnName("restaurant_id");
            entity.Property(e => e.Review)
                .HasColumnType("text")
                .HasColumnName("review");

            entity.HasOne(d => d.Order).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__Ratings__order_i__6477ECF3");

            entity.HasOne(d => d.Restaurant).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.RestaurantId)
                .HasConstraintName("FK__Ratings__restaur__656C112C");
        });

        modelBuilder.Entity<Restaurant>(entity =>
        {
            entity.HasKey(e => e.RestaurantId).HasName("PK__Restaura__3B0FAA9102864D8D");

            entity.Property(e => e.RestaurantId).HasColumnName("restaurant_id");
            entity.Property(e => e.RestaurantAddress)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("restaurant_address");
            entity.Property(e => e.RestaurantEmail)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("restaurant_email");
            entity.Property(e => e.RestaurantName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("restaurant_name");
            entity.Property(e => e.RestaurantPhone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("restaurant_phone");
            entity.Property(e => e.RestaurantUnhashedPassword)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("restaurant_unhashed_password");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__760965CC47B2F7E4");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__783254B15CE24888").IsUnique();

            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("role_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
