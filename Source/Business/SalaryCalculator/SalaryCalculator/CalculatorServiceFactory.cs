namespace SalaryCalculator
{
    /// <summary>
    /// 
    /// </summary>
    public static class CalculatorServiceFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="versionNumber"></param>
        /// <returns></returns>
        public static ICalculatorService CreateCalculatorService(short versionNumber = 2)
        {
            switch (versionNumber)
            {
                case 1: return new LegacyCalculatorService();
                case 2: return new CalculatorService();
                default: throw new InvalidVersionException();
            }
        }
    }
}