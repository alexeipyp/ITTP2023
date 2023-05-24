using Common.Enums;
using Common.Exceptions;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task CreateUserAsync(User userToCreate)
        {
            try
            {
                await _context.Users.AddAsync(userToCreate);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new NotUniqueLoginException();
            }
        }
        public async Task UpdateUserLoginAsync(User userToUpdate)
        {
            if (!(await IsUserFound(userToUpdate.Guid)))
            {
                throw new UserNotFoundException();
            }
            try
            {
                _context.Attach(userToUpdate);
                _context.Entry(userToUpdate).Property(p => p.Login).IsModified = true;
                _context.Entry(userToUpdate).Property(p => p.ModifiedBy).IsModified = true;
                _context.Entry(userToUpdate).Property(p => p.ModifiedOn).IsModified = true;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new NotUniqueLoginException();
            }
        }

        public async Task UpdateUserPasswordAsync(User userToUpdate)
        {
            try
            {
                _context.Attach(userToUpdate);
                _context.Entry(userToUpdate).Property(p => p.PasswordHash).IsModified = true;
                _context.Entry(userToUpdate).Property(p => p.ModifiedBy).IsModified = true;
                _context.Entry(userToUpdate).Property(p => p.ModifiedOn).IsModified = true;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new UserNotFoundException();
            }
        }

        public async Task UpdateUserInfoAsync(User userToUpdate)
        {
            try
            {
                _context.Attach(userToUpdate);
                if (userToUpdate.Name != default)
                {
                    _context.Entry(userToUpdate).Property(p => p.Name).IsModified = true;
                }
                if (userToUpdate.Birthday != default)
                {
                    _context.Entry(userToUpdate).Property(p => p.Birthday).IsModified = true;
                }
                if (userToUpdate.Gender != (int)Gender.Unknown)
                {
                    _context.Entry(userToUpdate).Property(p => p.Gender).IsModified = true;
                }
                _context.Entry(userToUpdate).Property(p => p.ModifiedBy).IsModified = true;
                _context.Entry(userToUpdate).Property(p => p.ModifiedOn).IsModified = true;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new UserNotFoundException();
            }
        }

        public async Task<IEnumerable<User>> ReadActiveUsersListAsync()
        {
            var users = await _context.Users
                .AsNoTracking()
                .Where(x => x.RevokedOn == null)
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync();
            return users;
        }

        public async Task<User> ReadUserByGuidAsync(Guid guid)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Guid == guid);
            if (user == null || user == default)
            {
                throw new UserNotFoundException();
            }
            return user;
        }

        public async Task<IEnumerable<User>> ReadUsersBornBeforeListAsync(DateTime dayBornBefore)
        {
            var users = await _context.Users.AsNoTracking().Where(x => x.Birthday <= dayBornBefore).ToListAsync();
            return users;
        }

        public async Task DeleteUserSoftlyAsync(User userToDelete)
        {
            try
            {
                _context.Attach(userToDelete);
                _context.Entry(userToDelete).Property(p => p.RevokedBy).IsModified = true;
                _context.Entry(userToDelete).Property(p => p.RevokedOn).IsModified = true;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new UserNotFoundException();
            }
        }

        public async Task DeleteUserFullyAsync(User userToDelete)
        {
            try
            {
                _context.Attach(userToDelete);
                _context.Entry(userToDelete).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new UserNotFoundException();
            }
        }

        public async Task UpdateUserActiveStatusAsync(Guid userGuid)
        {
            try
            {
                var userToRestore = new User
                {
                    Guid = userGuid,
                    RevokedBy = null,
                    RevokedOn = null,
                };
                _context.Attach(userToRestore);
                _context.Entry(userToRestore).Property(p => p.RevokedBy).IsModified = true;
                _context.Entry(userToRestore).Property(p => p.RevokedOn).IsModified = true;
                _context.Entry(userToRestore).Property(p => p.ModifiedBy).IsModified = true;
                _context.Entry(userToRestore).Property(p => p.ModifiedOn).IsModified = true;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new UserNotFoundException();
            }
        }

        public async Task<bool> IsUserActive(Guid userGuid)
            => await _context.Users.AnyAsync(x => x.Guid == userGuid && x.RevokedOn == null && x.RevokedBy == null);

        public async Task<bool> IsUserAdmin(Guid userGuid)
            => await _context.Users.AnyAsync(x => x.Guid == userGuid && x.Admin == true);

        private async Task<bool> IsUserFound(Guid userGuid)
            => await _context.Users.AnyAsync(x => x.Guid == userGuid);
    }
}
