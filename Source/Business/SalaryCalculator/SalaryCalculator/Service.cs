using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalaryCalculator.DomainObjects;

namespace SalaryCalculator
{
    public class Service
    {
        public CalculatorResponse Invoke(string firstName, string lastName, DateTime payDate)
        {
            var calculatorResponse = new CalculatorResponse {FullName = String.Format("{0}, {1}", lastName, firstName)};

            #region Calculate Pay Period
            // Payment is always for period two weeks in arrears from pay date
            calculatorResponse.PayPeriodBegin = payDate.AddDays(-28);
            calculatorResponse.PayPeriodEnd = payDate.AddDays(-14);
            #endregion

            using (var conn = new SqlConnection("Server=localhost;Initial Catalog=Paycheck;Integrated Security=SSPI"))
            {
                using(var cmd = new SqlCommand())
                {
                    #region Get Employee ID
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = 
                        @"Select Top 1 EmployeeId 
                            From dbo.Employee 
                            Where FirstName = @FirstName 
                                And LastName = @LastName";
                    cmd.Parameters.Add(new SqlParameter("@FirstName", SqlDbType.VarChar, 50) {Value = firstName});
                    cmd.Parameters.Add(new SqlParameter("@LastName", SqlDbType.VarChar, 50) { Value = lastName });
                    conn.Open();

                    calculatorResponse.EmployeeId = new Guid(cmd.ExecuteScalar().ToString());
                    #endregion

                    #region Get Employee's current annual salary
                    cmd.Parameters.Clear();
                    cmd.CommandText =
                        @"Select Amount, BeginDate, EndDate
                            From dbo.AnnualSalary
                            Where EmployeeId = @EmployeeId 
                                And BeginDate <= @PayPeriodEnd 
                                And EndDate >= @PayPeriodBegin";
                    cmd.Parameters.Add(new SqlParameter("@EmployeeId", SqlDbType.UniqueIdentifier) {Value = calculatorResponse.EmployeeId});
                    cmd.Parameters.Add(new SqlParameter("@PayPeriodBegin", SqlDbType.SmallDateTime) { Value = calculatorResponse.PayPeriodBegin });
                    cmd.Parameters.Add(new SqlParameter("@PayPeriodEnd", SqlDbType.SmallDateTime) { Value = calculatorResponse.PayPeriodEnd });

                    var salaries = new List<Salary>();
                    using(var reader = cmd.ExecuteReader())
                    {
                        int amountIndex = reader.GetOrdinal("Amount");
                        int beginDateIndex = reader.GetOrdinal("BeginDate");
                        int endDateIndex = reader.GetOrdinal("EndDate");
                        while(reader.Read())
                        {
                            //var annualAmount = reader.GetDouble(amountIndex);
                            //var beginDate = reader.GetDateTime(beginDateIndex);
                            //var endDate = reader.GetDateTime(endDateIndex);
                            var salary = new Salary
                            {
                                AnnualAmount = reader.GetDecimal(amountIndex),
                                BeginDate = reader.GetDateTime(beginDateIndex),
                                EndDate = reader.GetDateTime(endDateIndex),
                            };
                            salaries.Add(salary);
                        }
                    }
                    #endregion

                    #region Calculate Employee's bi-weekly payrate
                    foreach(var salary in salaries)
                    {
                        var rangeBegin = salary.BeginDate < calculatorResponse.PayPeriodBegin
                                             ? calculatorResponse.PayPeriodBegin
                                             : salary.BeginDate;
                        var rangeEnd = salary.EndDate > calculatorResponse.PayPeriodEnd
                                           ? calculatorResponse.PayPeriodEnd
                                           : salary.EndDate;
                        int daysInPeriodSalaryValidFor = (rangeEnd - rangeBegin).Days;
                        decimal biweeklyRate = salary.AnnualAmount/26;
                        decimal periodRate = (biweeklyRate/14*daysInPeriodSalaryValidFor);
                        calculatorResponse.TotalPay += periodRate;
                    }

                    #endregion

                    #region Get Employee's available deductions
                    cmd.CommandText =
                        @"select dt.TypeId, dr.Amount, 
	                        dr.StartDate RateStart, 
	                        dr.EndDate RateEnd, 
	                        a.StartDate EmpStart, 
	                        a.EndDate EmpEnd
                          from dbo.AvailableEmployeeDeduction a
	                        inner join dbo.DeductionType dt on a.DeductionTypeId = dt.TypeId
	                        inner join dbo.DeductionRate dr on dr.DeductionTypeId = dt.TypeId
                          where a.EmployeeId  = @EmployeeId
	                        and dr.StartDate <= @PayPeriodEnd
	                        and dr.EndDate >= @PayPeriodBegin
	                        and a.StartDate <= @PayPeriodEnd 
                            and a.EndDate >= @PayPeriodBegin";

                    var availableDeductions = new List<AvailableDeduction>();
                    using(var reader = cmd.ExecuteReader())
                    {
                        int typeIdOrd = reader.GetOrdinal("TypeId");
                        int amountOrd = reader.GetOrdinal("Amount");
                        int rateStartOrd = reader.GetOrdinal("RateStart");
                        int rateEndOrd = reader.GetOrdinal("RateEnd");
                        int empStartOrd = reader.GetOrdinal("EmpStart");
                        int empEndOrd = reader.GetOrdinal("EmpEnd");

                        while(reader.Read())
                        {
                            var availableDeduction = new AvailableDeduction
                            {
                                Type = (DeductionType) reader.GetInt32(typeIdOrd),
                                Rate = reader.GetDecimal(amountOrd),
                                RateStartDate = reader.GetDateTime(rateStartOrd),
                                RateEndDate = reader.GetDateTime(rateEndOrd),
                                EmployeeDeductionStartDate = reader.GetDateTime(empStartOrd),
                                EmployeeDeductionEndDate = reader.GetDateTime(empEndOrd),
                            };
                            availableDeductions.Add(availableDeduction);
                        }
                    }
                    #endregion

                    #region Apply Employee Deductions
                    foreach(var availableDeduction in availableDeductions)
                    {
                        /*
                         *  RateStart = 8/1/2012
                         *  RateEnd = 7/31/2013
                         *  EmpStart = 7/24/2012
                         *  EmpEnd = 7/23/2013
                         *  PayStart = 7/15/2012
                         *  PayEnd = 8/14/2012
                         *  
                         * RangeStart:
                         *      When RateStart <= PayStart && EmpStart <= PayStart Then PayStart
                         *      Else When RateStart > PayStart && EmpStart > RateStart Then RateStart
                         *      Else When EmpStart > PayStart Then EmpStart
                         * 
                         * RangeEnd:
                         *      When RateEnd >= PayEnd && EmpEnd >= PayEnd Then PayEnd
                         *      Else When RateEnd < PayEnd && EmpEnd > RateEnd Then RateEnd
                         *      Else EmpEnd
                         */
                        DateTime rangeBegin, rangeEnd;
                        if(availableDeduction.RateStartDate <= calculatorResponse.PayPeriodBegin && 
                            availableDeduction.EmployeeDeductionStartDate <= calculatorResponse.PayPeriodBegin)
                        {
                            rangeBegin = calculatorResponse.PayPeriodBegin;
                        }
                        else if(availableDeduction.RateStartDate > calculatorResponse.PayPeriodBegin &&
                             availableDeduction.EmployeeDeductionStartDate > availableDeduction.RateStartDate)
                        {
                            rangeBegin = availableDeduction.RateStartDate;
                        }
                        else
                        {
                            rangeBegin = availableDeduction.EmployeeDeductionStartDate;
                        }

                        if(availableDeduction.RateEndDate >= calculatorResponse.PayPeriodEnd &&
                            availableDeduction.EmployeeDeductionEndDate >= calculatorResponse.PayPeriodEnd)
                        {
                            rangeEnd = calculatorResponse.PayPeriodEnd;
                        }
                        else if(availableDeduction.RateEndDate < calculatorResponse.PayPeriodEnd &&
                            availableDeduction.EmployeeDeductionEndDate > availableDeduction.RateEndDate)
                        {
                            rangeEnd = availableDeduction.RateEndDate;
                        }
                        else
                        {
                            rangeEnd = availableDeduction.EmployeeDeductionEndDate;
                        }

                        int daysInPeriodDeductionValidFor = (rangeEnd - rangeBegin).Days;
                        decimal biweeklyDeduction = availableDeduction.Rate / 26;
                        decimal periodDeduction = (biweeklyDeduction / 14 * daysInPeriodDeductionValidFor);

                        Deduction deduction = calculatorResponse.Deductions.FirstOrDefault(d => d.DebitType == availableDeduction.Type);
                        if(deduction != null)
                        {
                            deduction.Amount += periodDeduction;
                        }
                        else
                        {
                            deduction = new Deduction
                            {
                                Amount = periodDeduction,
                                DebitType = availableDeduction.Type
                            };
                            calculatorResponse.Deductions.Add(deduction);    
                        }
                        
                    }
                    #endregion

                    #region Apply Employee Taxes

                    #endregion

                    #region Get Prior Paychecks for year

                    #endregion

                    #region Calculate Paycheck Totals

                    //calculatorResponse.TotalDeductions = calculatorResponse.Deductions.Sum(d => d.Amount);

                    #endregion

                    #region Save Paycheck to database

                    #endregion
                }
            }

            return calculatorResponse;
        }
    }
}
