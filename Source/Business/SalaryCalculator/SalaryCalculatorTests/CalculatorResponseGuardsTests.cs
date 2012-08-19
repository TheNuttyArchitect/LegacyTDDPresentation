using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SalaryCalculator;

namespace SalaryCalculatorTests
{
    [TestClass]
    public class CalculatorResponseGuardsTests
    {
        [TestMethod]
        public void AssertInitializeParametersFirstNameThrowsExpectedException()
        {
            AssertHelpers.Throws<ArgumentException>(() => CalculatorResponseGuards.AssertInitializeParameters(null, null, DateTime.MinValue), "Failed null first name check");
            AssertHelpers.Throws<ArgumentException>(() => CalculatorResponseGuards.AssertInitializeParameters("", null, DateTime.MinValue), "Failed empty string first name check");
            AssertHelpers.Throws<ArgumentException>(() => CalculatorResponseGuards.AssertInitializeParameters("  ", null, DateTime.MinValue), "Failed whitespace first name check");
        }

        [TestMethod]
        public void AssertInitializeParametersLastNameThrowsExpectedException()
        {
            AssertHelpers.Throws<ArgumentException>(() => CalculatorResponseGuards.AssertInitializeParameters("Jon", null, DateTime.MinValue), "Failed null last name check");
            AssertHelpers.Throws<ArgumentException>(() => CalculatorResponseGuards.AssertInitializeParameters("Jon", "", DateTime.MinValue), "Failed empty string last name check");
            AssertHelpers.Throws<ArgumentException>(() => CalculatorResponseGuards.AssertInitializeParameters("Jon", "  ", DateTime.MinValue), "Failed whitespace last name check");
        }

        [TestMethod]
        public void AssertInitializeParametersPaydateThrowsExpectedException()
        {
            var minimumYear = DateTime.Now.AddDays(-367);
            var maximumYear = DateTime.Now.AddDays(367);
            AssertHelpers.Throws<ArgumentException>(() => CalculatorResponseGuards.AssertInitializeParameters("Jon", "Doe", minimumYear), "Failed paydate too early check");
            AssertHelpers.Throws<ArgumentException>(() => CalculatorResponseGuards.AssertInitializeParameters("Jon", "Doe", maximumYear), "Failed paydate too late check");
        }

        [TestMethod]
        public void AssertInitializeParametersAcceptsAllValidParameters()
        {
            AssertHelpers.DoesNotThrow(() => CalculatorResponseGuards.AssertInitializeParameters("Jon", "Doe", DateTime.Now.AddYears(-1)));
            AssertHelpers.DoesNotThrow(() => CalculatorResponseGuards.AssertInitializeParameters("Jon", "Doe", DateTime.Now.AddYears(1)));
            AssertHelpers.DoesNotThrow(() => CalculatorResponseGuards.AssertInitializeParameters("Jon", "Doe", DateTime.Now));
        }
    }
}