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
        
        private bool _disposed = false;


        private IDbConnection _conn;
        internal IDbConnection Connection
        {
            get 
            { 
                return _conn ?? 
                    (
                        _conn = new SqlConnection("Server=localhost;Initial Catalog=Paycheck;Integrated Security=SSPI")
                    ); 
            }
            set { _conn = value; }
        }

        private IDbCommand _command = null;
        internal IDbCommand Command
        {
            get
            {
                return _command ?? 
                    (
                        _command = new SqlCommand
                            {
                                Connection = Connection as SqlConnection,
                                CommandType = CommandType.Text
                            }
                    );
            }
            set { _command = value; }
        }

        #region Constructor/Deconstructor
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
        public IDataReader ExecuteReader(string commandText, params  IDbDataParameter[] parameters)
        {
            InitCommandForExecution(commandText, parameters);
            return Command.ExecuteReader();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        public void ExecuteNonQuery(string commandText, params IDbDataParameter[] parameters)
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
        public object ExecuteScalar(string commandText, params IDbDataParameter[] parameters)
        {
            InitCommandForExecution(commandText, parameters);
            return Command.ExecuteScalar();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        public void InitCommandForExecution(string commandText, params IDbDataParameter [] parameters)
        {
            Command.Parameters.Clear();
            Command.CommandText = commandText;
            foreach(var parameter in parameters)
            {
                Command.Parameters.Add(parameter);
            }

            if(Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }
        }
    }
}