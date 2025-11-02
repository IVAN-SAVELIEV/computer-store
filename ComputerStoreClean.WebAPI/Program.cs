using ComputerStoreClean.Application.Common;
using ComputerStoreClean.Infrastructure;
using ComputerStoreClean.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

// Add Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Computer Store API",
        Version = "v1",
        Description = "API for Computer Store management system"
    });
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactApp", policy =>
    {
        policy.WithOrigins("https://localhost:7258", "http://localhost:30132", "http://localhost:5170") // React dev server
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Initialize database with sample data
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Apply migrations
        await context.Database.MigrateAsync();

        // Add sample products if database is empty
        // await SeedSampleData(context);
        await SeedData.SeedAsync(context);
    }
}

app.UseCors("ReactApp");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Method to seed sample data
//async Task SeedSampleData(ApplicationDbContext context)
//{
//    if (!await context.Products.AnyAsync())
//    {
//        var products = new List<ComputerStoreClean.Domain.Entities.Product>
//        {
//            new()
//            {
//                Name = "Intel Core i9-13900K",
//                Description = "Процессор Intel Core i9 13-го поколения",
//                Price = 599.99m,
//                StockQuantity = 10,
//                Brand = "Intel",
//                Model = "Core i9-13900K",
//                ImageUrl = "/images/intel-i9.jpg",
//                CategoryId = 1,
//                CreatedAt = DateTime.UtcNow,
//                Specifications = new List<ComputerStoreClean.Domain.Entities.ProductSpecification>
//                {
//                    new() { Key = "Ядер", Value = "24" },
//                    new() { Key = "Потоков", Value = "32" },
//                    new() { Key = "Базовая частота", Value = "3.0 GHz" },
//                    new() { Key = "Сокет", Value = "LGA 1700" }
//                }
//            },
//            new()
//            {
//                Name = "NVIDIA GeForce RTX 4080",
//                Description = "Видеокарта NVIDIA GeForce RTX 4080",
//                Price = 1199.99m,
//                StockQuantity = 5,
//                Brand = "NVIDIA",
//                Model = "RTX 4080",
//                ImageUrl = "/images/rtx4080.jpg",
//                CategoryId = 2,
//                CreatedAt = DateTime.UtcNow,
//                Specifications = new List<ComputerStoreClean.Domain.Entities.ProductSpecification>
//                {
//                    new() { Key = "VRAM", Value = "16 GB GDDR6X" },
//                    new() { Key = "Тактовая частота", Value = "2.2 GHz" },
//                    new() { Key = "Память", Value = "256-bit" }
//                }
//            },
//            new()
//            {
//                Name = "Kingston Fury Beast 32GB DDR5",
//                Description = "Оперативная память Kingston Fury Beast DDR5",
//                Price = 199.99m,
//                StockQuantity = 20,
//                Brand = "Kingston",
//                Model = "KF552C40BBK2-32",
//                ImageUrl = "/images/kingston-ram.jpg",
//                CategoryId = 3,
//                CreatedAt = DateTime.UtcNow,
//                Specifications = new List<ComputerStoreClean.Domain.Entities.ProductSpecification>
//                {
//                    new() { Key = "Объем", Value = "32 GB" },
//                    new() { Key = "Тип", Value = "DDR5" },
//                    new() { Key = "Частота", Value = "5200 MHz" }
//                }
//            }
//        };

//        await context.Products.AddRangeAsync(products);
//        await context.SaveChangesAsync();
//    }
//}