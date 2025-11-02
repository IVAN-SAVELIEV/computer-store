using ComputerStoreClean.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerStoreClean.Domain.Common
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsWithCategoryAndSpecsAsync();
        Task<Product> GetProductWithDetailsAsync(int id);
        Task<IEnumerable<Product>> GetProductsByCategoryWithDetailsAsync(int categoryId);
        Task<IEnumerable<Product>> SearchProductsWithDetailsAsync(string searchTerm);
    }
}
