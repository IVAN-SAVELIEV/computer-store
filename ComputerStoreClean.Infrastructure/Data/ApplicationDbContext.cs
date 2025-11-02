using ComputerStoreClean.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerStoreClean.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Конструктор для runtime (используется приложением)
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Конструктор для design-time (используется миграциями EF Core)
        public ApplicationDbContext()
        {
        }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<ProductSpecification> ProductSpecifications => Set<ProductSpecification>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Этот метод вызывается только если options не были предоставлены через конструктор
            if (!optionsBuilder.IsConfigured)
            {
                // Используется только для миграций
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ComputerStoreDb;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Product configuration
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(p => p.Description)
                    .HasMaxLength(1000);

                entity.Property(p => p.Price)
                    .HasColumnType("decimal(18,2)");

                entity.Property(p => p.Brand)
                    .HasMaxLength(100);

                entity.Property(p => p.Model)
                    .HasMaxLength(100);

                entity.Property(p => p.ImageUrl)
                    .HasMaxLength(500);

                // Relationship with Category
                entity.HasOne(p => p.Category)
                      .WithMany(c => c.Products)
                      .HasForeignKey(p => p.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Relationship with ProductSpecification
                entity.HasMany(p => p.Specifications)
                      .WithOne(ps => ps.Product)
                      .HasForeignKey(ps => ps.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Category configuration
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(c => c.Description)
                    .HasMaxLength(500);
            });

            // ProductSpecification configuration
            modelBuilder.Entity<ProductSpecification>(entity =>
            {
                entity.HasKey(ps => ps.Id);

                entity.Property(ps => ps.Key)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(ps => ps.Value)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            // Order configuration
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.Id);

                entity.Property(o => o.OrderNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(o => o.TotalAmount)
                    .HasColumnType("decimal(18,2)");

                entity.Property(o => o.CustomerName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(o => o.CustomerEmail)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(o => o.CustomerPhone)
                    .HasMaxLength(20);

                entity.Property(o => o.ShippingAddress)
                    .IsRequired();

                // Relationship with OrderItems
                entity.HasMany(o => o.OrderItems)
                      .WithOne(oi => oi.Order)
                      .HasForeignKey(oi => oi.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // OrderItem configuration
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(oi => oi.Id);

                entity.Property(oi => oi.UnitPrice)
                    .HasColumnType("decimal(18,2)");

                // Relationship with Product
                entity.HasOne(oi => oi.Product)
                      .WithMany()
                      .HasForeignKey(oi => oi.ProductId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Seed data for Categories
            //modelBuilder.Entity<Category>().HasData(
            //    new Category
            //    {
            //        Id = 1,
            //        Name = "Процессоры",
            //        Description = "Центральные процессоры для настольных компьютеров и серверов",
            //        CreatedAt = DateTime.UtcNow
            //    },
            //    new Category
            //    {
            //        Id = 2,
            //        Name = "Видеокарты",
            //        Description = "Графические процессоры для игр и профессиональной работы",
            //        CreatedAt = DateTime.UtcNow
            //    },
            //    new Category
            //    {
            //        Id = 3,
            //        Name = "Оперативная память",
            //        Description = "Модули оперативной памяти DDR4 и DDR5",
            //        CreatedAt = DateTime.UtcNow
            //    },
            //    new Category
            //    {
            //        Id = 4,
            //        Name = "Материнские платы",
            //        Description = "Системные платы для сборки компьютеров",
            //        CreatedAt = DateTime.UtcNow
            //    },
            //    new Category
            //    {
            //        Id = 5,
            //        Name = "Накопители",
            //        Description = "SSD и HDD накопители для хранения данных",
            //        CreatedAt = DateTime.UtcNow
            //    },
            //    new Category
            //    {
            //        Id = 6,
            //        Name = "Блоки питания",
            //        Description = "Источники питания для компьютерных систем",
            //        CreatedAt = DateTime.UtcNow
            //    }
            //);
        }
    }
}
