using Microsoft.VisualStudio.TestTools.UnitTesting;
using SalaryCalculator;

namespace SalaryCalculatorTests
{
    [TestClass]
    public class CalculatorServiceFactoryTests
    {
        [TestMethod]
        public void DefaultDefaultsToNewService()
        {
            ICalculatorService service = CalculatorServiceFactory.CreateCalculatorService();
            Assert.IsInstanceOfType(service, typeof (CalculatorService));
        }

        [TestMethod]
        public void ReturnsNewServiceA2PassedInForVersion()
        {
            ICalculatorService service = CalculatorServiceFactory.CreateCalculatorService(2);
            Assert.IsInstanceOfType(service, typeof(CalculatorService));
        }

        [TestMethod]
        public void ReturnsLegacyServiceWithA1PassedInForVersion()
        {
            ICalculatorService service = CalculatorServiceFactory.CreateCalculatorService(1);
            Assert.IsInstanceOfType(service, typeof(LegacyCalculatorService));
        }

        [TestMethod]
        public void ThrowsInvalidVersionExceptionWithBadVersionNumbers()
        {
            AssertInvalidVersionExceptionThrown(0);
            AssertInvalidVersionExceptionThrown(-2);
            AssertInvalidVersionExceptionThrown(3);
        }

        private void AssertInvalidVersionExceptionThrown(short versionNumber)
        {
            AssertHelpers.Throws<InvalidVersionException>
                (
                    () => CalculatorServiceFactory.CreateCalculatorService(versionNumber),
                    string.Format("Version #{0} was expected to be invalid, but was not treated that way.", versionNumber)
                );
        }
    }
}