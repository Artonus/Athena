using Athena.DataAccess.Model;
using Microsoft.EntityFrameworkCore;

namespace Athena.DataAccess
{
    public class AthenaDbContext : DbContext
    {
        private readonly DbContextOptions<AthenaDbContext> _options;

        public AthenaDbContext(DbContextOptions<AthenaDbContext> options) : base(options)
        {
            _options = options;
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Retailer> Retailers { get; set; }
        public DbSet<ProductRetailer> ProductRetailers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // override default EF pluralization of the table name that would be taken from the DbSet property names 
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<Retailer>().ToTable("Retailer");
            modelBuilder.Entity<ProductRetailer>().ToTable("ProductRetailer");
            // define the relationships
            //modelBuilder.Entity<Retailer>().HasMany(r => r.Products).WithOne();
            modelBuilder.Entity<ProductRetailer>()
                .HasOne(p => p.Product)
                .WithMany(r => r.ProductRetailers)
                .HasForeignKey(p=>p.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductRetailer>()
                .HasOne(p => p.Retailer)
                .WithMany(r => r.ProductRetailers)
                .HasForeignKey(p=>p.RetailerID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}