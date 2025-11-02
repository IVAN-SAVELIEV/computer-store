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
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public CategoryService(
            ICategoryRepository categoryRepository,
            IProductRepository productRepository,
            IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var categoryDtos = _mapper.Map<List<CategoryDto>>(categories);

            // Добавляем количество продуктов для каждой категории
            foreach (var categoryDto in categoryDtos)
            {
                categoryDto.ProductCount = await _categoryRepository.GetProductCountAsync(categoryDto.Id);
            }

            return categoryDtos;
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {id} not found.");

            var categoryDto = _mapper.Map<CategoryDto>(category);
            categoryDto.ProductCount = await _categoryRepository.GetProductCountAsync(id);

            return categoryDto;
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            var category = new Category
            {
                Name = createCategoryDto.Name,
                Description = createCategoryDto.Description,
                CreatedAt = DateTime.UtcNow
            };

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangesAsync();

            return await GetCategoryByIdAsync(category.Id);
        }

        public async Task UpdateCategoryAsync(int id, CreateCategoryDto updateCategoryDto)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {id} not found.");

            category.Name = updateCategoryDto.Name;
            category.Description = updateCategoryDto.Description;
            category.UpdatedAt = DateTime.UtcNow;

            _categoryRepository.Update(category);
            await _categoryRepository.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {id} not found.");

            // Проверяем, есть ли продукты в этой категории
            var productCount = await _categoryRepository.GetProductCountAsync(id);
            if (productCount > 0)
                throw new InvalidOperationException($"Cannot delete category with ID {id} because it has {productCount} associated products.");

            _categoryRepository.Delete(category);
            await _categoryRepository.SaveChangesAsync();
        }
    }
}
