using AutoMapper;
using RolexWatches.Application.Dto;
using RolexWatches.Domain.Entities;
using RolexWatches.Application.Dto;

namespace RolexWatches.Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(d => d.BrandName, opt => opt.MapFrom(s => s.Brand != null ? s.Brand.Name : string.Empty))
                .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category != null ? s.Category.Name : string.Empty));
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();

            CreateMap<Brand, BrandDto>().ReverseMap();
            CreateMap<CreateBrandDto, Brand>();

            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<CreateCategoryDto, Category>();

            CreateMap<Order, OrderDto>()
                .ForMember(d => d.CustomerName, opt => opt.MapFrom(s => s.User != null ? s.User.FullName : string.Empty))
                .ForMember(d => d.Items, opt => opt.MapFrom(s => s.OrderItems));
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductName, opt => opt.MapFrom(s => s.Product != null ? s.Product.Name : string.Empty));

            CreateMap<User, CustomerDto>();
            CreateMap<CartItem, CartItemDto>()
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product!.Name))
                .ForMember(d => d.ImageUrl, o => o.MapFrom(s => s.Product!.ImageUrl))
                .ForMember(d => d.Price, o => o.MapFrom(s => s.Product!.Price));

            CreateMap<WishlistItem, WishlistItemDto>()
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product!.Name))
                .ForMember(d => d.ImageUrl, o => o.MapFrom(s => s.Product!.ImageUrl))
                .ForMember(d => d.Price, o => o.MapFrom(s => s.Product!.Price));
            CreateMap<Review, ReviewDto>()
                .ForMember(d => d.ProductName, opt => opt.MapFrom(s => s.Product != null ? s.Product.Name : string.Empty))
                .ForMember(d => d.CustomerName, opt => opt.MapFrom(s => s.User != null ? s.User.FullName : string.Empty));
        }
    }
}
