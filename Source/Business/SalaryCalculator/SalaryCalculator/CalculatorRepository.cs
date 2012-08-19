using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using SalaryCalculator.DomainObjects;

namespace SalaryCalculator
{
    /// <summary>
    /// 
    /// </summary>
    public class CalculatorRepository : ICalculatorRepository
    {
        private bool _disposed;
        private IDataManager _dataManager;
        internal IDataManager DataManager
        {
            get { return _dataManager ?? (_dataManager = new DataManager()); }
            set { _dataManager = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        public Guid GetEmployeeId(string firstName, string lastName)
        {
            try
            {
                var id = DataManager.ExecuteScalar
                    (
                        SqlStatements.GetEmployeeId,
                        new CalculatorParameter("@FirstName", DbType.AnsiString, 50, firstName),
                        new CalculatorParameter("@LastName", DbType.AnsiString, 50, lastName)
                    );

                if(id == null || DBNull.Value.Equals(id))
                {
                    throw new NullReferenceException("Datastore contains no id for employee");
                }

                return new Guid(id.ToString());
            }
            catch (Exception ex)
            {
                throw new CalculatorRepositoryException(string.Format("Could not get EmployeeId for {0}, {1}", firstName, lastName), ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="payPeriodBegin"></param>
        /// <param name="payPeriodEnd"></param>
        /// <param name="annualSalary"> </param>
        /// <returns></returns>
        public IEnumerable<Salary> GetSalaryInformation(Guid employeeId, DateTime payPeriodBegin, DateTime payPeriodEnd, out decimal annualSalary)
        {
            annualSalary = 0;
            var salaries = new List<Salary>();
            using 
                (
                    var reader = DataManager.ExecuteReader
                        (
                            SqlStatements.GetSalaryInformation,
                            new CalculatorParameter("@EmployeeId", DbType.Guid, employeeId),
                            new CalculatorParameter("@PayPeriodBegin", DbType.DateTime, payPeriodBegin),
                            new CalculatorParameter("@PayPeriodEnd", DbType.DateTime, payPeriodEnd)
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
                    var reader = DataManager.ExecuteReader
                        (
                            SqlStatements.GetAvailableDeductions,
                            new CalculatorParameter("@EmployeeId", DbType.Guid, employeeId),
                            new CalculatorParameter("@PayPeriodBegin", DbType.DateTime, payPeriodBegin),
                            new CalculatorParameter("@PayPeriodEnd", DbType.DateTime, payPeriodEnd)
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
                    var reader = DataManager.ExecuteReader
                        (
                            SqlStatements.GetPeriodTaxRates,
                            new CalculatorParameter("@Salary", DbType.Decimal, annualSalary),
                            new CalculatorParameter("@PayPeriodBegin", DbType.DateTime, payPeriodBegin),
                            new CalculatorParameter("@PayPeriodEnd", DbType.DateTime, payPeriodEnd)
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
                    var reader = DataManager.ExecuteReader
                        (
                            SqlStatements.GetYearToDateFinancials,
                            new CalculatorParameter("@EmployeeId", DbType.Guid, employeeId),
                            new CalculatorParameter("@Paydate", DbType.DateTime, payDate)
                        )
                )
            {
                int totalYtdOrd = reader.GetOrdinal("TotalYTD");
                int deductionsYtdOrd = reader.GetOrdinal("DeductionsYTD");
                int taxesYtdOrd = reader.GetOrdinal("TaxesYTD");

                while(reader.Read())
                {
                    if(ytdFinancials != null)
                    {
                        throw new InvalidDataException("There can not be more than one row of prior paycheck information.");
                    }

                    ytdFinancials = new YearToDateFinancials
                        {
                            TotalPayYearToDate = reader.GetDecimal(totalYtdOrd),
                            TotalDeductionsYearToDate = reader.GetDecimal(deductionsYtdOrd),
                            TotalTaxesYearToDate = reader.GetDecimal(taxesYtdOrd)
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
            DataManager.ExecuteNonQuery
                (
                    SqlStatements.SavePaycheck,
                    new CalculatorParameter("@PaymentId", DbType.Guid, paymentId),
                    new CalculatorParameter("@EmployeeId", DbType.Guid, employeeId),
                    new CalculatorParameter("@Paydate", DbType.DateTime, payDate),
                    new CalculatorParameter("@TotalAmount", DbType.Decimal, 7, 2,totalAmount),
                    new CalculatorParameter("@TotalDeductions", DbType.Decimal,  6, 2, totalDeductions),
                    new CalculatorParameter("@TotalTaxes", DbType.Decimal, 6, 2, totalTaxes)
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paymentId"></param>
        /// <param name="amount"></param>
        /// <param name="deductionType"></param>
        public void SaveDeduction(Guid paymentId, decimal amount, DeductionType deductionType)
        {
            DataManager.ExecuteNonQuery
                (
                    SqlStatements.SaveDeduction,
                    new CalculatorParameter("@PaymentId", DbType.Guid, paymentId),
                    new CalculatorParameter("@Amount", DbType.Decimal, 6, 2, amount),
                    new CalculatorParameter("@Type", DbType.AnsiString, 50, deductionType.ToString())
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
            DataManager.ExecuteNonQuery
                (
                    SqlStatements.SaveTax,
                    new CalculatorParameter("@PaymentId", DbType.Guid, paymentId),
                    new CalculatorParameter("@Amount", DbType.Decimal, 6, 2,amount),
                    new CalculatorParameter("@Type", DbType.AnsiString, 50, taxType.ToString())
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
            if (!_disposed)
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