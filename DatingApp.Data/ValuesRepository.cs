using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DatingApp.Model;

namespace DatingApp.Data
{
    public class ValuesRepository : IValuesRepository
    {
        private readonly string _connectionString;

        public ValuesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Value>> GetValues()
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                await cn.OpenAsync();
                var values = await cn.QueryAsync<Value>("sp_values_get", commandType: CommandType.StoredProcedure);
                return values.ToList();
            }
        }

        public async Task<Value> GetValueById(int id)
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@id", id);
                await cn.OpenAsync();
                var value = await cn.QueryAsync<Value>("sp_values_get_by_id", parameters,
                    commandType: CommandType.StoredProcedure);
                return value.FirstOrDefault();
            }
        }
    }
}