using UserManagment.ApiContracts.User;
using UserManagment.Models;

namespace UserManagment.Repositories
{
    public interface IUserRepository
    {
        Task<User> CreateUser(UserCreateInfo createInfo, string creator);
        Task<bool> DeleteUserSoft(string login, string modifier);
        Task<bool> DeleteUserStrict(string login, string modifier);
        Task<IEnumerable<User>> GetActiveUsers();
        Task<User?> GetUserFullInfo(string login, string password);
        Task<UserPresentInfo?> GetUserInfoByLogin(string login);
        Task<IEnumerable<User>> GetUsersOlderThanAge(int age);
        Task<bool> Recover(string login, string modifier);
        Task<bool> UpdateLoginByAdmin(string userLogin, string newLogin, string modifier);
        Task<bool> UpdateLoginByUser(string userLogin, string newLogin);
        Task<bool> UpdatePasswordByAdmin(string userLogin, string password, string modifier);
        Task<bool> UpdatePasswordByUser(string userLogin, string password);
        Task<User> UpdateUser(string userLogin, UserUpdateInfo updateInfo, string modifier);
    }
}