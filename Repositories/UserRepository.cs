using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Diagnostics;
using UserManagment.ApiContracts.User;
using UserManagment.Exceptions;
using UserManagment.Models;

namespace UserManagment.Repositories
{
    public class UserRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<User> _users;
        public UserRepository(AppDbContext dbContext) 
        {
            _context = dbContext;
            _users = dbContext.Users;
        }
        /// <summary>
        /// Only for admins
        /// </summary>
        public async Task<User> CreateUser(UserCreateInfo createInfo, string creator)
        {
            var res = await _users.AddAsync(new User()
            {
                Guid = Guid.NewGuid(),
                Login = createInfo.Login,
                Password = createInfo.Pasword,
                Name = createInfo.Name,
                Gender = createInfo.Gender,
                Birthday = createInfo.Birthday,
                Admin = createInfo.IsAdmin,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = creator,
                ModifiedOn = DateTime.UtcNow,
                ModifiedBy = creator,
                RevokedOn = null,
                RevokedBy = ""
            });
            await _context.SaveChangesAsync();
            return res.Entity;
        }
        private async Task<User> GetUserByLogin(string userLogin)
        {
            var user = await _users.SingleOrDefaultAsync(u => u.Login.Equals(userLogin));
            if (user == null)
                throw new DatabaseException($"Failed to update user with login {userLogin}: Not Found", StatusCodes.Status404NotFound);
            return user;
        }
        public async Task<User> UpdateUser(string userLogin, UserUpdateInfo updateInfo, string modifier)
        {
            var user = await GetUserByLogin(userLogin);
            if (!user.IsActive())
                throw new DatabaseException($"Failed to update user with login {userLogin}: this user is revoked");
            user.Name = updateInfo.Name ?? user.Name; 
            user.Gender = updateInfo.Gender ?? user.Gender;
            user.Birthday = updateInfo.Birthday;
            user.ModifiedBy = modifier;
            user.ModifiedOn = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return user;
        }
        private async Task<bool> UpdatePasswordRaw(User user, string password, string modifier)
        {
            if(user.Password.Equals(password))
                return false;

            user.Password = password;
            user.ModifiedBy = modifier;
            user.ModifiedOn = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdatePasswordByUser(string userLogin, string password)
        {
            var user = await GetUserByLogin(userLogin);
            if (!user.IsActive())
                throw new DatabaseException($"Failed to update password for user with login {userLogin}: this user is revoked");
            return await UpdatePasswordRaw(user, password, user.Login);
        }
        /// <summary>
        /// Only for admins
        /// </summary>
        public async Task<bool> UpdatePasswordByAdmin(string userLogin, string password, string modifier)
        {
            var user = await GetUserByLogin(userLogin);
            return await UpdatePasswordRaw(user, password, modifier);
        }

        private async Task<bool> UpdateLoginRaw(User user, string newLogin, string modifier)
        {
            if (user.Login.Equals(newLogin))
                return false;
            if(await _users.AnyAsync(u => u.Login.Equals(newLogin))){
                throw new DatabaseException($"Failed to update login for user with login {user.Login}: this login is already in use");
            }
            user.Login = newLogin;
            user.ModifiedBy = modifier;
            user.ModifiedOn = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateLoginByUser(string userLogin, string newLogin)
        {
            var user = await GetUserByLogin(userLogin);
            if (!user.IsActive())
                throw new DatabaseException($"Failed to update password for user with login {userLogin}: this user is revoked");
            return await UpdateLoginRaw(user, newLogin, newLogin);
        }
        /// <summary>
        /// Only for admins
        /// </summary>
        public async Task<bool> UpdateLoginByAdmin(string userLogin, string newLogin, string modifier)
        {
            var user = await GetUserByLogin(userLogin);
            return await UpdateLoginRaw(user, newLogin, modifier);
        }
        //what kind of info of the users return?
        /// <summary>
        /// Only for admins
        /// </summary>
        public async Task<IEnumerable<User>> GetActiveUsers()
        {
            return await _users
                .AsNoTracking()
                .Where(u => u.IsActive())
                .OrderBy(u => u.CreatedOn)
                .ToListAsync();
        }
        /// <summary>
        /// Only for admins
        /// </summary>
        public async Task<UserPresentInfo?> GetUserInfoByLogin(string login)
        {
             return await _users
                .AsNoTracking()
                .Where(u => u.Login.Equals(login))
                .Select(u => new UserPresentInfo(u.Name, u.Gender, u.Birthday, u.IsActive()))
                .SingleOrDefaultAsync();
        }
        /// <summary>
        /// Only for user
        /// </summary>
        public async Task<User?> GetUserFullInfo(string login, string password)
        {
            var user = await _users
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Login.Equals(login) && u.Password.Equals(password));
            if(user == null)
                return null;
            if (!user.IsActive())
                throw new DatabaseException($"Failed to return user info for {login}: this user is revoked"); // general method for checking is revoked?
            return user;
        }
        /// <summary>
        /// Only for admins
        /// </summary>
        public async Task<IEnumerable<User>> GetUsersOlderThanAge(int age)
        {
            return await _users
                .AsNoTracking()
                .Where(u => u.IsOlderThan(age))
                .ToListAsync();
        }
        /// <summary>
        /// Only for admins
        /// </summary>
        public async Task<bool> DeleteUserSoft(string login, string modifier)
        {
            var user = await GetUserByLogin(login);
            if (user.RevokedOn != null)
                return false;
            user.RevokedBy = modifier;
            user.RevokedOn = DateTime.UtcNow;
            user.ModifiedBy = modifier;
            user.ModifiedOn = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
        /// <summary>
        /// Only for admins
        /// </summary>
        public async Task<bool> DeleteUserStrict(string login, string modifier)
        {
            return await _users.Where(u => u.Login.Equals(login)).ExecuteDeleteAsync() > 0;
        }
        /// <summary>
        /// Only for admins
        /// </summary>
        public async Task<bool> Recover(string login, string modifier)
        {
            var user = await GetUserByLogin(login);
            if(user.RevokedOn == null)
                return false;
            user.RevokedOn = null;
            user.RevokedBy = "";
            user.ModifiedBy = modifier;
            user.ModifiedOn = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;    
        }
    }
}
