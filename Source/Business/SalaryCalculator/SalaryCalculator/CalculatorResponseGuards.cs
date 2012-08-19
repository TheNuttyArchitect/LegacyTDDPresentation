using System;

namespace SalaryCalculator
{
    /// <summary>
    /// 
    /// </summary>
    internal class CalculatorResponseGuards
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="payDate"></param>
        internal static void AssertInitializeParameters(string firstName, string lastName, DateTime payDate)
        {
            if(String.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentException("firstName must be provided");
            }

            if(String.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentException("lastName must be provided");
            }

            var minimumPayDate = DateTime.Now.AddYears(-1);
            var maximumPayDate = DateTime.Now.AddYears(1);
            if(payDate < minimumPayDate || payDate > maximumPayDate)
            {
                throw new ArgumentException("payDate must be within a year of the current date to run this service");
            }
        }
    }
}