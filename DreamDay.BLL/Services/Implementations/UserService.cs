using DreamDay.BLL.Services.Interfaces;
using DreamDay.DAL.Repositories.Interfaces;
using DreamDay.Models.Entities;
using DreamDay.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.BLL.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<IEnumerable<User>> GetAllUsersAsync() => _userRepository.GetAllAsync();

        public Task<User?> GetUserByIdAsync(int id) => _userRepository.GetByIdAsync(id);

        public Task CreateUserAsync(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Password))
            {
                throw new ArgumentException("Password cannot be empty.");
            }

            user.Password = PasswordUtility.HashPassword(user.Password);

            return _userRepository.AddAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            var existingUser = await _userRepository.GetByIdAsync(user.Id);
            if (existingUser == null)
            {
                throw new ArgumentException("User not found.");
            }

            user.Password = string.IsNullOrWhiteSpace(user.Password)
                ? existingUser.Password
                : PasswordUtility.HashPassword(user.Password);

            await _userRepository.UpdateAsync(user);
        }

        public Task DeleteUserAsync(int id) => _userRepository.DeleteAsync(id);

        public Task<User?> GetUserByEmailAsync(string email)    
            => _userRepository.GetByConditionAsync(u => u.Email == email);

        public Task<User?> GetUserByResetTokenAsync(string token)
            => _userRepository.GetByConditionAsync(u =>
                u.PasswordResetToken == token &&
                u.PasswordResetTokenExpiry != null &&
                u.PasswordResetTokenExpiry > DateTime.UtcNow
            );

        public async Task<bool> SetPasswordResetTokenAsync(int userId, string token, DateTime expiry)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            user.PasswordResetToken = token;
            user.PasswordResetTokenExpiry = expiry;

            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task UpdatePasswordAsync(int userId, string newHashedPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            user.Password = newHashedPassword;
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiry = null;

            await _userRepository.UpdateAsync(user);
        }

        public async Task ClearPasswordResetTokenAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return;
            }

            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiry = null;

            await _userRepository.UpdateAsync(user);
        }

    }
}
