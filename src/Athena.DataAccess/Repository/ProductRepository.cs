using Athena.DataAccess.Model;
using Athena.DataAccess.Repository.Base;

namespace Athena.DataAccess.Repository
{
    public interface IProductRepository : IRepository<Product>
    {
        // if any specific methods needed, add them here
        // example in the Retailer repository
    }
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(AthenaDbContext context) : base(context)
        {
        }
    }
}