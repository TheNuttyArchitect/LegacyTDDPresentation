using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SalaryCalculator;
using SalaryCalculator.DomainObjects;

namespace SalaryCalculatorTests
{
    [TestClass]
    public class ServiceTests
    {
        [TestMethod]
        public void CanInvokeService()
        {
            var service = new Service();
            
            var response = service.Invoke("Ben", "Chesnut", DateTime.Parse("9/04/2012"));

            Assert.IsNotNull(response);
            
        }

        
    }
}
