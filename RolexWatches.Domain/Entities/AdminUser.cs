namespace RolexWatches.Domain.Entities
{
    public class AdminUser
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string PasswordSalt { get; set; } = string.Empty;
        public string Role { get; set; } = "Customer"; // "Customer" or "Admin"
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}