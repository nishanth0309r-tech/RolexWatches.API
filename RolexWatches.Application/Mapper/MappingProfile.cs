using AutoMapper;
using RolexWatches.Application.Dto;
using RolexWatches.Domain.Entities;

namespace RolexWatches.Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CartItem, CartItemDto>()
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product!.Name))
                .ForMember(d => d.ImageUrl, o => o.MapFrom(s => s.Product!.ImageUrl))
                .ForMember(d => d.Price, o => o.MapFrom(s => s.Product!.Price));

            CreateMap<WishlistItem, WishlistItemDto>()
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product!.Name))
                .ForMember(d => d.ImageUrl, o => o.MapFrom(s => s.Product!.ImageUrl))
                .ForMember(d => d.Price, o => o.MapFrom(s => s.Product!.Price));
        }
    }
}
