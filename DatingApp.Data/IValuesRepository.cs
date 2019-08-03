using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.Model;

namespace DatingApp.Data
{
    public interface IValuesRepository
    {
        Task<IEnumerable<Value>> GetValues();
        Task<Value> GetValueById(int id);
    }
}