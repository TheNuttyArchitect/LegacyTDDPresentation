using System;
using System.Data;

namespace SalaryCalculator
{
    public interface IDataManager : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IDataReader ExecuteReader(string commandText, params  IDbDataParameter[] parameters);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        void ExecuteNonQuery(string commandText, params IDbDataParameter[] parameters);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        object ExecuteScalar(string commandText, params IDbDataParameter[] parameters);
    }
}