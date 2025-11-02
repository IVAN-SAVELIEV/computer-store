using AutoMapper;
using ComputerStoreClean.Application.DTOs;
using ComputerStoreClean.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerStoreClean.Application.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Product mappings
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Specifications, opt => opt.MapFrom(src => src.Specifications));

            CreateMap<ProductSpecification, ProductSpecificationDto>();

            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.Specifications, opt => opt.Ignore());

            // Order mappings
            CreateMap<Order, OrderDto>();

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));

            // Category mappings
            CreateMap<Category, CategoryDto>();
            CreateMap<CreateCategoryDto, Category>();
        }
    }
}
