using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Athena.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockController : ControllerBase
    {
        private readonly IConnectionMultiplexer redis;
        public StockController(IConnectionMultiplexer _redis){
            redis = _redis;
        }
        [HttpGet("{product}")]
        public IActionResult Check([FromRoute] string product)
        {
            string conn = Startup.Configuration.GetConnectionString("Redis")+":6379";
            Dictionary<string, string> result =  new Dictionary<string, string>();
            IDatabase db = redis.GetDatabase();
            var server = redis.GetServer(conn);
            
            foreach (var key in server.Keys(pattern: product+":*"))
            {
                result.Add(key, db.StringGet(key));
            }

            return Ok(result);
        }
    }
}