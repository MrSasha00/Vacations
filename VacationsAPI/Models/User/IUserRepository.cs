using System.Threading.Tasks;
using VacationsAPI.Models.Worker;

namespace VacationsAPI.Models.User
{
    interface IUserRepository
    {
        Task<UserEntity> Insert(UserEntity user);
        Task<UserEntity> GetByLogin(string login);
        Task<UserEntity> Delete(string login);
    } 
}