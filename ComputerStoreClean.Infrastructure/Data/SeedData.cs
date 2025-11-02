using ComputerStoreClean.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerStoreClean.Infrastructure.Data
{
    public static class SeedData
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            // Проверяем, есть ли уже данные
            if (await context.Categories.AnyAsync())
            {
                return; // База уже заполнена
            }

            // 1. Создаем категории
            var categories = new[]
            {
            new Category { Name = "Процессоры", Description = "Центральные процессоры для настольных ПК и серверов" },
            new Category { Name = "Видеокарты", Description = "Графические карты для игр и профессиональной работы" },
            new Category { Name = "Материнские платы", Description = "Основные платы для сборки ПК" },
            new Category { Name = "Оперативная память", Description = "Модули оперативной памяти" },
            new Category { Name = "Накопители", Description = "SSD и HDD накопители" },
            new Category { Name = "Блоки питания", Description = "Источники питания для ПК" }
        };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            // 2. Создаем продукты
            var products = new[]
            {
            // Процессоры
            new Product
            {
                Name = "AMD Ryzen 7 5800X",
                Description = "8-ядерный процессор для игр и творчества",
                Price = 29999,
                StockQuantity = 15,
                Brand = "AMD",
                Model = "Ryzen 7 5800X",
                ImageUrl = "https://example.com/images/ryzen7-5800x.jpg",
                CategoryId = categories[0].Id,
                Specifications = new List<ProductSpecification>
                {
                    new() { Key = "Количество ядер", Value = "8" },
                    new() { Key = "Тактовая частота", Value = "3.8 GHz" },
                    new() { Key = "Сокет", Value = "AM4" }
                }
            },
            new Product
            {
                Name = "Intel Core i5-12400F",
                Description = "6-ядерный процессор для игр",
                Price = 18999,
                StockQuantity = 20,
                Brand = "Intel",
                Model = "Core i5-12400F",
                ImageUrl = "https://example.com/images/i5-12400f.jpg",
                CategoryId = categories[0].Id,
                Specifications = new List<ProductSpecification>
                {
                    new() { Key = "Количество ядер", Value = "6" },
                    new() { Key = "Тактовая частота", Value = "2.5 GHz" },
                    new() { Key = "Сокет", Value = "LGA 1700" }
                }
            },

            // Видеокарты
            new Product
            {
                Name = "NVIDIA GeForce RTX 4070",
                Description = "Видеокарта для игр в 4K",
                Price = 69999,
                StockQuantity = 8,
                Brand = "NVIDIA",
                Model = "GeForce RTX 4070",
                ImageUrl = "https://example.com/images/rtx4070.jpg",
                CategoryId = categories[1].Id,
                Specifications = new List<ProductSpecification>
                {
                    new() { Key = "Объем памяти", Value = "12 GB" },
                    new() { Key = "Тип памяти", Value = "GDDR6X" },
                    new() { Key = "Потребление", Value = "200W" }
                }
            },
            new Product
            {
                Name = "AMD Radeon RX 7700 XT",
                Description = "Игровая видеокарта среднего класса",
                Price = 45999,
                StockQuantity = 12,
                Brand = "AMD",
                Model = "Radeon RX 7700 XT",
                ImageUrl = "https://example.com/images/rx7700xt.jpg",
                CategoryId = categories[1].Id,
                Specifications = new List<ProductSpecification>
                {
                    new() { Key = "Объем памяти", Value = "12 GB" },
                    new() { Key = "Тип памяти", Value = "GDDR6" },
                    new() { Key = "Потребление", Value = "245W" }
                }
            },

            // Материнские платы
            new Product
            {
                Name = "ASUS ROG Strix B550-F Gaming",
                Description = "Игровая материнская плата для AMD",
                Price = 15999,
                StockQuantity = 10,
                Brand = "ASUS",
                Model = "ROG Strix B550-F Gaming",
                ImageUrl = "https://example.com/images/b550-f.jpg",
                CategoryId = categories[2].Id,
                Specifications = new List<ProductSpecification>
                {
                    new() { Key = "Сокет", Value = "AM4" },
                    new() { Key = "Чипсет", Value = "B550" },
                    new() { Key = "Форм-фактор", Value = "ATX" }
                }
            },

            // Оперативная память
            new Product
            {
                Name = "Corsair Vengeance RGB 32GB",
                Description = "Игровая оперативная память с подсветкой",
                Price = 8999,
                StockQuantity = 25,
                Brand = "Corsair",
                Model = "Vengeance RGB",
                ImageUrl = "https://example.com/images/vengeance-rgb.jpg",
                CategoryId = categories[3].Id,
                Specifications = new List<ProductSpecification>
                {
                    new() { Key = "Объем", Value = "32 GB" },
                    new() { Key = "Частота", Value = "3600 MHz" },
                    new() { Key = "Тайминги", Value = "CL18" }
                }
            },

            // SSD накопители
            new Product
            {
                Name = "Samsung 980 Pro 1TB",
                Description = "Высокоскоростной SSD NVMe",
                Price = 12999,
                StockQuantity = 18,
                Brand = "Samsung",
                Model = "980 Pro",
                ImageUrl = "https://example.com/images/980-pro.jpg",
                CategoryId = categories[4].Id,
                Specifications = new List<ProductSpecification>
                {
                    new() { Key = "Объем", Value = "1 TB" },
                    new() { Key = "Интерфейс", Value = "NVMe PCIe 4.0" },
                    new() { Key = "Скорость чтения", Value = "7000 MB/s" }
                }
            },

            // Блоки питания
            new Product
            {
                Name = "Seasonic Focus GX-750",
                Description = "Модульный блок питания 80+ Gold",
                Price = 11999,
                StockQuantity = 14,
                Brand = "Seasonic",
                Model = "Focus GX-750",
                ImageUrl = "https://example.com/images/focus-gx750.jpg",
                CategoryId = categories[5].Id,
                Specifications = new List<ProductSpecification>
                {
                    new() { Key = "Мощность", Value = "750W" },
                    new() { Key = "Сертификат", Value = "80+ Gold" },
                    new() { Key = "Модульность", Value = "Полная" }
                }
            }
        };

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();

        }
    }
}
