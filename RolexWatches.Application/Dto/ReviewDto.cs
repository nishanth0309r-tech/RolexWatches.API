using System.ComponentModel.DataAnnotations;

namespace RolexWatches.Application.Dto
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class CreateReviewDto
    {
        [Required] public int ProductId { get; set; }
        [Range(1, 5)] public int Rating { get; set; }
        [Required, MaxLength(1000)] public string Comment { get; set; } = string.Empty;
    }

    public class ProductReviewsDto
    {
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public List<ReviewDto> Reviews { get; set; } = new();
    }
}