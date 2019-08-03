using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace DatingApp.Data
{
    public interface IDataAccessLayerRepository
    {
        DataTable GetDataTable(string sqlStatement);
        IEnumerable<T> SelectAll<T>(string storeProcedure, DynamicParameters parameters = null) where T : new();
        bool Execute(string storeProcedure, DynamicParameters parameters = null);

        bool Execute(string storeProcedure, SqlConnection cn,
            SqlTransaction transaction, DynamicParameters parameters = null);
    }
}