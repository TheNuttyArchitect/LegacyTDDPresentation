using System;

namespace SalaryCalculator.DomainObjects
{
    public class Salary
    {
        public decimal AnnualAmount { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}