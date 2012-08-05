using System;

namespace SalaryCalculator.DomainObjects
{
    public class AvailableDeduction
    {
        public DeductionType Type { get; set; }
        public decimal Rate { get; set; }
        public DateTime RateStartDate { get; set; }
        public DateTime RateEndDate { get; set; }
        public DateTime EmployeeDeductionStartDate { get; set; }
        public DateTime EmployeeDeductionEndDate { get; set; }
    }
}