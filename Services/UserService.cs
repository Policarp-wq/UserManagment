using Microsoft.IdentityModel.Tokens;
using UserManagment.ApiContracts.User;
using UserManagment.Exceptions;
using UserManagment.Models;
using UserManagment.Repositories;

namespace UserManagment.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository; 
        private readonly ICurrentActorService _currentActorService;
        private string CurrentUserLogin => _currentActorService.GetLogin();
        private bool CurrentUserIsAdmin => _currentActorService.IsAdmin();
        public UserService(ICurrentActorService currentActor, IUserRepository userRepository) 
        {
            _userRepository = userRepository;
            _currentActorService = currentActor;
        }

        public Task<User> CreateUser(UserCreateInfo createInfo)
        {
            if (!CurrentUserIsAdmin)
                throw new PermissionException("Attempted to delete data by user without permission");
            return _userRepository.CreateUser(createInfo, CurrentUserLogin);
        }

        public Task<bool> DeleteUserSoft(string login)
        {
            if (!CurrentUserIsAdmin)
                throw new PermissionException("Attempted to delete data by user without permission");
            return _userRepository.DeleteUserSoft(login, CurrentUserLogin);
        }

        public Task<bool> DeleteUserStrict(string login)
        {
            if (!CurrentUserIsAdmin)
                throw new PermissionException("Attempted to delete data by user without permission"); //attrs?
            return _userRepository.DeleteUserStrict(login, CurrentUserLogin);
        }

        public Task<IEnumerable<User>> GetActiveUsers()
        {
            if (!CurrentUserIsAdmin)
                throw new PermissionException("Attempted to receive sensitive data by user without permission");
            return _userRepository.GetActiveUsers();
        }
        public async Task<AuthInfo?> GetAuthInfo(string login, string password)
        {
            var user = await _userRepository.GetUserFullInfo(login, password);
            if(user == null)
                return null;
            var authInfo = new AuthInfo {Login = user.Login, IsAdmin = user.Admin };
            return authInfo;
        }
        public Task<User?> GetUserFullInfo(string login, string password)
        {
            if (!CurrentUserLogin.Equals(login))
                throw new PermissionException("Attempted to receive user full data by other");
            return _userRepository.GetUserFullInfo(login, password);
        }

        public Task<UserPresentInfo?> GetUserInfoByLogin(string login)
        {
            if (!CurrentUserIsAdmin)
                throw new PermissionException("Attempted to receive sensitive data by user without permission");
            return _userRepository.GetUserInfoByLogin(login);
        }

        public Task<IEnumerable<User>> GetUsersOlderThanAge(int age)
        {
            if (!CurrentUserIsAdmin)
                throw new PermissionException("Attempted to receive sensitive data by user without permission");
            return _userRepository.GetUsersOlderThanAge(age);
        }

        public Task<bool> Recover(string login)
        {
            if (!CurrentUserIsAdmin)
                throw new PermissionException("Attempted to recover without permission");
            return _userRepository.Recover(login, CurrentUserLogin);
        }

        public Task<bool> UpdateLogin(string userLogin, string newLogin)
        {
            if(CurrentUserIsAdmin)
                return _userRepository.UpdateLoginByAdmin(userLogin, newLogin, CurrentUserLogin);
            if(!CurrentUserLogin.Equals(userLogin))
                throw new PermissionException("Attempted to update user login by other user");
            return _userRepository.UpdateLoginByUser(userLogin, newLogin);
        }


        public Task<bool> UpdatePassword(string userLogin, string password)
        {
            if (CurrentUserIsAdmin)
                return _userRepository.UpdatePasswordByAdmin(userLogin, password, CurrentUserLogin);
            if (!CurrentUserLogin.Equals(userLogin))
                throw new PermissionException("Attempted to update user password by other user");
            return _userRepository.UpdatePasswordByUser(userLogin, password);
        }

        public Task<User> UpdateUser(string userLogin, UserUpdateInfo updateInfo)
        {
            if(CurrentUserIsAdmin || CurrentUserLogin.Equals(userLogin))
                return _userRepository.UpdateUser(userLogin, updateInfo, CurrentUserLogin);
            throw new PermissionException("Attempted to update user info by other user without permission");

        }
    }
}
