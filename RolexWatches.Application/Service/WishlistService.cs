using AutoMapper;
using RolexWatches.Application.Dto;
using RolexWatches.Application.ServiceInterface;
using RolexWatches.Domain.Entities;
using RolexWatches.Domain.Interfaces;

namespace RolexWatches.Application.Service
{
    public class WishlistService : IWishlistService
    {
        private readonly IWishlistRepository _repo;
        private readonly IMapper _mapper;

        public WishlistService(IWishlistRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<WishlistItemDto>> GetWishlistAsync(string userId)
        {
            var items = await _repo.GetByUserIdAsync(userId);
            return _mapper.Map<List<WishlistItemDto>>(items);
        }

        public async Task<bool> AddAsync(string userId, AddToWishlistDto dto)
        {
            var existing = await _repo.GetByUserAndProductAsync(userId, dto.ProductId);
            if (existing != null) return true;

            await _repo.AddAsync(new WishlistItem { UserId = userId, ProductId = dto.ProductId });
            return await _repo.SaveChangesAsync();
        }

        public async Task<bool> RemoveAsync(string userId, int productId)
        {
            var item = await _repo.GetByUserAndProductAsync(userId, productId);
            if (item == null) return false;
            _repo.Remove(item);
            return await _repo.SaveChangesAsync();
        }
    }
}