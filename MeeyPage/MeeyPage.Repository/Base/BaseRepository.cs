using Dapper;
using MeeyPage.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace MeeyPage.Repository.Base
{
    public class BaseRepository : IBaseRepository
    {
        /// <summary>
        /// configuration
        /// </summary>
        IConfiguration configuration;
        public string _connectionString { get; set; }
        public DataAccessMySql _dataAccessMySql { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_configuration"></param>
        public BaseRepository(IConfiguration _configuration)
        {
            _connectionString = _configuration["DBConnection"];
            _dataAccessMySql = new DataAccessMySql(_connectionString);
        }

        public List<object> QueryMultiple(string procedureName, Dictionary<string, object> param, params Func<SqlMapper.GridReader, object>[] readerFuncs)
        {
            return _dataAccessMySql.QueryMultiple(procedureName, param, readerFuncs);
        }

        public List<object> QueryMultiple(string procedureName, DynamicParameters param, params Func<SqlMapper.GridReader, object>[] readerFuncs)
        {
            return _dataAccessMySql.QueryMultiple(procedureName, param, readerFuncs);
        }

        public bool Insert(string procName, object obj)
        {
            int iRowAffects = _dataAccessMySql.ExecuteNonQuery(procName, CommonHelper.ConvertObjectToDictionary(obj));
            return true; // no exception => true
        }

        public int InsertScalar(string procName, object obj)
        {
            int iRowAffects = _dataAccessMySql.ExecuteScalar(procName, CommonHelper.ConvertObjectToDictionary(obj));
            return iRowAffects; // no exception => true
        }

        public int InsertMaster(string procName, object obj)
        {
            int iRowAffects = _dataAccessMySql.ExecuteNonQuery(procName, CommonHelper.ConvertObjectToDictionary(obj));
            return iRowAffects; // no exception => true
        }

        public bool Insert(string procName, List<object> obj)
        {
            int iRowAffects = _dataAccessMySql.ExecuteNonQuery(procName, obj);
            return true;
        }

        public bool Update(string procName, object obj)
        {
            int iRowAffects = _dataAccessMySql.ExecuteNonQuery(procName, CommonHelper.ConvertObjectToDictionary(obj));
            return true; // no exception => true
        }

        public bool DeleteMultiple(string procName)
        {
            int iRowAffects = _dataAccessMySql.ExecuteQueryString(procName);
            if (iRowAffects != 0)
                return true; // no exception => true
            else
                return false;
        }

        public bool Delete(string procName, string idProperty, object idValue)
        {
            try
            {
                _dataAccessMySql.ExecuteNonQuery(procName, CommonHelper.ConvertObjectToDictionary(idValue));
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
