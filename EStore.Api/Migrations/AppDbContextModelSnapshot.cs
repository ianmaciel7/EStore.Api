﻿// <auto-generated />
using System;
using EStore.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EStore.API.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EStore.API.Data.Entities.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            CategoryId = 1,
                            Name = "Periféricos"
                        },
                        new
                        {
                            CategoryId = 2,
                            Name = "Hardware"
                        });
                });

            modelBuilder.Entity("EStore.API.Data.Entities.SubCategory", b =>
                {
                    b.Property<int>("SubCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SubCategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("SubCategories");

                    b.HasData(
                        new
                        {
                            SubCategoryId = 1,
                            CategoryId = 1,
                            Name = "Teclado"
                        },
                        new
                        {
                            SubCategoryId = 2,
                            CategoryId = 1,
                            Name = "Mouse"
                        },
                        new
                        {
                            SubCategoryId = 3,
                            CategoryId = 2,
                            Name = "Placa de video"
                        },
                        new
                        {
                            SubCategoryId = 4,
                            CategoryId = 2,
                            Name = "Processador"
                        });
                });

            modelBuilder.Entity("EStore.API.Data.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int?>("SubCategoryId")
                        .HasColumnType("int");

                    b.HasKey("ProductId");

                    b.HasIndex("SubCategoryId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            ProductId = 1,
                            Name = "GTX 1080TI",
                            Price = 1200.0,
                            SubCategoryId = 3
                        },
                        new
                        {
                            ProductId = 2,
                            Name = "GTX 2080TI",
                            Price = 2500.9899999999998,
                            SubCategoryId = 3
                        },
                        new
                        {
                            ProductId = 3,
                            Name = "GTX 1070",
                            Price = 500.0,
                            SubCategoryId = 3
                        },
                        new
                        {
                            ProductId = 4,
                            Name = "AMD Ryzen 7 3700X",
                            Price = 1700.0,
                            SubCategoryId = 4
                        },
                        new
                        {
                            ProductId = 5,
                            Name = "Teclado gamer",
                            Price = 1700.0,
                            SubCategoryId = 1
                        });
                });

            modelBuilder.Entity("EStore.API.Data.Entities.SubCategory", b =>
                {
                    b.HasOne("EStore.API.Data.Entities.Category", "Category")
                        .WithMany("SubCategories")
                        .HasForeignKey("CategoryId");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("EStore.API.Data.Product", b =>
                {
                    b.HasOne("EStore.API.Data.Entities.SubCategory", "SubCategory")
                        .WithMany()
                        .HasForeignKey("SubCategoryId");

                    b.Navigation("SubCategory");
                });

            modelBuilder.Entity("EStore.API.Data.Entities.Category", b =>
                {
                    b.Navigation("SubCategories");
                });
#pragma warning restore 612, 618
        }
    }
}
