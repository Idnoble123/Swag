
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;


namespace SauceDemoTest
{
    public class SauceDemoTests
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddArgument("incognito");
            driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(500);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(500));
        }

        [Test]
        public void BuyTShirtTest()
        {
            // 1. Navigate to the Sauce Labs Sample Application
            driver.Navigate().GoToUrl("https://www.saucedemo.com/");

            // 2. Enter valid credentials to log in
            driver.FindElement(By.Id("user-name")).SendKeys("standard_user");
            driver.FindElement(By.Id("password")).SendKeys("secret_sauce");
            driver.FindElement(By.Id("login-button")).Click();

            // 3. Verify that the login is successful and the user is redirected to the products page
            Assert.IsTrue(driver.Url.Contains("inventory.html"));

            // 4. Select a T-shirt by clicking on its image or name
            driver.FindElement(By.Id("item_1_title_link")).Click();

            // 5. Verify that the T-shirt details page is displayed
            Assert.IsTrue(driver.Url.Contains("inventory-item.html?id=1"));

            // 6. Click the "Add to Cart" button using an explicit wait
            var addToCartButton = WaitForElementToBeClickable(By.Id("add-to-cart"));
            addToCartButton.Click();

            // 7. Verify that the T-shirt is added to the cart successfully
            IWebElement cartBadge = WaitForElementToBeVisible(By.ClassName("shopping_cart_badge"));
            Assert.IsNotNull(cartBadge);

            // 8. Navigate to the cart by clicking the cart icon
            driver.FindElement(By.ClassName("shopping_cart_link")).Click();

            // 9. Verify that the cart page is displayed
            Assert.IsTrue(driver.Url.Contains("cart.html"));

            // 10. Review the items in the cart and ensure that the T-shirt is listed with the correct details
            Assert.IsTrue(driver.FindElement(By.ClassName("inventory_item_name")).Text.Contains("Bolt T-Shirt"));

            // 11. Click the "Checkout" button
            driver.FindElement(By.Id("checkout")).Click();

            // 12. Verify that the checkout information page is displayed
            Assert.IsTrue(driver.Url.Contains("checkout-step-one.html"));

            // 13. Enter the required checkout information
            driver.FindElement(By.Id("first-name")).SendKeys("John");
            driver.FindElement(By.Id("last-name")).SendKeys("Abdul");
            driver.FindElement(By.Id("postal-code")).SendKeys("12345");
            driver.FindElement(By.Id("continue")).Click();

            // 14. Verify that the order summary page is displayed
            Assert.IsTrue(driver.Url.Contains("checkout-step-two.html"));

            // 15. Click the "Finish" button
            driver.FindElement(By.Id("finish")).Click();

            // 16. Verify that the order confirmation page is displayed
            Assert.IsTrue(driver.Url.Contains("checkout-complete.html"));

            // 17. Logout from the application
            driver.FindElement(By.Id("react-burger-menu-btn")).Click();
            driver.FindElement(By.Id("logout_sidebar_link")).Click();

            // 18. Verify that the user is successfully logged out and redirected to the login page
            Assert.IsTrue(driver.Url.Contains("https://www.saucedemo.com/"));
        }

        private IWebElement WaitForElementToBeClickable(By by)
        {
            return wait.Until(ExpectedConditions.ElementToBeClickable(by));
        }

        private IWebElement WaitForElementToBeVisible(By by)
        {
            return wait.Until(ExpectedConditions.ElementIsVisible(by));
        }

        [TearDown]
        public void TearDown()
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
                driver = null;
            }
        }
    }

    public static class ExpectedConditions
    {
        public static Func<IWebDriver, IWebElement> ElementToBeClickable(By locator)
        {
            return driver =>
            {
                try
                {
                    var element = driver.FindElement(locator);
                    return (element != null && element.Enabled) ? element : null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
            };
        }

        public static Func<IWebDriver, IWebElement> ElementIsVisible(By locator)
        {
            return driver =>
            {
                try
                {
                    var element = driver.FindElement(locator);
                    return (element != null && element.Displayed) ? element : null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
            };
        }
    }
}


