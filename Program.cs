using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;

namespace RegularFormAutomationTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Selenium test for regular HTML form");

            var options = new FirefoxOptions();

            IWebDriver driver = new FirefoxDriver(options);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            try
            {
                driver.Navigate().GoToUrl("https://app.cloudqa.io/home/AutomationPracticeForm");

                wait.Until(d => d.FindElement(By.Id("fname")));

                Console.WriteLine("Page loaded, starting tests...");

                // Fill in all the form fields
                FillContactInfo(driver, wait);
                FillPersonalInfo(driver, wait);
                SelectHobbies(driver, wait);
                AgreeToTerms(driver);

                // Submit the form
                SubmitForm(driver, wait);

                // Wait to see results
                Thread.Sleep(3000);

                VerifySubmission(driver, wait);

                Console.WriteLine("All tests completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed with error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
            finally
            {
                Console.WriteLine("Press any key to close the browser and exit...");
                Console.ReadKey();
                // Close the browser
                driver.Quit();
            }
        }

        static void FillContactInfo(IWebDriver driver, WebDriverWait wait)
        {
            Console.WriteLine("Filling contact information...");

            try
            {
                // First Name
                IWebElement fnameElement = wait.Until(d => d.FindElement(By.Id("fname")));
                fnameElement.Clear();
                fnameElement.SendKeys("John");
                Console.WriteLine("✓ First Name entered");

                // Last Name
                IWebElement lnameElement = wait.Until(d => d.FindElement(By.Id("lname")));
                lnameElement.Clear();
                lnameElement.SendKeys("Doe");
                Console.WriteLine("✓ Last Name entered");

                // Email
                IWebElement emailElement = wait.Until(d => d.FindElement(By.Id("email")));
                emailElement.Clear();
                emailElement.SendKeys("john.doe@example.com");
                Console.WriteLine("✓ Email entered");

                // Mobile Number
                IWebElement mobileElement = wait.Until(d => d.FindElement(By.Id("mobile")));
                mobileElement.Clear();
                mobileElement.SendKeys("1234567890");
                Console.WriteLine("✓ Mobile number entered");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error filling contact information: {ex.Message}");
                throw;
            }
        }

        static void FillPersonalInfo(IWebDriver driver, WebDriverWait wait)
        {
            Console.WriteLine("Filling personal information...");

            try
            {
                // Gender - select Male
                IWebElement maleRadio = wait.Until(d => d.FindElement(By.Id("male")));
                if (!maleRadio.Selected)
                {
                    maleRadio.Click();
                }
                Console.WriteLine("✓ Gender selected: Male");

                IWebElement dobElement = wait.Until(d => d.FindElement(By.Id("dob")));
                dobElement.Clear();
                dobElement.SendKeys("1990-01-15");
                Console.WriteLine("✓ Date of Birth entered");

                // Country
                IWebElement countryElement = wait.Until(d => d.FindElement(By.Id("country")));
                countryElement.Clear();
                countryElement.SendKeys("United States");
                Console.WriteLine("✓ Country entered");

                // State
                SelectElement stateDropdown = new SelectElement(driver.FindElement(By.Id("state")));
                stateDropdown.SelectByValue("United States");
                Console.WriteLine("✓ State selected: United States");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error filling personal information: {ex.Message}");
                throw;
            }
        }

        static void SelectHobbies(IWebDriver driver, WebDriverWait wait)
        {
            Console.WriteLine("Selecting hobbies...");

            try
            {
                // Select Reading and Dance hobbies
                string[] hobbiesToSelect = { "Reading", "Dance" };

                foreach (string hobby in hobbiesToSelect)
                {
                    IWebElement hobbyCheckbox = wait.Until(d => d.FindElement(By.Id(hobby)));
                    if (!hobbyCheckbox.Selected)
                    {
                        hobbyCheckbox.Click();
                    }
                    Console.WriteLine($"✓ Hobby selected: {hobby}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error selecting hobbies: {ex.Message}");
                throw;
            }
        }

        static void AgreeToTerms(IWebDriver driver)
        {
            Console.WriteLine("Agreeing to terms...");

            try
            {
                // Locate the checkbox and click to agree
                IWebElement agreeCheckbox = driver.FindElement(By.Id("Agree"));
                if (!agreeCheckbox.Selected)
                {
                    agreeCheckbox.Click();
                }
                Console.WriteLine("✓ Agreed to terms");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error agreeing to terms: {ex.Message}");
                throw;
            }
        }

        static void SubmitForm(IWebDriver driver, WebDriverWait wait)
        {
            Console.WriteLine("Submitting the form...");

            try
            {
                // Find and click the submit button
                IWebElement submitButton = wait.Until(d => d.FindElement(By.CssSelector("#automationtestform button[type='submit']")));
                submitButton.Click();
                Console.WriteLine("✓ Form submitted");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error submitting form: {ex.Message}");
                throw;
            }
        }

        static void VerifySubmission(IWebDriver driver, WebDriverWait wait)
        {
            Console.WriteLine("Verifying form submission...");

            try
            {
                // Check for success message or any verification logic
                IWebElement successMessage = wait.Until(d => d.FindElement(By.CssSelector(".success-message")));
                if (successMessage.Displayed)
                {
                    Console.WriteLine($"✓ Form submission verified: Success message found - {successMessage.Text}");
                    return;
                }

                string currentUrl = driver.Url;
                if (currentUrl.Contains("success") || currentUrl.Contains("thank-you"))
                {
                    Console.WriteLine($"✓ Form submission verified: Redirected to {currentUrl}");
                    return;
                }

                // If verification fails, log a warning
                Console.WriteLine("! Warning: Could not definitively verify form submission, but no errors occurred");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error verifying submission: {ex.Message}");
                throw;
            }
        }
    }
}