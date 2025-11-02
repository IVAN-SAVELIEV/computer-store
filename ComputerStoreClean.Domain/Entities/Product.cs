using ComputerStoreClean.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerStoreClean.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;

        // Foreign key
        public int CategoryId { get; set; }

        // Navigation properties
        public Category Category { get; set; } = null!;
        public ICollection<ProductSpecification> Specifications { get; set; } = new List<ProductSpecification>();
    }
}
