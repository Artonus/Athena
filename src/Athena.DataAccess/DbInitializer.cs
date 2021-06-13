using System.Linq;
using Athena.DataAccess.Model;

namespace Athena.DataAccess
{
    public static class DbInitializer
    {
        public static void Initialize(AthenaDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.ProductRetailers.Any())
            {
                return;
            }

            var retailers = new[]
            {
                new Retailer(){ID = 1, Name = "Amazon", Description = "Amazon UK", Url = "https://amazon.co.uk"},
                new Retailer(){ID = 2, Name = "Argos", Description = "Argos", Url = "https://argos.co.uk"},
                new Retailer(){ID = 3, Name = "Game", Description = "Game", Url = "https://game.co.uk"},
                new Retailer(){ID = 4, Name = "Curry's", Description = "Curry's", Url = "https://currys.co.uk"},
            };
            foreach (var retailer in retailers)
            {
                context.Retailers.Add(retailer);
            }
            context.SaveChanges();
            var products = new[]
            {
                new Product() {ID = 1, Name = "PlayStation 5", Description = "PlayStation 5 Gaming Console"},
                new Product() {ID = 2, Name = "XBox Series X", Description = "XBox Series X Gaming Console"}
            };
            foreach (var product in products)
            {
                context.Products.Add(product);
            }

            context.SaveChanges();

            var productRetailers = new[]
            {
                new ProductRetailer() {ID = 1, ProductID = 1, RetailerID = 1},
                new ProductRetailer() {ID = 2, ProductID = 1, RetailerID = 2},
                new ProductRetailer() {ID = 3, ProductID = 1, RetailerID = 3},
                new ProductRetailer() {ID = 4, ProductID = 1, RetailerID = 4},
                new ProductRetailer() {ID = 5, ProductID = 2, RetailerID = 1},
                new ProductRetailer() {ID = 6, ProductID = 2, RetailerID = 2},
                new ProductRetailer() {ID = 7, ProductID = 2, RetailerID = 3},
                new ProductRetailer() {ID = 8, ProductID = 2, RetailerID = 4}
            };
            foreach (var productRetailer in productRetailers)
            {
                context.ProductRetailers.Add(productRetailer);
            }

            context.SaveChanges();
        }
    }
}