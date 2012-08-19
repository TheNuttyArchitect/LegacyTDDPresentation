using System;
using SalaryCalculator.DomainObjects;

namespace SalaryCalculator
{
    /// <summary>
    /// 
    /// </summary>
    internal class CalculatorService : ICalculatorService
    {
        private bool _disposed;
        private ICalculatorRepository _calculatorRepository;
        /// <summary>
        /// 
        /// </summary>
        public ICalculatorRepository Repository
        {
            get { return _calculatorRepository ?? (_calculatorRepository = new CalculatorRepository()); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="payDate"></param>
        /// <returns></returns>
        public CalculatorResponse Invoke(string firstName, string lastName, DateTime payDate)
        {
            CalculatorResponse response = InitializeResponse(firstName, lastName, payDate);

            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="payDate"></param>
        /// <returns></returns>
        internal CalculatorResponse InitializeResponse(string firstName, string lastName, DateTime payDate)
        {
            CalculatorResponseGuards.AssertInitializeParameters(firstName, lastName, payDate);

            var calculatorResponse = new CalculatorResponse
            {
                FullName = String.Format("{0}, {1}", lastName, firstName),
                PaymentId = Guid.NewGuid(),
                PayDate = payDate,
                PayPeriodBegin = payDate.AddDays(-28),
                PayPeriodEnd = payDate.AddDays(-14)
            };

            return calculatorResponse;
        }

        #region Disposable Pattern
        ~CalculatorService()
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
            if(!this._disposed)
            {
                if (isDisposing)
                {
                    if (_calculatorRepository != null)
                    {
                        _calculatorRepository.Dispose();
                    }
                }

                _disposed = true;
            }
        }
        #endregion
    }
}