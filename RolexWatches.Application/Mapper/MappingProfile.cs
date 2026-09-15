using AutoMapper;
using RolexWatches.Application.Dto;
using RolexWatches.Domain.Entities;

namespace RolexWatches.Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // =====================================================
            // PRODUCT
            // =====================================================

            CreateMap<Product, ProductDto>()
                .ForMember(
                    d => d.BrandName,
                    opt => opt.MapFrom(s =>
                        s.Brand != null ? s.Brand.Name : string.Empty))
                .ForMember(
                    d => d.CategoryName,
                    opt => opt.MapFrom(s =>
                        s.Category != null ? s.Category.Name : string.Empty));

            CreateMap<CreateProductDto, Product>();

            CreateMap<UpdateProductDto, Product>();


            // =====================================================
            // BRAND
            // =====================================================

            CreateMap<Brand, BrandDto>()
                .ReverseMap();

            CreateMap<CreateBrandDto, Brand>();


            // =====================================================
            // CATEGORY
            // =====================================================

            CreateMap<Category, CategoryDto>()
                .ReverseMap();

            CreateMap<CreateCategoryDto, Category>();


            // =====================================================
            // ORDER
            // =====================================================

            CreateMap<Order, OrderDto>()
                .ForMember(
                    d => d.CustomerName,
                    opt => opt.MapFrom(s =>
                        s.User != null
                            ? s.User.FullName
                            : string.Empty))
                .ForMember(
                    d => d.Items,
                    opt => opt.MapFrom(s => s.OrderItems));


            // =====================================================
            // ORDER ITEM
            // =====================================================

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(
                    d => d.ProductName,
                    opt => opt.MapFrom(s =>
                        s.Product != null
                            ? s.Product.Name
                            : string.Empty));


            // =====================================================
            // USER / CUSTOMER
            // =====================================================

            CreateMap<User, CustomerDto>();


            // =====================================================
            // CART ITEM
            // =====================================================

            CreateMap<CartItem, CartItemDto>()
                .ForMember(
                    d => d.ProductName,
                    opt => opt.MapFrom(s =>
                        s.Product != null
                            ? s.Product.Name
                            : string.Empty))
                .ForMember(
                    d => d.Price,
                    opt => opt.MapFrom(s =>
                        s.Product != null
                            ? s.Product.Price
                            : 0));


            // =====================================================
            // WISHLIST ITEM
            // =====================================================

            CreateMap<WishlistItem, WishlistItemDto>()
                .ForMember(
                    d => d.ProductName,
                    opt => opt.MapFrom(s =>
                        s.Product != null
                            ? s.Product.Name
                            : string.Empty))
                .ForMember(
                    d => d.Price,
                    opt => opt.MapFrom(s =>
                        s.Product != null
                            ? s.Product.Price
                            : 0));


            // =====================================================
            // REVIEW
            // =====================================================

            CreateMap<Review, ReviewDto>()
                .ForMember(
                    d => d.ProductName,
                    opt => opt.MapFrom(s =>
                        s.Product != null
                            ? s.Product.Name
                            : string.Empty))
                .ForMember(
                    d => d.CustomerName,
                    opt => opt.MapFrom(s =>
                        s.User != null
                            ? s.User.FullName
                            : string.Empty));
        }
    }
}