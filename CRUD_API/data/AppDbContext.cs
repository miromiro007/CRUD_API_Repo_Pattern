using CRUD_API.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD_API.data
{
    public class AppDbContext:DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>().HasData(
                new Item { Id = 1, Name = "Item 1", Description = "Description for Item 1", CreatedDate = new DateTime(2026, 1, 1) },
                new Item { Id = 2, Name = "Item 2", Description = "Description for Item 2", CreatedDate = new DateTime(2026, 1, 1) },
                new Item { Id = 3, Name = "Item 3", Description = "Description for Item 3", CreatedDate = new DateTime(2026, 1, 1) },
                new Item { Id = 4, Name = "Item 4", Description = "Description for Item 4", CreatedDate = new DateTime(2026, 1, 1) },
                new Item { Id = 5, Name = "Item 5", Description = "Description for Item 5", CreatedDate = new DateTime(2026, 1, 1) }
            );
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Phones" },
                new Category { Id = 2, Name = "Computer" },
                new Category { Id = 3, Name = "TV" }
            );
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Item> items { get; set; }
        public DbSet<Category> categories { get; set; }
    }
}
