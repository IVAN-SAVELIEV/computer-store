using AutoMapper;
using ComputerStoreClean.Application.DTOs;
using ComputerStoreClean.Application.Interfaces;
using ComputerStoreClean.Domain.Common;
using ComputerStoreClean.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerStoreClean.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public ProductService(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetProductsWithCategoryAndSpecsAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetProductWithDetailsAsync(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found.");

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId)
        {
            var products = await _productRepository.GetProductsByCategoryWithDetailsAsync(categoryId);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<IEnumerable<ProductDto>> SearchProductsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllProductsAsync();

            var products = await _productRepository.SearchProductsWithDetailsAsync(searchTerm);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
        {
            // Проверяем существование категории
            var category = await _categoryRepository.GetByIdAsync(createProductDto.CategoryId);
            if (category == null)
                throw new ArgumentException($"Category with ID {createProductDto.CategoryId} not found.");

            var product = new Product
            {
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                Price = createProductDto.Price,
                StockQuantity = createProductDto.StockQuantity,
                Brand = createProductDto.Brand,
                Model = createProductDto.Model,
                ImageUrl = createProductDto.ImageUrl,
                CategoryId = createProductDto.CategoryId,
                CreatedAt = DateTime.UtcNow
            };

            // Добавляем спецификации
            foreach (var specDto in createProductDto.Specifications)
            {
                product.Specifications.Add(new ProductSpecification
                {
                    Key = specDto.Key,
                    Value = specDto.Value
                });
            }

            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();

            return await GetProductByIdAsync(product.Id);
        }

        public async Task UpdateProductAsync(int id, CreateProductDto updateProductDto)
        {
            var product = await _productRepository.GetProductWithDetailsAsync(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found.");

            // Проверяем существование категории
            var category = await _categoryRepository.GetByIdAsync(updateProductDto.CategoryId);
            if (category == null)
                throw new ArgumentException($"Category with ID {updateProductDto.CategoryId} not found.");

            product.Name = updateProductDto.Name;
            product.Description = updateProductDto.Description;
            product.Price = updateProductDto.Price;
            product.StockQuantity = updateProductDto.StockQuantity;
            product.Brand = updateProductDto.Brand;
            product.Model = updateProductDto.Model;
            product.ImageUrl = updateProductDto.ImageUrl;
            product.CategoryId = updateProductDto.CategoryId;
            product.UpdatedAt = DateTime.UtcNow;

            // Обновляем спецификации
            product.Specifications.Clear();
            foreach (var specDto in updateProductDto.Specifications)
            {
                product.Specifications.Add(new ProductSpecification
                {
                    Key = specDto.Key,
                    Value = specDto.Value
                });
            }

            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found.");

            _productRepository.Delete(product);
            await _productRepository.SaveChangesAsync();
        }
    }
}
