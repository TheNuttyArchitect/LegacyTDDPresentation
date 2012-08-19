namespace SalaryCalculator
{
    /// <summary>
    /// 
    /// </summary>
    public class SqlStatements
    {
        public const string GetEmployeeId = 
            @"Select Top 1 EmployeeId 
                From dbo.Employee with (nolock)
                Where FirstName = @FirstName 
                    And LastName = @LastName";

        public const string GetSalaryInformation =
            @"Select Amount, BeginDate, EndDate
                From dbo.AnnualSalary with (nolock)
                Where EmployeeId = @EmployeeId 
                    And BeginDate <= @PayPeriodEnd 
                    And EndDate >= @PayPeriodBegin";

        public const string GetAvailableDeductions =
            @"select dt.TypeId, dr.Amount, 
	            dr.StartDate RateStart, 
	            dr.EndDate RateEnd, 
	            a.StartDate EmpStart, 
	            a.EndDate EmpEnd
                from dbo.AvailableEmployeeDeduction a with (nolock)
	                inner join dbo.DeductionType dt with (nolock) on a.DeductionTypeId = dt.TypeId
	                inner join dbo.DeductionRate dr with (nolock) on dr.DeductionTypeId = dt.TypeId
                where a.EmployeeId  = @EmployeeId
	                and dr.StartDate <= @PayPeriodEnd
	                and dr.EndDate >= @PayPeriodBegin
	                and a.StartDate <= @PayPeriodEnd 
                    and a.EndDate >= @PayPeriodBegin";

        public const string GetPeriodTaxRates =
            @"select tr.TaxTypeId, tr.Rate, tr.StartDate, tr.EndDate
                from dbo.TaxRate tr with (nolock)
                where tr.StartDate <= @PayPeriodBegin
                    and tr.EndDate >= @PayPeriodEnd
	                and ((@Salary between tr.MinimumSalary and tr.MaximumSalary)
		                or (tr.MinimumSalary is null and tr.MaximumSalary is null)
		                or (tr.MinimumSalary < @Salary and tr.MaximumSalary is null)
		                or (tr.MinimumSalary is null and tr.MaximumSalary > @Salary))";

        public const string GetYearToDateFinancials =
            @"select sum(TotalAmount) TotalYTD, sum(TotalDeductions) DeductionsYTD, sum(TotalTaxes) TaxesYTD
                from dbo.Payment  
                where EmployeeID = @EmployeeID
                    and Paydate < @PayDate";

        public const string SavePaycheck =
            @"insert into dbo.Payment
		        (PaymentId, EmployeeId, Paydate, TotalAmount, TotalDeductions, TotalTaxes)
	          values
		        (@PaymentId, @EmployeeId, @Paydate, @TotalAmount, @TotalDeductions, @TotalTaxes)";

        public const string SaveDeduction =
            @"insert into dbo.Deduction
	            (PaymentId, Amount, [Type])
              values
                (@PaymentId, @Amount, @Type)";

        public const string SaveTax =
            @"insert into dbo.Tax
	            (PaymentId, Amount, [Type])
              values
                (@PaymentId, @Amount, @Type)";
    }
}