using AutoMapper;
using RolexWatches.Application.Dto;
using RolexWatches.Application.ServiceInterface;
using RolexWatches.Domain.Entities;
using RolexWatches.Domain.Interfaces;

namespace RolexWatches.Application.Service
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _repo;
        private readonly IMapper _mapper;

        public CartService(ICartRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<CartItemDto>> GetCartAsync(string userId)
        {
            var items = await _repo.GetByUserIdAsync(userId);
            return _mapper.Map<List<CartItemDto>>(items);
        }

        public async Task<CartItemDto> AddToCartAsync(string userId, AddToCartDto dto)
        {
            var existing = await _repo.GetByUserAndProductAsync(userId, dto.ProductId);
            if (existing != null)
            {
                existing.Quantity += dto.Quantity;
                _repo.Update(existing);
                await _repo.SaveChangesAsync();
                var updated = await _repo.GetByIdAsync(existing.Id);
                return _mapper.Map<CartItemDto>(updated);
            }

            var item = new CartItem { UserId = userId, ProductId = dto.ProductId, Quantity = dto.Quantity };
            await _repo.AddAsync(item);
            await _repo.SaveChangesAsync();
            var created = await _repo.GetByIdAsync(item.Id);
            return _mapper.Map<CartItemDto>(created);
        }

        public async Task<bool> UpdateQuantityAsync(string userId, int cartItemId, int quantity)
        {
            var item = await _repo.GetByIdAsync(cartItemId);
            if (item == null || item.UserId != userId) return false;
            item.Quantity = quantity;
            _repo.Update(item);
            return await _repo.SaveChangesAsync();
        }

        public async Task<bool> RemoveAsync(string userId, int cartItemId)
        {
            var item = await _repo.GetByIdAsync(cartItemId);
            if (item == null || item.UserId != userId) return false;
            _repo.Remove(item);
            return await _repo.SaveChangesAsync();
        }
    }
}