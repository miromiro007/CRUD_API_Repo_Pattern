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

        public DbSet<Item> items { get; set; }
        public DbSet<Category> categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Phones" },
                new Category { Id = 2, Name = "Computers" },
                new Category { Id = 3, Name = "TV" }
            );

            var items = new List<Item>();
            for (int i = 3; i <= 102; i++) // Start from 3 if 1 and 2 already exist
            {
                items.Add(new Item
                {
                    Id = i,
                    Name = $"Item {i}",
                    Description = $"Description for Item {i}",
                    CreatedDate = new DateTime(2025, 1, 1).AddDays(i),
                    CategoryId = (i % 3) + 1
                });
            }
            modelBuilder.Entity<Item>().HasData(
                new Item { Id = 1, Name = "iPhone 15", Description = "Apple smartphone", CreatedDate = new DateTime(2025, 1, 1), CategoryId = 1 },
                new Item { Id = 2, Name = "Dell XPS", Description = "Laptop", CreatedDate = new DateTime(2025, 1, 2), CategoryId = 2 }
            );
            modelBuilder.Entity<Item>().HasData(items);

            modelBuilder.Entity<AppUser>().HasIndex(u => u.Email).IsUnique();
              

            base.OnModelCreating(modelBuilder);
        }

        
    }
}
