using System.Threading.Tasks;
using Athena.API.Services;
using Quartz;

namespace Athena.API.Jobs
{
    public class CrawlerJob : BaseJob
    {
        private readonly IStockService _stockService;

        public CrawlerJob(IStockService stockService)
        {
            _stockService = stockService;
        }
        protected override async Task ExecuteJobAsync(IJobExecutionContext context)
        {
            await _stockService.Check();
        }
    }
}