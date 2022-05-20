using Dapper;
using System;
using System.Collections.Generic;

namespace MeeyPage.Repository
{
    public interface IBaseRepository
    {
        List<object> QueryMultiple(string procedureName, Dictionary<string, object> param, params Func<SqlMapper.GridReader, object>[] readerFuncs);
        bool Insert(string procName, object obj);
        bool Update(string procName, object obj);
        bool Delete(string procName, string idProperty, object idValue);
    }
}
