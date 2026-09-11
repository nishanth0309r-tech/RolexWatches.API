using AutoMapper;
using RolexWatches.Application.Dto;
using RolexWatches.Application.ServiceInterface;
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

        public ReviewService(IReviewRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
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
    }
}
