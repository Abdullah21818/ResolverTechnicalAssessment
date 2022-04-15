using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;



namespace ResolverAutomationAssessment
{
    public class Tests
    {
        IWebDriver driver;
        public string homePageURL = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\Resources\QE-index.html";
      
        [SetUp]
        public void Setup()
        {
            string path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            driver = new ChromeDriver(path + @"\drivers\");
        }
       

        [Test]
        public void Test1()
        {
            driver.Navigate().GoToUrl(homePageURL);
            IWebElement emailInputField = driver.FindElement(By.Id("inputEmail"));
            IWebElement passwordInputField = driver.FindElement(By.Id("inputPassword"));
            
            Assert.IsTrue(emailInputField.Displayed,"Could not find input for email address!");
            Assert.IsTrue(passwordInputField.Displayed, "Could not find input for password!");
            Assert.IsTrue(driver.FindElement(By.CssSelector("button[class='btn btn-lg btn-primary btn-block']")).Displayed, "Could not find Sign-in button!");

            emailInputField.SendKeys("email@resolver.com");
            passwordInputField.SendKeys("password");
            //Note for evaluator, did not press sign in button since not included in instructions
        }

        [Test]
        public void Test2()
        {
            driver.Navigate().GoToUrl(homePageURL);
            IWebElement test2DivListContainer = driver.FindElement(By.Id("test-2-div")).FindElement(By.CssSelector("ul[class='list-group']"));
            List<IWebElement> test2DivListItems = test2DivListContainer.FindElements(By.CssSelector("li[class='list-group-item justify-content-between']")).ToList();
            
            Assert.AreEqual(3, test2DivListItems.Count, $"The number of expected items was 3, but the list only contains {test2DivListItems.Count} items!");
            
            string secondListItemBadgeValue = test2DivListItems[1].FindElement(By.CssSelector("span[class='badge badge-pill badge-primary']")).Text;
            string secondListItemValue = test2DivListItems[1].Text.Replace(secondListItemBadgeValue, "");
            Assert.AreEqual("List Item 2 ", secondListItemValue, "Second item in list's value is incorrect");
            Assert.AreEqual("6", secondListItemBadgeValue, "Second item's badge value is incorrect");

        }

        [Test]
        public void Test3()
        {
            driver.Navigate().GoToUrl(homePageURL);
            IWebElement test3DivContainer = driver.FindElement(By.Id("test-3-div"));
            IWebElement dropDownMenu = test3DivContainer.FindElement(By.CssSelector("button[id='dropdownMenuButton']"));
            
            Assert.AreEqual("Option 1", dropDownMenu.Text, $"The default selected value is not correct!");

            dropDownMenu.Click();
            driver.FindElement(By.LinkText("Option 3")).Click();
           

        }

        [Test]
        public void Test4()
        {
            driver.Navigate().GoToUrl(homePageURL);
            IWebElement test4DivContainer = driver.FindElement(By.Id("test-4-div"));
            
            Assert.IsNull(test4DivContainer.FindElement(By.CssSelector("button[class='btn btn-lg btn-primary']")).GetAttribute("disabled"), $"The first button is not enabled!");
            Assert.IsTrue(bool.Parse(test4DivContainer.FindElement(By.CssSelector("button[class='btn btn-lg btn-secondary']")).GetAttribute("disabled")), $"The second button is enabled!");
           

        }

        [Test]
        public void Test5()
        {
            driver.Navigate().GoToUrl(homePageURL);
            IWebElement test5DivContainer = driver.FindElement(By.Id("test-5-div"));
            
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(13));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button[id='test5-button']")));

            test5DivContainer.FindElement(By.CssSelector("button[id='test5-button']")).Click();
            
            Assert.AreNotEqual("display: none;", test5DivContainer.FindElement(By.CssSelector("div[class='alert alert-success']")).GetAttribute("style"), "Success message is not displayed");
            Assert.IsTrue(bool.Parse(test5DivContainer.FindElement(By.CssSelector("button[id='test5-button']")).GetAttribute("disabled")), $"The button is still enabled!");


        }

        [Test]
        public void Test6()
        {
            driver.Navigate().GoToUrl(homePageURL);
            IWebElement test6DivContainer = driver.FindElement(By.Id("test-6-div"));

            Assert.AreEqual("Ventosanzap", FindValueInCell(2, 2, driver));

        }

        public string FindValueInCell(int x, int y, IWebDriver driver)
        {
            IWebElement tableBody = driver.FindElement(By.CssSelector("tbody"));
            List<IWebElement> listOfTableRows = tableBody.FindElements(By.CssSelector("tr")).ToList();
            List<IWebElement> tableColumnForYRow = listOfTableRows[y].FindElements(By.CssSelector("td")).ToList();
            return tableColumnForYRow[x].Text;
        }
    }
}