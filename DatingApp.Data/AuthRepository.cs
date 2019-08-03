using Dapper;
using DatingApp.Model;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly string _connectionString;

        public AuthRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<User> Register(User user, string password)
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                var parameters = new DynamicParameters();
                parameters.Add("@user_name", user.UserName);
                parameters.Add("@password_hash", user.PasswordHash);
                parameters.Add("@password_salt", user.PasswordSalt);
                parameters.Add("@id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                await cn.OpenAsync();
                await cn.ExecuteAsync("sp_users_save", parameters, commandType: CommandType.StoredProcedure);
                user.Id = parameters.Get<int>("@id");
                return user;
            }
        }

        public async Task<User> Login(string username, string password)
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                using (var hmac = new HMACSHA512())
                {
                    hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                    var parameters = new DynamicParameters();
                    parameters.Add("@user_name", username);
                    //parameters.Add("@password_hash", passwordHash, dbType: DbType.Binary);
                    await cn.OpenAsync();
                    var obj = await cn.QueryAsync<User>("sp_login", parameters,
                        commandType: CommandType.StoredProcedure);
                    var user = obj.FirstOrDefault();
                    if (user == null)
                        return null;

                    return !VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt) ? null : user;
                }
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                if (computedHash.Where((t, i) => t != passwordHash[i]).Any())
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> UserExists(string username)
        {
            using (var cn = new SqlConnection(_connectionString))
            {

                var parameters = new DynamicParameters();
                parameters.Add("@user_name", username);
                await cn.OpenAsync();
                var result = await
                    cn.ExecuteScalarAsync<bool>("sp_check_user_name", parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}