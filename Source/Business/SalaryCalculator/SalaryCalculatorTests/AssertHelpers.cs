using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SalaryCalculatorTests
{
    public static class AssertHelpers
    {
        public static void Throws<T>(Action methodPointer, string errorMessage = null) where T : Exception
        {
            T expected = null;

            try
            {
                methodPointer.Invoke();
            }
            catch (T thrown)
            {
                expected = thrown;
            }

            if (errorMessage != null)
            {
                Assert.IsNotNull(expected, errorMessage);
            }
            else
            {
                Assert.IsNotNull(expected);
            }
        }

        public static void DoesNotThrow<T>(Action methodPointer, string errorMessage = null) where T : Exception
        {
            T expected = null;

            try
            {
                methodPointer.Invoke();
            }
            catch (T thrown)
            {
                expected = thrown;
            }

            if (errorMessage != null)
            {
                Assert.IsNull(expected, errorMessage);
            }
            else
            {
                Assert.IsNull(expected);
            }
        }

        public static void DoesNotThrow(Action methodPointer, string errorMessage = null)
        {
            DoesNotThrow<Exception>(methodPointer, errorMessage);
        }
    }
}