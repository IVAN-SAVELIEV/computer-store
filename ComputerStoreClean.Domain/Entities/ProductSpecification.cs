using ComputerStoreClean.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerStoreClean.Domain.Entities
{
    public class ProductSpecification : BaseEntity
    {
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;

        // Foreign key
        public int ProductId { get; set; }

        // Navigation property
        public Product Product { get; set; } = null!;
    }
}
