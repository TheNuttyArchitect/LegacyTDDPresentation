using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Linq;
using Microsoft.QualityTools.Testing.Fakes;
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
                var response = service.Invoke("Ben", "Chesnut", DateTime.Parse("9/07/2012"));

                Assert.IsNotNull(response);
                Assert.AreEqual(response.FullName, "Chesnut, Ben");
                Assert.IsTrue(response.TotalPay > 0);
                Assert.IsTrue(response.TotalTaxes > 0);
                Assert.IsTrue(response.TotalDeductions > 0);
                Assert.IsTrue(response.TotalPayYearToDate > response.TotalPay);
                Assert.IsTrue(response.TotalTaxesYearToDate > response.TotalTaxes);
                Assert.IsTrue(response.TotalDeductionsYearToDate > response.TotalDeductions);
                Assert.IsTrue(response.Taxes.Any());
                Assert.IsTrue(response.Deductions.Any());
                Assert.IsTrue(!response.EmployeeId.Equals(Guid.Empty));
                Assert.IsTrue(!response.PaymentId.Equals(Guid.Empty));
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
