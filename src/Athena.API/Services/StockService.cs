using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Athena.API.Model;
using Athena.DataAccess.Repository;

namespace Athena.API.Services
{
    public interface IStockService
    {
        void Check();
    }

    public class StockService : IStockService
    {
        private readonly ICrawler _crawler;
        private readonly IProductRepository _productRepository;

        public StockService(ICrawler crawler, IProductRepository productRepository)
        {
            _crawler = crawler;
            _productRepository = productRepository;
        }

        public void Check()
        {
            var templates = _productRepository.GetAll().Select(s => s.AccessTemplate).ToList();
            foreach (var template in templates)
            {
                if (string.IsNullOrEmpty(template))
                    continue;

                var retailers = JsonSerializer.Deserialize<Dictionary<int, RetailerModel>>(template);

                foreach (KeyValuePair<int, RetailerModel> retailer in retailers)
                {
                    _crawler.Crawl(retailer.Value);
                }
            }
        }
    }
}