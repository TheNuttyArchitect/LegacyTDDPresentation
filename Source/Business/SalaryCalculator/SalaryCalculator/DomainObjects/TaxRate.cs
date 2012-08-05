using System;

namespace SalaryCalculator.DomainObjects
{
    public class TaxRate
    {
        public TaxType Type { get; set; }
        public decimal Rate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}