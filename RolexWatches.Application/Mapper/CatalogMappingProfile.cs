using AutoMapper;
using RolexWatches.Application.Dto;
using RolexWatches.Domain.Entities;

namespace RolexWatches.Application.Mapper
{
    public class CatalogMappingProfile : Profile
    {
        public CatalogMappingProfile()
        {
            // ---------- Product ----------
            CreateMap<Product, ProductDto>()
                .ForMember(d => d.BrandName, o => o.MapFrom(s => s.Brand != null ? s.Brand.Name : string.Empty))
                .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category != null ? s.Category.Name : string.Empty))
                .ForMember(d => d.EffectivePrice, o => o.MapFrom(s => s.EffectivePrice))
                .ForMember(d => d.IsLowStock, o => o.MapFrom(s => s.IsLowStock))
                .ForMember(d => d.IsInStock, o => o.MapFrom(s => s.IsInStock));

            CreateMap<ProductImage, ProductImageDto>();
            CreateMap<ProductSpecification, ProductSpecificationDto>();

            // Entity creation from Create/Update DTOs — Images/Specifications are
            // handled manually in ProductService (not by AutoMapper), since the
            // incoming list items are a different shape (CreateProductImageDto,
            // not ProductImage) and need explicit conversion.
            CreateMap<CreateProductDto, Product>()
                .ForMember(d => d.Images, o => o.Ignore())
                .ForMember(d => d.Specifications, o => o.Ignore());

            CreateMap<UpdateProductDto, Product>()
                .ForMember(d => d.Images, o => o.Ignore())
                .ForMember(d => d.Specifications, o => o.Ignore());

            // ---------- Brand ----------
            CreateMap<Brand, BrandDto>()
                .ForMember(d => d.ProductCount, o => o.MapFrom(s => s.Products.Count));
            CreateMap<CreateBrandDto, Brand>();

            // ---------- Category ----------
            CreateMap<Category, CategoryDto>()
                .ForMember(d => d.ParentCategoryName, o => o.MapFrom(s => s.ParentCategory != null ? s.ParentCategory.Name : null))
                .ForMember(d => d.ProductCount, o => o.MapFrom(s => s.Products.Count));
            CreateMap<CreateCategoryDto, Category>();
        }
    }
}
