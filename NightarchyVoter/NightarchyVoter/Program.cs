using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Interactions;
using System.Collections.ObjectModel;

namespace NightarchyVoter
{
    class Program
    {
        static void Main(string[] args)
        {
            IWebDriver driver = new EdgeDriver();
            List<String> NightarchyVoteSites = new List<string>();
            NightarchyVoteSites.Add("https://minecraft-tracker.com/server/5874/vote/");
            NightarchyVoteSites.Add("https://minelist.net/vote/2401");
            NightarchyVoteSites.Add("https://minecraftservers.org/vote/541893");
            NightarchyVoteSites.Add("https://topminecraftservers.org/vote/4947");
            NightarchyVoteSites.Add("https://www.planetminecraft.com/server/nightarchy-anarchy-minecraft/vote/");
            Boolean couldNotVote = false;
            foreach(String link in NightarchyVoteSites)
            {
                driver.Url = link;
                try
                {
                    if (driver.FindElements(By.Name("username")).Count() != 0)
                    {
                        driver.FindElement(By.Name("username")).SendKeys("NeonWizard");
                    }
                    if (driver.FindElements(By.Name("nickname")).Count() != 0)
                    {
                        driver.FindElement(By.Name("nickname")).SendKeys("NeonWizard");
                        IWebElement checkbox = driver.FindElement(By.Name("accept"));
                        if (!checkbox.Selected)
                            checkbox.Click();
                    }
                    if (driver.FindElements(By.Name("mc_username")).Count() != 0)
                    {
                        driver.FindElement(By.Name("mc_username")).SendKeys("NeonWizard");
                    }
                    if (driver.FindElements(By.Name("mcname")).Count() != 0)
                    {
                        driver.FindElement(By.Name("mcname")).SendKeys("NeonWizard");
                    }
                }
                catch (Exception e)
                {
                    Console.Write("Voting site failed. Skipping: ");
                    Console.WriteLine(link);
                    couldNotVote = true;        
                }
                CheckRecaptcha(driver);
                if (!couldNotVote)
                {
                    Console.WriteLine("Enter any key after vote is completed...");
                    Console.ReadKey();
                }
                else
                {
                    couldNotVote = false;
                }
            }
        }
        private static Boolean CheckRecaptcha(IWebDriver driver)
        {
            ReadOnlyCollection<IWebElement> iframeElements = driver.FindElements(By.TagName("iframe"));
            Boolean captchaFound = false;
            for (int i = 0; i < iframeElements.Count; i++)
            {
                if (iframeElements.ElementAt(i).GetAttribute("src").Contains("https://www.google.com/recaptcha/api2/anchor?"))
                {
                    driver.SwitchTo().Frame(i);
                    //recaptcha-checkbox-border
                    driver.FindElement(By.ClassName("recaptcha-checkbox")).Click();
                    captchaFound = true;
                }
                if (captchaFound == true)
                    break;
            }
            return captchaFound;
        }
    }
}
