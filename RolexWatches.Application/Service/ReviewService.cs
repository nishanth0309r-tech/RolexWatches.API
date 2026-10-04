using AutoMapper;
using RolexWatches.Application.Dto;
using RolexWatches.Application.ServiceInterface;
using RolexWatches.Domain.Entities;
using RolexWatches.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RolexWatches.Application.Service
{
    public class ReviewService:IReviewService
    {
        private readonly IReviewRepository repo;
        private readonly IMapper mapper;
        private readonly IProductRepository productRepo;

        public ReviewService(IReviewRepository repo, IMapper mapper, IProductRepository productRepository)
        {
            this.repo = repo;
            this.mapper = mapper;
            this.productRepo = productRepository;
        }

        public async Task<ReviewDto> CreateAsync(int userId, CreateReviewDto dto)
        {
            {
                if (await productRepo.GetByIdAsync(dto.ProductId) == null)
                    throw new InvalidOperationException("Product not found.");

                if (!await repo.HasPurchasedAsync(userId, dto.ProductId))
                    throw new InvalidOperationException("You can review a product only after buying it.");

                if (await repo.GetByUserAndProductAsync(userId, dto.ProductId) != null)
                    throw new InvalidOperationException("You have already reviewed this product.");

                var review = new Review
                {
                    ProductId = dto.ProductId,
                    UserId = userId,
                    Rating = dto.Rating,
                    Comment = dto.Comment.Trim(),
                    CreatedAt = DateTime.UtcNow
                };

                await repo.AddAsync(review);
                if (!await repo.SaveChangesAsync())
                    throw new InvalidOperationException("Failed to save review.");

                var saved = await repo.GetByIdAsync(review.Id);   // reload with User and Product
                return mapper.Map<ReviewDto>(saved);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var review=await repo.GetByIdAsync(id);
            if (review == null)
            {
                return false;
            }
            repo.Delete(review);
            return await repo.SaveChangesAsync();
        }

        public async Task<List<ReviewDto>> GetAllAsync()
        {
            var reviews=await repo.GetAllAsync();
            return mapper.Map<List<ReviewDto>>(reviews);
        }

        public async Task<ProductReviewsDto> GetForProductAsync(int productId)
        {
            var reviews = await repo.GetByProductIdAsync(productId);
            return new ProductReviewsDto
            {
                ReviewCount = reviews.Count,
                AverageRating = reviews.Count == 0 ? 0 : Math.Round(reviews.Average(r => r.Rating), 1),
                Reviews = mapper.Map<List<ReviewDto>>(reviews)
            };
        }
    }
}
