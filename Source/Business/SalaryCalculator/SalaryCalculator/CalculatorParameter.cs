using System.Data;

namespace SalaryCalculator
{
    public class CalculatorParameter : IDbDataParameter
    {
        public DbType DbType { get; set; }
        public ParameterDirection Direction { get; set; }
        public bool IsNullable { get; private set; }
        public string ParameterName { get; set; }
        public string SourceColumn { get; set; }
        public DataRowVersion SourceVersion { get; set; }
        public object Value { get; set; }
        public byte Precision { get; set; }
        public byte Scale { get; set; }
        public int Size { get; set; }

        public CalculatorParameter (string parameterName, DbType dbType, object value)
        {
            ParameterName = parameterName;
            DbType = dbType;
            Value = value;
        }

        public CalculatorParameter(string parameterName, DbType dbType, int size, object value)
        {
            ParameterName = parameterName;
            DbType = dbType;
            Value = value;
            Size = size;
        }

        public CalculatorParameter(string parameterName, DbType dbType, byte precision, byte scale, object value)
        {
            ParameterName = parameterName;
            DbType = dbType;
            Precision = precision;
            Scale = scale;
            Value = value;
        }
    }
}