using System;
using System.Collections.Generic;
using SalaryCalculator.DomainObjects;

namespace SalaryCalculator
{
    public interface ICalculatorRepository : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        Guid GetEmployeeId(string firstName, string lastName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="payPeriodBegin"></param>
        /// <param name="payPeriodEnd"></param>
        /// <param name="annualSalary"> </param>
        /// <returns></returns>
        IEnumerable<Salary> GetSalaryInformation(Guid employeeId, DateTime payPeriodBegin, DateTime payPeriodEnd, out decimal annualSalary);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="payPeriodBegin"></param>
        /// <param name="payPeriodEnd"></param>
        /// <returns></returns>
        IEnumerable<AvailableDeduction> GetAvailableDeductions(Guid employeeId, DateTime payPeriodBegin, DateTime payPeriodEnd);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="annualSalary"></param>
        /// <param name="payPeriodBegin"></param>
        /// <param name="payPeriodEnd"></param>
        /// <returns></returns>
        IEnumerable<TaxRate> GetPeriodTaxRates(decimal annualSalary, DateTime payPeriodBegin, DateTime payPeriodEnd);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="payDate"></param>
        /// <returns></returns>
        YearToDateFinancials GetYearToDateFinancials(Guid employeeId, DateTime payDate);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paymentId"></param>
        /// <param name="employeeId"></param>
        /// <param name="payDate"></param>
        /// <param name="totalAmount"></param>
        /// <param name="totalDeductions"></param>
        /// <param name="totalTaxes"></param>
        void SavePaycheck(Guid paymentId, Guid employeeId, DateTime payDate, decimal totalAmount, decimal totalDeductions, decimal totalTaxes);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paymentId"></param>
        /// <param name="amount"></param>
        /// <param name="deductionType"></param>
        void SaveDeduction(Guid paymentId, decimal amount, DeductionType deductionType);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paymentId"></param>
        /// <param name="amount"></param>
        /// <param name="taxType"></param>
        void SaveTax(Guid paymentId, Decimal amount, TaxType taxType);
    }
}