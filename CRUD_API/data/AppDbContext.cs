using CRUD_API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CRUD_API.data
{
    public class AppDbContext:IdentityDbContext<AppUser>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Phones" },
                new Category { Id = 2, Name = "Computers" },
                new Category { Id = 3, Name = "TV" }
            );

            modelBuilder.Entity<Item>().HasData(
                new Item { Id = 1, Name = "iPhone 15", Description = "Apple smartphone", CreatedDate = new DateTime(2025, 1, 1), CategoryId = 1 },
                new Item { Id = 2, Name = "Dell XPS", Description = "Laptop", CreatedDate = new DateTime(2025, 1, 2), CategoryId = 2 }
            );

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Item> items { get; set; }
        public DbSet<Category> categories { get; set; }
    }
}
