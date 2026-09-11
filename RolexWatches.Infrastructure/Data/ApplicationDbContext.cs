using Microsoft.EntityFrameworkCore;
using RolexWatches.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RolexWatches.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options) { }

        public DbSet<User> Qwin9Users => Set<User>();

        public DbSet<CartItem> CartItems { get; set; } = null!;
        public DbSet<WishlistItem> WishlistItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {

                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Email).IsRequired().HasMaxLength(200);
                entity.Property(u => u.FullName).IsRequired().HasMaxLength(100);
            });
        }
    }
}
