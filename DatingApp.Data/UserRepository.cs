using Dapper;
using DatingApp.Model;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DatingApp.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }


        public async Task<User> GetUser(int id)
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@id", id);
                await cn.OpenAsync();
                var user = await cn.QueryFirstOrDefaultAsync<User>("sp_get_user", parameters,
                    commandType: CommandType.StoredProcedure);
                return user;
            }
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                var users = await cn.QueryAsync<User>(
                    "sp_get_users", commandType: CommandType.StoredProcedure);
                return users;
            }
        }

        public async Task<IEnumerable<Photos>> GetPhoto(int userId)
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@user_id", userId);
                var photo = await cn.QueryAsync<Photos>("sp_get_photo", parameters,
                    commandType: CommandType.StoredProcedure);
                return photo;
            }
        }

        public async Task<bool> UserUpdate(int id, User user)
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@id", id);
                parameters.Add("@introduction", user.Introduction);
                parameters.Add("@looking_for", user.LookingFor);
                parameters.Add("@interests", user.Interests);
                parameters.Add("@city", user.City);
                parameters.Add("@country", user.Country);
                await cn.OpenAsync();
                var isUpdate = await cn.ExecuteAsync("sp_update_user", parameters,
                    commandType: CommandType.StoredProcedure) > 0;
                return isUpdate;
            }
        }
    }
}