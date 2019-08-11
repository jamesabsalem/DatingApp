using DatingApp.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatingApp.Data
{
    public interface IUserRepository
    {
        Task<User> GetUser(int id);
        Task<IEnumerable<User>> GetUsers();
        Task<IEnumerable<Photos>> GetPhoto(int userId);

    }
}
