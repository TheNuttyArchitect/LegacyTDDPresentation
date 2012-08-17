namespace SalaryCalculator.DomainObjects
{
    public class YearToDateFinancials
    {
        public decimal TotalPayYearToDate { get; set; }
        public decimal TotalDeductionsYearToDate { get; set; }
        public decimal TotalTaxesYearToDate { get; set; }
    }
}