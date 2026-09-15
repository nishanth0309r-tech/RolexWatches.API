
using System.ComponentModel.DataAnnotations;


namespace RolexWatches.Application.DTOs.Brand
{
    public class BrandDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? LogoUrl { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public int ProductCount { get; set; }
    }
    public class CreateUpdateBrandDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Url]
        public string? LogoUrl { get; set; }

        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
