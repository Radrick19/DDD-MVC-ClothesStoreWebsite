﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Store.Domain;

#nullable disable

namespace Store.Domain.Migrations
{
    [DbContext(typeof(StoreContext))]
    [Migration("20230413144743_addDescriptionFieldToClothingCollection")]
    partial class addDescriptionFieldToClothingCollection
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Store.Domain.Models.ManyToManyEntity.CollectionProduct", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<int>("CollectionId")
                        .HasColumnType("integer");

                    b.HasKey("ProductId", "CollectionId");

                    b.HasIndex("CollectionId");

                    b.HasIndex("ProductId");

                    b.HasIndex("ProductId", "CollectionId")
                        .IsUnique();

                    b.ToTable("CollectionsProducts");
                });

            modelBuilder.Entity("Store.Domain.Models.ManyToManyEntity.ColorProduct", b =>
                {
                    b.Property<int>("ColorId")
                        .HasColumnType("integer");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.HasKey("ColorId", "ProductId");

                    b.HasIndex("ProductId");

                    b.HasIndex("ProductId", "ColorId")
                        .IsUnique();

                    b.ToTable("ColorsProducts");
                });

            modelBuilder.Entity("Store.Domain.Models.ManyToManyEntity.ProductShop", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<int>("ShopId")
                        .HasColumnType("integer");

                    b.Property<int>("ProductQuantity")
                        .HasColumnType("integer");

                    b.HasKey("ProductId", "ShopId");

                    b.HasIndex("ProductId");

                    b.HasIndex("ShopId");

                    b.HasIndex("ProductId", "ShopId")
                        .IsUnique();

                    b.ToTable("ProductsShops");
                });

            modelBuilder.Entity("Store.Domain.Models.ManyToManyEntity.ProductSize", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<int>("SizeId")
                        .HasColumnType("integer");

                    b.HasKey("ProductId", "SizeId");

                    b.HasIndex("ProductId");

                    b.HasIndex("SizeId");

                    b.HasIndex("ProductId", "SizeId")
                        .IsUnique();

                    b.ToTable("ProductsSizes");
                });

            modelBuilder.Entity("Store.Domain.Models.ProductProps.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Order")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Store.Domain.Models.ProductProps.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("Store.Domain.Models.ProductProps.ClothingCollection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActual")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Order")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("ClothingCollections");
                });

            modelBuilder.Entity("Store.Domain.Models.ProductProps.Color", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Hex")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Order")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Colors");
                });

            modelBuilder.Entity("Store.Domain.Models.ProductProps.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string[]>("AdditionalPictures")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("Article")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MainPicture")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Price")
                        .HasColumnType("integer");

                    b.Property<int>("SubcategoryId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("SubcategoryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Store.Domain.Models.ProductProps.Shop", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("CityId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("Shops");
                });

            modelBuilder.Entity("Store.Domain.Models.ProductProps.Size", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Order")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Sizes");
                });

            modelBuilder.Entity("Store.Domain.Models.ProductProps.Subcategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("CanReturn")
                        .HasColumnType("boolean");

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Order")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Subcategories");
                });

            modelBuilder.Entity("Store.Domain.Models.ManyToManyEntity.CollectionProduct", b =>
                {
                    b.HasOne("Store.Domain.Models.ProductProps.ClothingCollection", "Collection")
                        .WithMany("Products")
                        .HasForeignKey("CollectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Store.Domain.Models.ProductProps.Product", "Product")
                        .WithMany("Collections")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Collection");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Store.Domain.Models.ManyToManyEntity.ColorProduct", b =>
                {
                    b.HasOne("Store.Domain.Models.ProductProps.Color", "Color")
                        .WithMany("Products")
                        .HasForeignKey("ColorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Store.Domain.Models.ProductProps.Product", "Product")
                        .WithMany("Colors")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Color");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Store.Domain.Models.ManyToManyEntity.ProductShop", b =>
                {
                    b.HasOne("Store.Domain.Models.ProductProps.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Store.Domain.Models.ProductProps.Shop", "Shop")
                        .WithMany()
                        .HasForeignKey("ShopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Shop");
                });

            modelBuilder.Entity("Store.Domain.Models.ManyToManyEntity.ProductSize", b =>
                {
                    b.HasOne("Store.Domain.Models.ProductProps.Product", "Product")
                        .WithMany("Sizes")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Store.Domain.Models.ProductProps.Size", "Size")
                        .WithMany("Products")
                        .HasForeignKey("SizeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Size");
                });

            modelBuilder.Entity("Store.Domain.Models.ProductProps.Product", b =>
                {
                    b.HasOne("Store.Domain.Models.ProductProps.Subcategory", "Subcategory")
                        .WithMany()
                        .HasForeignKey("SubcategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subcategory");
                });

            modelBuilder.Entity("Store.Domain.Models.ProductProps.Shop", b =>
                {
                    b.HasOne("Store.Domain.Models.ProductProps.City", "City")
                        .WithMany()
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("Store.Domain.Models.ProductProps.Subcategory", b =>
                {
                    b.HasOne("Store.Domain.Models.ProductProps.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Store.Domain.Models.ProductProps.ClothingCollection", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Store.Domain.Models.ProductProps.Color", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Store.Domain.Models.ProductProps.Product", b =>
                {
                    b.Navigation("Collections");

                    b.Navigation("Colors");

                    b.Navigation("Sizes");
                });

            modelBuilder.Entity("Store.Domain.Models.ProductProps.Size", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
