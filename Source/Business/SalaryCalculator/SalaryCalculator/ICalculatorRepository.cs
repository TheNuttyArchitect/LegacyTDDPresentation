using System;
using System.Collections.Generic;
using SalaryCalculator.DomainObjects;

namespace SalaryCalculator
{
    public interface ICalculatorRepository
    {
        Guid GetEmployeeId(string firstName, string lastName);
        IEnumerable<Salary> GetSalaryInformation(Guid employeeId, DateTime payPeriodBegin, DateTime payPeriodEnd);
        IEnumerable<AvailableDeduction> GetAvailableDeductions(Guid employeeId, DateTime payPeriodBegin, DateTime payPeriodEnd);
        IEnumerable<TaxRate> GetPeriodTaxRates(decimal annualSalary, DateTime payPeriodBegin, DateTime payPeriodEnd);
        YearToDateFinancials GetYearToDateFinancials(Guid employeeId, DateTime payDate);
        void SavePaycheck(Guid paymentId, Guid employeeId, DateTime payDate, decimal totalAmount, decimal totalDeductions, decimal totalTaxes);
        void SaveDeduction(Guid paymentId, decimal amount, DeductionType deductionType);
        void SaveTax(Guid paymentId, Decimal amount, TaxType taxType);
    }
}