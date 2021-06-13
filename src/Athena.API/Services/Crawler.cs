using System;
using System.Threading;
using Athena.API.Model;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium.Chrome;
using StackExchange.Redis;

namespace Athena.API.Services
{
    public class Crawler
    {
        public Crawler(RetailerModel retailerModel)
        {
            var options = new ChromeOptions();
            bool result = false;

            options.AddArguments("--headless");
            options.AddArguments("--window-size=1920,1080");
            options.AddArguments("--no-sandbox");
            options.AddArguments("--disable-dev-shm-usage");

            if(!string.IsNullOrEmpty(retailerModel.options))options.AddArguments(retailerModel.options);

            var service = ChromeDriverService.CreateDefaultService();
            service.SuppressInitialDiagnosticInformation = true;

            using(var driver = new ChromeDriver(service, options)){
                try {
                    driver.Navigate().GoToUrl(retailerModel.start);
                    driver.Manage().Window.Maximize();

                    foreach(string[] actions in retailerModel.actions){
                        switch(actions[0]){
                            case "equals":
                                switch(actions[1]){
                                    case "id":
                                    result = driver.FindElementById(actions[2]).Text == actions[3] ? true : false;
                                    break;
                                    case "class":
                                    result = driver.FindElementByClassName(actions[2]).Text == actions[3] ? true : false;
                                    break;
                                    case "xpath":
                                    result = driver.FindElementByXPath(actions[2]).Text == actions[3] ? true : false;
                                    break;
                                    case "css":
                                    result = driver.FindElementByCssSelector(actions[2]).Text == actions[3] ? true : false;
                                    break;
                                }
                                if(actions.Length>4)result = !result;
                            break;

                            case "click":
                                switch(actions[1]){
                                    case "id":
                                    driver.FindElementById(actions[2]).Click();
                                    break;

                                    case "class":
                                    driver.FindElementByClassName(actions[2]).Click();
                                    break;

                                    case "xpath":
                                    driver.FindElementByXPath(actions[2]).Click();
                                    break;

                                    case "css":
                                    driver.FindElementByCssSelector(actions[2]).Click();
                                    break;
                                }
                            break;

                            case "type":
                                switch(actions[1]){
                                    case "id":
                                    driver.FindElementById(actions[2]).SendKeys(actions[3]);
                                    break;

                                    case "class":
                                    driver.FindElementByClassName(actions[2]).SendKeys(actions[3]);
                                    break;

                                    case "xpath":
                                    driver.FindElementByXPath(actions[2]).SendKeys(actions[3]);
                                    break;

                                    case "css":
                                    driver.FindElementByCssSelector(actions[2]).SendKeys(actions[3]);
                                    break;
                                }
                            break;

                            case "contains":
                                switch(actions[1]){
                                    case "id":
                                    result = driver.FindElementById(actions[2]).Text.Contains(actions[3]);
                                    break;

                                    case "class":
                                    result = driver.FindElementByClassName(actions[2]).Text.Contains(actions[3]);
                                    break;

                                    case "xpath":
                                    result = driver.FindElementByXPath(actions[2]).Text.Contains(actions[3]);
                                    break;

                                    case "css":
                                    result = driver.FindElementByCssSelector(actions[2]).Text.Contains(actions[3]);
                                    break;
                                }
                            break;

                            case "wait":
                                Thread.Sleep(Convert.ToInt32(actions[1]));
                            break;

                            case "screenshot":
                                driver.GetScreenshot().SaveAsFile(actions[1]);
                            break;
                        }
                    }
                    Console.WriteLine("Successfully checked {0}", retailerModel.name);
                    driver.Close();
                    driver.Quit();
                }
                catch(Exception e){
                    Console.WriteLine("Error when checking {0}", retailerModel.name);
                    Console.WriteLine(e.ToString());
                }
            }

            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(Startup.Configuration.GetConnectionString("Redis"));
            IDatabase db = redis.GetDatabase();
            db.StringSet(retailerModel.name,result);
        }
    }
}