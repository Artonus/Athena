using System.Collections.Generic;
using System.Linq;
using Athena.DataAccess.Model;
using Athena.DataAccess.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace Athena.DataAccess.Repository
{
    public interface IRetailerRepository : IRepository<Retailer>
    {
        /// <summary>
        /// Get all products that retailer has in offer
        /// </summary>
        /// <param name="retailerId"></param>
        /// <returns></returns>
        List<Product> GetAllProductsByRetailer(long retailerId);

    }
    public class RetailerRepository : BaseRepository<Retailer>, IRetailerRepository
    {
        public RetailerRepository(AthenaDbContext context) : base(context)
        {
        }

        /// <summary>
        /// <inheritdoc cref="IRetailerRepository.GetAllProductsByRetailer"/>
        /// </summary>
        /// <param name="retailerId"></param>
        /// <returns></returns>
        public List<Product> GetAllProductsByRetailer(long retailerId)
        {
            return Context.ProductRetailers
                .Where(p => p.RetailerID == retailerId)
                .Include(s => s.Product)
                .Select(p => p.Product)
                .ToList();
        }
    }

    
}