using System;

namespace SalaryCalculator
{
    public class CalculatorRepositoryException : Exception
    {
        public CalculatorRepositoryException(string message, Exception ex ) : base(message, ex)
        {
        }
    }
}