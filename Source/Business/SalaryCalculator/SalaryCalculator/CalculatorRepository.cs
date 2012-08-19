using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using SalaryCalculator.DomainObjects;

namespace SalaryCalculator
{
    /// <summary>
    /// 
    /// </summary>
    public class CalculatorRepository : ICalculatorRepository
    {
        private bool _disposed = false;
        private DataManager _dataManager = null;

        /// <summary>
        /// 
        /// </summary>
        public CalculatorRepository()
        {
            _dataManager = new DataManager();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        public Guid GetEmployeeId(string firstName, string lastName)
        {
            return new Guid
                (
                    _dataManager.ExecuteScalar
                        (
                            SqlStatements.GetEmployeeId,
                            new SqlParameter("@FirstName", SqlDbType.VarChar, 50) { Value = firstName },
                            new SqlParameter("@LastName", SqlDbType.VarChar, 50) { Value = lastName }
                        ).ToString()
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="payPeriodBegin"></param>
        /// <param name="payPeriodEnd"></param>
        /// <returns></returns>
        public IEnumerable<Salary> GetSalaryInformation(Guid employeeId, DateTime payPeriodBegin, DateTime payPeriodEnd)
        {
            decimal annualSalary = 0;
            var salaries = new List<Salary>();
            using 
                (
                    var reader = _dataManager.ExecuteReader
                        (
                            SqlStatements.GetSalaryInformation,
                            new SqlParameter("@EmployeeId", SqlDbType.UniqueIdentifier) {Value = employeeId},
                            new SqlParameter("@PayPeriodBegin", SqlDbType.SmallDateTime) { Value = payPeriodBegin },
                            new SqlParameter("@PayPeriodEnd", SqlDbType.SmallDateTime) { Value = payPeriodEnd }
                        )
                )
            {
                int amountIndex = reader.GetOrdinal("Amount");
                int beginDateIndex = reader.GetOrdinal("BeginDate");
                int endDateIndex = reader.GetOrdinal("EndDate");
                while (reader.Read())
                {
                    var salary = new Salary
                    {
                        AnnualAmount = reader.GetDecimal(amountIndex),
                        BeginDate = reader.GetDateTime(beginDateIndex),
                        EndDate = reader.GetDateTime(endDateIndex),
                    };
                    if (salary.AnnualAmount > annualSalary)
                        annualSalary = salary.AnnualAmount;
                    salaries.Add(salary);
                }
            }

            return salaries;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="payPeriodBegin"></param>
        /// <param name="payPeriodEnd"></param>
        /// <returns></returns>
        public IEnumerable<AvailableDeduction> GetAvailableDeductions(Guid employeeId, DateTime payPeriodBegin, DateTime payPeriodEnd)
        {
            var availableDeductions = new List<AvailableDeduction>();
            using
                (
                    var reader = _dataManager.ExecuteReader
                        (
                            SqlStatements.GetAvailableDeductions,
                            new SqlParameter("@EmployeeId", SqlDbType.UniqueIdentifier) { Value = employeeId },
                            new SqlParameter("@PayPeriodBegin", SqlDbType.SmallDateTime) { Value = payPeriodBegin },
                            new SqlParameter("@PayPeriodEnd", SqlDbType.SmallDateTime) { Value = payPeriodEnd }
                        )
                )
            {
                int typeIdOrd = reader.GetOrdinal("TypeId");
                int amountOrd = reader.GetOrdinal("Amount");
                int rateStartOrd = reader.GetOrdinal("RateStart");
                int rateEndOrd = reader.GetOrdinal("RateEnd");
                int empStartOrd = reader.GetOrdinal("EmpStart");
                int empEndOrd = reader.GetOrdinal("EmpEnd");

                while (reader.Read())
                {
                    var availableDeduction = new AvailableDeduction
                    {
                        Type = (DeductionType)reader.GetInt32(typeIdOrd),
                        Rate = reader.GetDecimal(amountOrd),
                        RateStartDate = reader.GetDateTime(rateStartOrd),
                        RateEndDate = reader.GetDateTime(rateEndOrd),
                        EmployeeDeductionStartDate = reader.GetDateTime(empStartOrd),
                        EmployeeDeductionEndDate = reader.GetDateTime(empEndOrd),
                    };
                    availableDeductions.Add(availableDeduction);
                }
            }

            return availableDeductions;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="annualSalary"></param>
        /// <param name="payPeriodBegin"></param>
        /// <param name="payPeriodEnd"></param>
        /// <returns></returns>
        public IEnumerable<TaxRate> GetPeriodTaxRates(decimal annualSalary, DateTime payPeriodBegin, DateTime payPeriodEnd)
        {
            var taxRates = new List<TaxRate>();
            using
                (
                    var reader = _dataManager.ExecuteReader
                        (
                            SqlStatements.GetPeriodTaxRates,
                            new SqlParameter("@Salary", SqlDbType.Decimal) { Value = annualSalary},
                            new SqlParameter("@PayPeriodBegin", SqlDbType.SmallDateTime) { Value = payPeriodBegin },
                            new SqlParameter("@PayPeriodEnd", SqlDbType.SmallDateTime) { Value = payPeriodEnd }
                        )
                )
            {
                int taxTypeOrd = reader.GetOrdinal("TaxTypeId");
                int rateOrd = reader.GetOrdinal("Rate");
                int startDateOrd = reader.GetOrdinal("StartDate");
                int endDateOrd = reader.GetOrdinal("EndDate");

                while (reader.Read())
                {
                    var taxRate = new TaxRate
                    {
                        Type = (TaxType)reader.GetInt32(taxTypeOrd),
                        Rate = reader.GetDecimal(rateOrd),
                        StartDate = reader.GetDateTime(startDateOrd),
                        EndDate = reader.GetDateTime(endDateOrd)
                    };
                    taxRates.Add(taxRate);
                }
            }

            return taxRates;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="payDate"></param>
        /// <returns></returns>
        public YearToDateFinancials GetYearToDateFinancials(Guid employeeId, DateTime payDate)
        {
            YearToDateFinancials ytdFinancials = null;

            using
                (
                    var reader = _dataManager.ExecuteReader
                        (
                            SqlStatements.GetYearToDateFinancials,
                            new SqlParameter("@EmployeeId", SqlDbType.UniqueIdentifier) { Value = employeeId },
                            new SqlParameter("@Paydate", SqlDbType.SmallDateTime) { Value = payDate }
                        )
                )
            {
                int totalYTDOrd = reader.GetOrdinal("TotalYTD");
                int deductionsYTDOrd = reader.GetOrdinal("DeductionsYTD");
                int taxesYTDOrd = reader.GetOrdinal("TaxesYTD");

                while(reader.Read())
                {
                    if(ytdFinancials != null)
                    {
                        throw new InvalidDataException("There can not be more than one row of prior paycheck information.");
                    }

                    ytdFinancials = new YearToDateFinancials
                        {
                            TotalPayYearToDate = reader.GetDecimal(totalYTDOrd),
                            TotalDeductionsYearToDate = reader.GetDecimal(deductionsYTDOrd),
                            TotalTaxesYearToDate = reader.GetDecimal(taxesYTDOrd)
                        };
                }
            }

            return ytdFinancials;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paymentId"></param>
        /// <param name="employeeId"></param>
        /// <param name="payDate"></param>
        /// <param name="totalAmount"></param>
        /// <param name="totalDeductions"></param>
        /// <param name="totalTaxes"></param>
        public void SavePaycheck(Guid paymentId, Guid employeeId, DateTime payDate, decimal totalAmount, decimal totalDeductions, decimal totalTaxes)
        {
            _dataManager.ExecuteNonQuery
                (
                    SqlStatements.SavePaycheck,
                    new SqlParameter("@PaymentId", SqlDbType.UniqueIdentifier) {Value = paymentId},
                    new SqlParameter("@EmployeeId", SqlDbType.UniqueIdentifier) { Value = employeeId },
                    new SqlParameter("@Paydate", SqlDbType.SmallDateTime) { Value = payDate },
                    new SqlParameter("@TotalAmount", SqlDbType.Decimal) { Precision = 7, Scale = 2, Value = totalAmount },
                    new SqlParameter("@TotalDeductions", SqlDbType.Decimal) { Precision = 6, Scale = 2, Value = totalDeductions },
                    new SqlParameter("@TotalTaxes", SqlDbType.Decimal) { Precision = 6, Scale = 2, Value = totalTaxes }
                );

            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paymentId"></param>
        /// <param name="amount"></param>
        /// <param name="deductionType"></param>
        public void SaveDeduction(Guid paymentId, decimal amount, DeductionType deductionType)
        {
            _dataManager.ExecuteNonQuery
                (
                    SqlStatements.SaveDeduction,
                    new SqlParameter("@PaymentId", SqlDbType.UniqueIdentifier) { Value = paymentId },
                    new SqlParameter("@Amount", SqlDbType.Decimal) { Precision = 6, Scale = 2, Value = amount },
                    new SqlParameter("@Type", SqlDbType.VarChar, 50) {Value = deductionType.ToString()}
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paymentId"></param>
        /// <param name="amount"></param>
        /// <param name="taxType"></param>
        public void SaveTax(Guid paymentId, decimal amount, TaxType taxType)
        {
            _dataManager.ExecuteNonQuery
                (
                    SqlStatements.SaveTax,
                    new SqlParameter("@PaymentId", SqlDbType.UniqueIdentifier) { Value = paymentId },
                    new SqlParameter("@Amount", SqlDbType.Decimal) { Precision = 6, Scale = 2, Value = amount },
                    new SqlParameter("@Type", SqlDbType.VarChar, 50) { Value = taxType.ToString() }
                );
        }

        #region Dispoable Pattern
        ~CalculatorRepository()
        {
            Dispose(false);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isDisposing"></param>
        private void Dispose(bool isDisposing)
        {
            if (!this._disposed)
            {
                if (isDisposing)
                {
                    if (_dataManager != null)
                    {
                        _dataManager.Dispose();
                    }
                }
                _disposed = true;
            }
        }
        #endregion
    }
}