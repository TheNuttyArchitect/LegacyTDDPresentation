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
            
            //var response = service.Invoke("Ben", "Chesnut", DateTime.Parse("9/07/2012"));
            //var response = service.Invoke("Ben", "Chesnut", DateTime.Parse("8/17/2012"));

            //Assert.IsNotNull(response);
            
        }

        //[TestMethod]
        //public void PrepopulateData()
        //{
        //    DateTime testPayDate = DateTime.Parse("9/07/2012");
        //    DateTime payDate = DateTime.Parse("1/6/2012");
        //    var service = new Service();

        //    while (payDate < testPayDate)
        //    {
        //        service.Invoke("Ben", "Chesnut", payDate);
        //        payDate = payDate.AddDays(14);
        //    }
        //}
        
    }
}
