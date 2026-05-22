using FOI2026.WarehouseFlow.Infrastructure.Data.Models;
using FOI2026.WarehouseFlow.Services.Repository_Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FOI2026.WarehouseFlow.Services.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> GetByStatusAsync(bool isActive)
        {
            var users = await _userRepository.GetAllAsync();
            return users.Where(u => u.IsActive == isActive);
        }


        public async Task AddAsync(ApplicationUser user)
        {
            await _userRepository.AddAsync(user);
        }

        public async Task UpdateAsync(ApplicationUser user)
        {
            await _userRepository.UpdateAsync(user);
        }


        public async Task DeleteAsync(string id)
        {
            await _userRepository.DeleteAsync(id);
        }

        public async Task DeactivateAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user != null)
            {
                user.IsActive = false;
                await _userRepository.UpdateAsync(user);
            }
        }

        public async Task ActivateAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user != null)
            {
                user.IsActive = true;
                await _userRepository.UpdateAsync(user);
            }
        }
    }
}
