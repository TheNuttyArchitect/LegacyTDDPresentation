using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SalaryCalculator;

namespace SalaryCalculatorTests
{
    [TestClass]
    public class CalculatorServiceTests
    {
        [TestMethod]
        public void InitializeResponseInitializesAsExpected()
        {
            var expectedPayDate = DateTime.Now.AddDays(-2);
            var periodBeginDate = expectedPayDate.AddDays(-28);
            var periodEndDate = expectedPayDate.AddDays(-14);
            
            var service = new CalculatorService();
            var response = service.InitializeResponse("Jon", "Doe", expectedPayDate);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Deductions);
            Assert.IsNotNull(response.Taxes);
            Assert.AreEqual(response.FullName, "Doe, Jon");
            Assert.AreEqual(response.PayDate, expectedPayDate);
            Assert.AreEqual(response.PayPeriodBegin, periodBeginDate);
            Assert.AreEqual(response.PayPeriodEnd, periodEndDate);
        }   

        [TestMethod]
        public void InitializeResponseFailsWithBadParameters()
        {
            var service = new CalculatorService();
            AssertHelpers.Throws<ArgumentException>(() => service.InitializeResponse(null, null, DateTime.MinValue));
        }
    }
}