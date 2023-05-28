using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;
using Store.Domain.Enums;
using Store.Domain.Models;
using Store.Domain.Models.ManyToManyProductEntities;
using Store.Domain.Models.ProductEntities;

namespace Store.Domain
{
    public class StoreContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<ClothingCollection> ClothingCollections { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<CollectionProduct> CollectionsProducts { get; set; }
        public DbSet<ColorProduct> ColorsProducts { get; set; }
        public DbSet<ProductSize> ProductsSizes { get; set; }
        public DbSet<PromoPage> PromoPages { get; set; }
        public DbSet<UserEmailConfirmationHash> UserConfirmationHashes { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Product>()
                .HasIndex(prod => prod.Article)
                .IsUnique();

            modelBuilder
                .Entity<CollectionProduct>()
                .HasKey(cp => new { cp.ProductId, cp.CollectionId });
            modelBuilder
                .Entity<CollectionProduct>()
                .HasIndex(cp => new { cp.ProductId });
            modelBuilder
                .Entity<CollectionProduct>()
                .HasIndex(cp => new { cp.ProductId, cp.CollectionId })
                .IsUnique();
            modelBuilder
                .Entity<CollectionProduct>()
                .HasOne(cp => cp.Product)
                .WithMany(cp => cp.Collections)
                .HasForeignKey(cp => cp.ProductId);
            modelBuilder
                .Entity<CollectionProduct>()
                .HasOne(cp => cp.Collection)
                .WithMany(cp => cp.Products)
                .HasForeignKey(cp => cp.CollectionId);

            modelBuilder
                .Entity<ColorProduct>()
                .HasKey(cp => new { cp.ColorId, cp.ProductId });
            modelBuilder
                .Entity<ColorProduct>()
                .HasIndex(cp => new { cp.ProductId });
            modelBuilder
                .Entity<ColorProduct>()
                .HasIndex(cp => new { cp.ProductId, cp.ColorId })
                .IsUnique();
            modelBuilder
                .Entity<ColorProduct>()
                .HasOne(cp => cp.Color)
                .WithMany(cp => cp.Products)
                .HasForeignKey(cp => cp.ColorId);
            modelBuilder
                .Entity<ColorProduct>()
                .HasOne(cp => cp.Product)
                .WithMany(cp => cp.Colors)
                .HasForeignKey(cp => cp.ProductId);

            modelBuilder
                .Entity<ProductSize>()
                .HasKey(ps => new { ps.ProductId, ps.SizeId });
            modelBuilder
                .Entity<ProductSize>()
                .HasIndex(ps => new { ps.ProductId });
            modelBuilder
                .Entity<ProductSize>()
                .HasIndex(ps => new { ps.ProductId, ps.SizeId })
                .IsUnique();
            modelBuilder
                .Entity<ProductSize>()
                .HasOne(cp => cp.Product)
                .WithMany(cp => cp.Sizes)
                .HasForeignKey(cp => cp.ProductId);
            modelBuilder
                .Entity<ProductSize>()
                .HasOne(cp => cp.Size)
                .WithMany(cp => cp.Products)
                .HasForeignKey(cp => cp.SizeId);

            modelBuilder
                .Entity<User>()
                .Property(user => user.UserRole)
                .HasConversion(new EnumToStringConverter<UserRole>());

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();

        }

        public StoreContext(DbContextOptions<StoreContext> options)
            : base(options)
        {

        }

        public StoreContext()
        {
            Database.EnsureCreated();
        }
    }
}
