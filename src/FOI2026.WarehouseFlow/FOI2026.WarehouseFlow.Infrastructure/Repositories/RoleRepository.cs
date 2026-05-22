using FOI2026.WarehouseFlow.Services;
using FOI2026.WarehouseFlow.Services.Models;
using FOI2026.WarehouseFlow.Services.Repository_Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FOI2026.WarehouseFlow.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {

        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ApplicationRole>> GetAllAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<ApplicationRole?> GetByIdAsync(string id)
        {
            return await _context.Roles.FindAsync(id);
        }

        public async Task AddAsync(ApplicationRole role)
        {
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ApplicationRole role)
        {
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role != null)
            {
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
            }
        }

    }
}
