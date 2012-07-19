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

                    #region Apply Employee Deductions

                    #endregion

                    #region Apply Employee Taxes

                    #endregion

                    #region Calculate Paycheck Totals

                    #endregion

                    #region Save Paycheck to database

                    #endregion
                }
            }

            return calculatorResponse;
        }
    }
}
