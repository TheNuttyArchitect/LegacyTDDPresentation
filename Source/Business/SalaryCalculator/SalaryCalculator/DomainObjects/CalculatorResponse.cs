﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryCalculator.DomainObjects
{
    public class CalculatorResponse
    {
        public Guid EmployeeId { get; set; }
        public string FullName { get; set; }
        public DateTime PayPeriodBegin { get; set; }
        public DateTime PayPeriodEnd { get; set; }
        public decimal TotalAnnualPay { get; set; }
        public decimal TotalPay { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal TotalTaxes { get; set; }
        public decimal NetPay { get; set; }
        public IList<Deduction> Deductions { get; set; }
        public IList<Tax> Taxes { get; set; }
    }
}
