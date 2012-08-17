using System;
using System.Data;
using System.Data.SqlClient;

namespace SalaryCalculator
{
    public class DataManager : IDisposable
    {
        private readonly SqlConnection _conn;
        private bool _disposed = false;
        private SqlCommand _command = null;

        private SqlCommand Command
        {
            get
            {
                if(_command == null)
                {
                    _command = new SqlCommand
                                   {
                                       Connection = _conn,
                                       CommandType = CommandType.Text
                                   };
                }
                return _command;
            }
        }

        #region Constructor/Deconstructor
        public DataManager()
        {
            _conn = new SqlConnection("Server=localhost;Initial Catalog=Paycheck;Integrated Security=SSPI");
        }

        ~DataManager()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool isDisposing)
        {
            if(!this._disposed)
            {
                if (isDisposing)
                {
                    if(_command != null)
                    {
                        _command.Dispose();
                    }
                    if (_conn != null)
                    {
                        _conn.Dispose();
                    }
                }

                _disposed = true;
            }
        }
        #endregion
   

        public SqlDataReader ExecuteReader(string commandText, params  SqlParameter[] parameters)
        {
            InitCommandForExecution(commandText, parameters);
            return Command.ExecuteReader();
        }

        public void ExecuteNonQuery(string commandText, params SqlParameter[] parameters)
        {
            InitCommandForExecution(commandText, parameters);
            Command.ExecuteNonQuery();
        }

        public object ExecuteScalar(string commandText, params SqlParameter[] parameters)
        {
            InitCommandForExecution(commandText, parameters);
            return Command.ExecuteScalar();
        }

        public void InitCommandForExecution(string commandText, params SqlParameter[] parameters)
        {
            Command.Parameters.Clear();
            Command.CommandText = commandText;
            Command.Parameters.AddRange(parameters);
        }
    }
}