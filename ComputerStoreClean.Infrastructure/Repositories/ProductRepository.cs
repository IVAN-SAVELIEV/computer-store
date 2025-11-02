using ComputerStoreClean.Domain.Common;
using ComputerStoreClean.Domain.Entities;
using ComputerStoreClean.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerStoreClean.Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetProductsWithCategoryAndSpecsAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Specifications)
                .ToListAsync();
        }

        public async Task<Product> GetProductWithDetailsAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Specifications)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryWithDetailsAsync(int categoryId)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Specifications)
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> SearchProductsWithDetailsAsync(string searchTerm)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Specifications)
                .Where(p => p.Name.Contains(searchTerm) ||
                           p.Description.Contains(searchTerm) ||
                           p.Brand.Contains(searchTerm) ||
                           p.Model.Contains(searchTerm))
                .ToListAsync();
        }
    }
}
