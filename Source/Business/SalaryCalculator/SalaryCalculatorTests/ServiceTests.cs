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
            NotImplementedException nex = null;
            try
            {
                service.Invoke("Ben", "Chesnut", DateTime.Now);
            }
            catch (NotImplementedException nie)
            {
                nex = nie;
            }

            Assert.IsTrue(nex != null);
        }

        
    }
}
