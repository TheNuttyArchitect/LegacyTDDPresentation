using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SalaryCalculator;
using SalaryCalculator.Fakes;

namespace SalaryCalculatorTests
{
    [TestClass]
    public class CalculatorServiceTests
    {
        [TestMethod]
        public void InitializeResponseInitializesAsExpected()
        {
            var expectedEmployeeId = Guid.NewGuid();
            var expectedPayDate = DateTime.Now.AddDays(-2);
            var periodBeginDate = expectedPayDate.AddDays(-28);
            var periodEndDate = expectedPayDate.AddDays(-14);
            
            
            var repo = new StubICalculatorRepository();
            repo.GetEmployeeIdStringString = (fn, ln) => expectedEmployeeId;
            var service = new CalculatorService
                {
                    Repository = repo
                };

            var response = service.InitializeResponse("Jon", "Doe", expectedPayDate);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Deductions);
            Assert.IsNotNull(response.Taxes);
            Assert.AreEqual(response.EmployeeId, expectedEmployeeId);
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