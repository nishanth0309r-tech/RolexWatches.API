namespace RolexWatches.Application.Dto
{
    public class BrandDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class CreateBrandDto
    {
        public string Name { get; set; } = string.Empty;
    }
}