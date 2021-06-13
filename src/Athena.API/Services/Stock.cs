using System.Collections.Generic;
using System.Text.Json;
using Athena.API.Model;

namespace Athena.API.Services
{
    public class Stock
    {   

        private static List<string> templates;
        public static void Check(){
            templates.ForEach(template =>
            {
                if(!string.IsNullOrEmpty(template)){
                    var retailers = JsonSerializer.Deserialize<Dictionary<int ,RetailerModel>>(template);

                    foreach(KeyValuePair<int, RetailerModel> retailer in retailers){
                        Crawler crawler = new Crawler(retailer.Value);
                    }
                }
            });
        }

        public static void Init(List<string> _templates){
            templates = _templates;
        }
    }
}