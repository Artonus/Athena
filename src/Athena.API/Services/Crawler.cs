using System;
using System.Threading;
using Athena.API.Model;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium.Chrome;
using StackExchange.Redis;

namespace Athena.API.Services
{
    public interface ICrawler
    {
        bool Crawl(RetailerModel retailerModel);
    }

    public class Crawler : ICrawler
    {
        private readonly ChromeOptions _options;
        public Crawler(RetailerModel retailerModel)
        {
            _options = new ChromeOptions();
            _options.AddArguments("--headless");
            _options.AddArguments("--window-size=1920,1080");
            _options.AddArguments("--no-sandbox");
            _options.AddArguments("--disable-dev-shm-usage");
        }

        public bool Crawl(RetailerModel retailerModel)
        {
            bool result = false;
            if (string.IsNullOrWhiteSpace(retailerModel.options) == false)
                _options.AddArguments(retailerModel.options);

            var service = ChromeDriverService.CreateDefaultService();
            service.SuppressInitialDiagnosticInformation = true;

            using (var driver = new ChromeDriver(service, _options))
            {
                try
                {
                    driver.Navigate().GoToUrl(retailerModel.start);
                    driver.Manage().Window.Maximize();

                    foreach (string[] actions in retailerModel.actions)
                    {
                        switch (actions[0])
                        {
                            case "equals":
                                switch (actions[1])
                                {
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

                                if (actions.Length > 4) result = !result;
                                break;

                            case "click":
                                switch (actions[1])
                                {
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
                                switch (actions[1])
                                {
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
                                switch (actions[1])
                                {
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
                catch (Exception e)
                {
                    Console.WriteLine("Error when checking {0}", retailerModel.name);
                    Console.WriteLine(e.ToString());
                }
            }
            //TODO: extract redis connection
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(Startup.Configuration.GetConnectionString("Redis"));
            IDatabase db = redis.GetDatabase();
            db.StringSet(retailerModel.name, result);
            return result;
        }
    }
}