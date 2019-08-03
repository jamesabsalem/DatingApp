using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace DatingApp.Data
{
    public class DataAccessLayerRepository : IDataAccessLayerRepository
    {
        private readonly string _connectionString;

        public DataAccessLayerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DataTable GetDataTable(string sqlStatement)
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlDataAdapter(sqlStatement, cn);
                if (cn.State == ConnectionState.Closed) cn.Open();
                var dt = new DataTable();
                cmd.Fill(dt);
                if (cn.State == ConnectionState.Open) cn.Close();
                return dt;
            }
        }

        public IEnumerable<T> SelectAll<T>(string storeProcedure, DynamicParameters parameters = null) where T : new()
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                if (cn.State == ConnectionState.Closed) cn.Open();
                var queryResult = cn.QueryAsync<T>(storeProcedure, parameters, commandType: CommandType.StoredProcedure)
                    .Result.ToList();
                if (cn.State == ConnectionState.Open) cn.Close();
                return queryResult;
            }
        }

        public bool Execute(string storeProcedure, DynamicParameters parameters = null)
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                var isExecute = cn.Execute(storeProcedure, parameters, commandType: CommandType.StoredProcedure) > 0;
                return isExecute;
            }
        }

        public bool Execute(string storeProcedure, SqlConnection cn,
            SqlTransaction transaction, DynamicParameters parameters = null)
        {
            var isExecute = cn.Execute(storeProcedure, parameters, commandType: CommandType.StoredProcedure,
                                transaction: transaction) > 0;
            return isExecute;
        }
    }
}