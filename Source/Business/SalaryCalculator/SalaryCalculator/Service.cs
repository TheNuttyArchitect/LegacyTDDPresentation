using System;
using SalaryCalculator.DomainObjects;

namespace SalaryCalculator
{
    /// <summary>
    /// 
    /// </summary>
    public class Service
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="payDate"></param>
        /// <returns></returns>
        public CalculatorResponse Invoke(string firstName, string lastName, DateTime payDate)
        {
            using (ICalculatorService service = CalculatorServiceFactory.CreateCalculatorService(1))
            {
                return service.Invoke(firstName, lastName, payDate);
            }
        }
    }
}
