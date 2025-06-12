using UserManagment.ApiContracts.User;
using UserManagment.Models;

namespace UserManagment.Services
{
    public interface IUserService
    {
        Task<User> CreateUser(UserCreateInfo createInfo);
        Task<bool> DeleteUserSoft(string login);
        Task<bool> DeleteUserStrict(string login);
        Task<IEnumerable<User>> GetActiveUsers();
        Task<AuthInfo?> GetAuthInfo(string login, string password);
        Task<User?> GetUserFullInfo(string login, string password);
        Task<UserPresentInfo?> GetUserInfoByLogin(string login);
        Task<IEnumerable<User>> GetUsersOlderThanAge(int age);
        Task<bool> Recover(string login);
        Task<bool> UpdateLogin(string userLogin, string newLogin);
        Task<bool> UpdatePassword(string userLogin, string password);
        Task<User> UpdateUser(string userLogin, UserUpdateInfo updateInfo);
    }
}
