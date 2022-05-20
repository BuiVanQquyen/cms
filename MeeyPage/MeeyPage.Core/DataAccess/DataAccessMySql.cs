using Dapper;
using EntLibContrib.Data.MySql;
using Microsoft.Practices.EnterpriseLibrary.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace MeeyPage.Core
{
    public class DataAccessMySql
    {
        /// <summary>
        /// connection string
        /// </summary>
        protected string _connectionString;

        protected Database _dbBase;
        protected Database InitDatabase()
        {
            return new MySqlDatabase(_connectionString);
        }

        public DataAccessMySql(string connectionString)
        {
            _connectionString = connectionString;
            _dbBase = InitDatabase();
        }

        public List<object> QueryMultiple(string procedureName, Dictionary<string, object> dicParams, params Func<SqlMapper.GridReader, object>[] readerFuncs)
        {
            var result = new List<object>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                if (dicParams == null)
                {
                    dicParams = new Dictionary<string, object>();
                }
                DynamicParameters _dynamicParams = TypedDynamicParameter(_dbBase, procedureName, dicParams);
                var gridReader = connection.QueryMultiple(procedureName, _dynamicParams, commandType: CommandType.StoredProcedure);

                foreach (var readerFunc in readerFuncs)
                {
                    var obj = readerFunc(gridReader);
                    result.Add(obj);
                }

                connection.Close();
                connection.Dispose();
            }
            return result;
        }

        public List<object> QueryMultiple(string procedureName, DynamicParameters dicParams, params Func<SqlMapper.GridReader, object>[] readerFuncs)
        {
            var result = new List<object>();
            using (var connection = new MySqlConnection(_connectionString))
            { 
                connection.Open();
                var gridReader = connection.QueryMultiple(procedureName, dicParams, commandType: CommandType.StoredProcedure);

                foreach (var readerFunc in readerFuncs)
                {
                    var obj = readerFunc(gridReader);
                    result.Add(obj);
                }

                connection.Close();
                connection.Dispose();
            }
            return result;
        }

        public int ExecuteNonQuery(string procedureName, Dictionary<string, object> dicParams)
        {
            int iRowAffects = 0;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var dbTransaction = connection.BeginTransaction();

                if (dicParams == null)
                {
                    dicParams = new Dictionary<string, object>();
                }
                DynamicParameters _dynamicParams = TypedDynamicParameter(_dbBase, procedureName, dicParams);
               
                try
                {
                    CommandDefinition cmd = new CommandDefinition(procedureName, _dynamicParams, dbTransaction, null, CommandType.StoredProcedure);
                    iRowAffects = connection.Execute(cmd);
                    dbTransaction.Commit();
                }
                catch (Exception ex)
                {
                    iRowAffects = 0;
                    dbTransaction.Rollback();
                    throw ex;
                }

                connection.Close();
                connection.Dispose();
            }

            return iRowAffects;
        }

        public int ExecuteScalar(string procedureName, Dictionary<string, object> dicParams)
        {
            int iRowAffects = 0;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var dbTransaction = connection.BeginTransaction();

                if (dicParams == null)
                {
                    dicParams = new Dictionary<string, object>();
                }
                DynamicParameters _dynamicParams = TypedDynamicParameter(_dbBase, procedureName, dicParams);

                try
                {
                    CommandDefinition cmd = new CommandDefinition(procedureName, _dynamicParams, dbTransaction, null, CommandType.StoredProcedure);
                    iRowAffects = connection.ExecuteScalar<int>(cmd);
                    dbTransaction.Commit();
                }
                catch (Exception ex)
                {
                    iRowAffects = 0;
                    dbTransaction.Rollback();
                    throw ex;
                }

                connection.Close();
                connection.Dispose();
            }

            return iRowAffects;
        }

        public int ExecuteQueryString(string query)
        {
            int iRowAffects = 0;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
               
                MySqlCommand myCommand = connection.CreateCommand();
                MySqlTransaction myTrans;

                myTrans = connection.BeginTransaction();
                myCommand.Connection = connection;
                myCommand.Transaction = myTrans;

                try
                {
                    myCommand.CommandText = query;
                    iRowAffects = myCommand.ExecuteNonQuery();
                    myTrans.Commit();
                }
                catch (Exception ex)
                {
                    iRowAffects = 0;
                    myTrans.Rollback();
                    throw ex;
                }

                connection.Close();
                connection.Dispose();
            }

            return iRowAffects;
        }

        public int ExecuteNonQuery(string procedureName, List<object> obj)
        {
            int iRowAffects = 1;
            
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var dbTransaction = connection.BeginTransaction();

                try
                {
                    foreach (var item in obj)
                    {
                        DynamicParameters _dynamicParams = TypedDynamicParameter(_dbBase, procedureName, CommonHelper.ConvertObjectToDictionary(item));
                        CommandDefinition cmd = new CommandDefinition(procedureName, _dynamicParams, dbTransaction, null, CommandType.StoredProcedure);
                        iRowAffects = connection.Execute(cmd);
                    }
                    dbTransaction.Commit();
                }
                catch (Exception ex)
                {
                    iRowAffects = 0;
                    dbTransaction.Rollback();
                    throw ex;
                }

                connection.Close();
                connection.Dispose();
            }

            return iRowAffects;
        }

        public DynamicParameters TypedDynamicParameter(Database dbBase, string procName, Dictionary<string, object> paramsValue, DbCommand dbCommand = null)
        {
            var _params = new DynamicParameters();
            DbCommand _dbCommand = dbCommand != null ? dbCommand : null;
            bool isTextParameterType = false;

            if (_dbCommand == null)
            {
                _dbCommand = dbBase.GetStoredProcCommand(procName);
                dbBase.DiscoverParameters(_dbCommand);
            }

            if (paramsValue != null)
            {
                foreach (DbParameter parameter in _dbCommand.Parameters)
                {
                    isTextParameterType = IsTextParameterType(parameter);

                    var parameterName = parameter.ParameterName.Remove(0, 3);

                    if (parameter.Direction == ParameterDirection.Input &&
                        (paramsValue.ContainsKey(parameterName)
                        || paramsValue.ContainsKey(parameterName.ToUpper())
                        || paramsValue.ContainsKey(parameterName.ToLower()))
                        || paramsValue.Any(x => x.Key.ToUpper().Contains(parameterName.ToUpper())))
                    {
                        parameter.Value = paramsValue.Where(o => o.Key.ToLower() == parameterName.ToLower()).Select(x => x.Value).FirstOrDefault();
                        _params.Add(parameter.ParameterName, parameter.Value, parameter.DbType, parameter.Direction);
                    }
                }
            }

            return _params;
        }

        public bool IsTextParameterType(DbParameter parameter)
        {
            return parameter.DbType != DbType.Int16
                && parameter.DbType != DbType.Int32
                && parameter.DbType != DbType.Int64
                && parameter.DbType != DbType.Decimal
                && parameter.DbType != DbType.Date
                && parameter.DbType != DbType.DateTime;
        }
    }
}
