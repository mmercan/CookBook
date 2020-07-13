using System;
using System.Collections.Generic;
using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using Polly;
using SeleniumExtras.WaitHelpers;
using Xunit;

namespace CookBook.Recipe.UI.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {

            ChromeOptions options = new ChromeOptions();
            options.AddArguments("user-data-dir=C:\\Users\\mmercan\\AppData\\Local\\Google\\Chrome\\User Data");
            // WebDriver drivers = new ChromeDriver(options);
            // using (var driver = new ChromeDriver(options))


            // FirefoxOptions options = new FirefoxOptions();
            // var profileManager = new FirefoxProfileManager();
            // FirefoxProfile profile = profileManager.GetProfile("default");
            // options.Profile = profile;
            //IWebDriver driver = new FirefoxDriver(profile);
            //using (var driver = new FirefoxDriver(options))


            using (var driver = new ChromeDriver(options))
            {
                var idList = new List<string>();
                string line;
                int counter = 0;
                System.IO.StreamReader file = new System.IO.StreamReader(@"c:\temp\tfvcids.txt");
                while ((line = file.ReadLine()) != null)
                {
                    System.Console.WriteLine(line);
                    counter++;
                    idList.Add(line);
                }


                Assert.Equal(idList.Count, 1500);

                //var driver = attachToChrome(@"https://bupaaunz.visualstudio.com/Hugo/_versionControl/changeset/93399/");
                driver.Navigate().GoToUrl(@"https://bupaaunz.visualstudio.com/Hugo/_versionControl/changeset/93399/");
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                //var link = driver.FindElement(By.CssSelector("span[class='body-m text-ellipsis']"));


                var item = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("span[class='body-m text-ellipsis']")));



                Assert.Equal(item.Text, "2 changed file");

                //var link = driver.FindElement(By.PartialLinkText("Browse Files"));
                // var jsToBeExecuted = $"window.scroll(0, {link.Location.Y});";
                // ((IJavaScriptExecutor)driver).ExecuteScript(jsToBeExecuted);
                //var wait = new WebDriverWait(driver, TimeSpan.FromMinutes(1));
                // var clickableElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.PartialLinkText("Browse Files")));
                // clickableElement.Click();
            }
        }


        private void launchBrowser(string url)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
            proc.StartInfo.Arguments = url + " --remote-debugging-port=9222";
            proc.Start();
        }


        public IWebDriver attachToChrome(string url)
        {
            launchBrowser(url);
            IWebDriver driver = null;
            ChromeOptions options = new ChromeOptions();
            options.DebuggerAddress = "127.0.0.1:9222";

            // Using Polly library: https://github.com/App-vNext/Polly
            // Polly probably isn't needed in a single scenario like this, but can be useful in a broader automation project
            // Once we attach to Chrome with Selenium, use a WebDriverWait implementation
            var policy = Policy
              .Handle<InvalidOperationException>()
              .WaitAndRetry(10, t => TimeSpan.FromSeconds(1));

            policy.Execute(() =>
            {
                driver = new ChromeDriver(options);
            });
            return driver;
        }

    }
}
