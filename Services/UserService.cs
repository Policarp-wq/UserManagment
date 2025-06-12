using UserManagment.ApiContracts.User;
using UserManagment.Models;
using UserManagment.Repositories;

namespace UserManagment.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository; 
        private readonly ICurrentActorService _currentActorService;
        private string Login => _currentActorService.GetLogin();
        private bool IsAdmin => _currentActorService.IsAdmin();
        public UserService(ICurrentActorService currentActor, IUserRepository userRepository) 
        {
            _userRepository = userRepository;
            _currentActorService = currentActor;
        }

        public Task<User> CreateUser(UserCreateInfo createInfo)
        {
            retrun _userRepository.CreateUser(createInfo, Login);
        }

        public Task<bool> DeleteUserSoft(string login)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserStrict(string login)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetActiveUsers()
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetUserFullInfo(string login, string password)
        {
            throw new NotImplementedException();
        }

        public Task<UserPresentInfo?> GetUserInfoByLogin(string login)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetUsersOlderThanAge(int age)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Recover(string login, string modifier)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateLoginByAdmin(string userLogin, string newLogin)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateLoginByUser(string userLogin, string newLogin)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatePasswordByAdmin(string userLogin, string password)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatePasswordByUser(string userLogin, string password)
        {
            throw new NotImplementedException();
        }

        public Task<User> UpdateUser(string userLogin, UserUpdateInfo updateInfo)
        {
            throw new NotImplementedException();
        }
    }
}
