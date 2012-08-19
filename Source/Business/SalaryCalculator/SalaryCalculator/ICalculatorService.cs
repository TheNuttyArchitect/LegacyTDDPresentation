using System;
using SalaryCalculator.DomainObjects;

namespace SalaryCalculator
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICalculatorService : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="payDate"></param>
        /// <returns></returns>
        CalculatorResponse Invoke(string firstName, string lastName, DateTime payDate);
    }
}