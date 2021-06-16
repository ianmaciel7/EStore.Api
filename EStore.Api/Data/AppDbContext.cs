using EStore.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace EStore.API.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext([NotNullAttribute] DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder
                .Entity<Category>()
                .HasData(
                new { CategoryId = 1, Name = "Periféricos" },
                new { CategoryId = 2, Name = "Hardware" }
                );

            modelBuilder
                .Entity<SubCategory>()
                .HasData(
                new { SubCategoryId = 1, CategoryId = 1, Name = "Teclado" },
                new { SubCategoryId = 2, CategoryId = 1, Name = "Mouse" },
                new { SubCategoryId = 3, CategoryId = 2, Name = "Placa de video" },
                new { SubCategoryId = 4, CategoryId = 2, Name = "Processador" }
                );

            modelBuilder
                .Entity<Product>()
                .HasData(
                new { ProductId = 1, Name = "GTX 1080TI", Price = 1200.00, SubCategoryId = 3 },
                new { ProductId = 2, Name = "GTX 2080TI", Price = 2500.99, SubCategoryId = 3 },
                new { ProductId = 3, Name = "GTX 1070", Price = 500.00, SubCategoryId = 3 },
                new { ProductId = 4, Name = "AMD Ryzen 7 3700X", Price = 1700.00, SubCategoryId = 4 },
                new { ProductId = 5, Name = "Teclado gamer", Price = 1700.00, SubCategoryId = 1 }
                );

        }
    }
}
