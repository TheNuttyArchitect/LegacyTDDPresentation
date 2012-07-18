using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryCalculator.DomainObjects
{
    public class CalculatorResponse
    {
        public string FullName { get; set; }
        public string FriendlyPayDate { get; set; }
        public double TotalAnnualPay { get; set; }
        public double TotalPay { get; set; }
        public double TotalDeductions { get; set; }
        public double TotalTaxes { get; set; }
        public double NetPay { get; set; }
        public IList<Deduction> Deductions { get; set; }
        public IList<Tax> Taxes { get; set; }
    }
}
