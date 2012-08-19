using System;
using System.Data;
using System.Data.SqlClient;

namespace SalaryCalculator
{
    /// <summary>
    /// 
    /// </summary>
    public class DataManager : IDataManager
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
        /// <summary>
        /// 
        /// </summary>
        public DataManager()
        {
            _conn = new SqlConnection("Server=localhost;Initial Catalog=Paycheck;Integrated Security=SSPI");
        }

        ~DataManager()
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
   
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string commandText, params  SqlParameter[] parameters)
        {
            InitCommandForExecution(commandText, parameters);
            return Command.ExecuteReader();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        public void ExecuteNonQuery(string commandText, params SqlParameter[] parameters)
        {
            InitCommandForExecution(commandText, parameters);
            Command.ExecuteNonQuery();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public object ExecuteScalar(string commandText, params SqlParameter[] parameters)
        {
            InitCommandForExecution(commandText, parameters);
            return Command.ExecuteScalar();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        public void InitCommandForExecution(string commandText, params SqlParameter[] parameters)
        {
            Command.Parameters.Clear();
            Command.CommandText = commandText;
            Command.Parameters.AddRange(parameters);
        }
    }
}