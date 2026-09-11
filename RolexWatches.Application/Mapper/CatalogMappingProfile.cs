using AutoMapper;
using RolexWatches.Application.DTOs.Brand;
using RolexWatches.Application.DTOs.Category;
using RolexWatches.Application.DTOs.Product;
using RolexWatches.Domain.Entities;

namespace RolexWatches.Application.Mapping
{
    public class CatalogMappingProfile : Profile
    {
        public CatalogMappingProfile()
        {
            // ----- Product -----
            CreateMap<Product, ProductDto>()
                .ForMember(d => d.BrandName, o => o.MapFrom(s => s.Brand != null ? s.Brand.Name : string.Empty))
                .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category != null ? s.Category.Name : string.Empty))
                .ForMember(d => d.EffectivePrice, o => o.MapFrom(s => s.EffectivePrice))
                .ForMember(d => d.IsLowStock, o => o.MapFrom(s => s.IsLowStock))
                .ForMember(d => d.IsInStock, o => o.MapFrom(s => s.IsInStock));

            CreateMap<ProductImage, ProductImageDto>();
            CreateMap<ProductSpecification, ProductSpecificationDto>();

            CreateMap<CreateProductDto, Product>()
                .ForMember(d => d.Images, o => o.Ignore())
                .ForMember(d => d.Specifications, o => o.Ignore());

            CreateMap<UpdateProductDto, Product>()
                .ForMember(d => d.Images, o => o.Ignore())
                .ForMember(d => d.Specifications, o => o.Ignore());

            CreateMap<CreateProductImageDto, ProductImage>();
            CreateMap<CreateProductSpecificationDto, ProductSpecification>();

            // ----- Brand -----
            CreateMap<Brand, BrandDto>()
                .ForMember(d => d.ProductCount, o => o.MapFrom(s => s.Products.Count));
            CreateMap<CreateUpdateBrandDto, Brand>();

            // ----- Category -----
            CreateMap<Category, CategoryDto>()
                .ForMember(d => d.ParentCategoryName, o => o.MapFrom(s => s.ParentCategory != null ? s.ParentCategory.Name : null))
                .ForMember(d => d.ProductCount, o => o.MapFrom(s => s.Products.Count))
                .ForMember(d => d.SubCategories, o => o.MapFrom(s => s.SubCategories));
            CreateMap<CreateUpdateCategoryDto, Category>();
        }
    }
}