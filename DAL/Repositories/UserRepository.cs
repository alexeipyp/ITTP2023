using Common.Enums;
using Common.Exceptions;
using Common.Utils;
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
        private readonly DataContext _context = null!;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public UserRepository() 
        {
            #warning Use this constructor while testing
        }

        public virtual async Task CreateUserAsync(User userToCreate)
        {
            if(!(await IsLoginUnique(userToCreate.Login)))
            {
                throw new NotUniqueLoginException();
            }

            await _context.Users.AddAsync(userToCreate);
            await _context.SaveChangesAsync();
        }
        public virtual async Task UpdateUserLoginAsync(User userToUpdate)
        {
            if (!(await IsUserFound(userToUpdate.Guid)))
            {
                throw new UserNotFoundException();
            }
            if (!(await IsLoginUnique(userToUpdate.Login)))
            {
                throw new NotUniqueLoginException();
            }

            _context.Attach(userToUpdate);
            _context.Entry(userToUpdate).Property(p => p.Login).IsModified = true;
            _context.Entry(userToUpdate).Property(p => p.ModifiedBy).IsModified = true;
            _context.Entry(userToUpdate).Property(p => p.ModifiedOn).IsModified = true;
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateUserPasswordAsync(User userToUpdate)
        {
            if (!(await IsUserFound(userToUpdate.Guid)))
            {
                throw new UserNotFoundException();
            }

            _context.Attach(userToUpdate);
            _context.Entry(userToUpdate).Property(p => p.PasswordHash).IsModified = true;
            _context.Entry(userToUpdate).Property(p => p.ModifiedBy).IsModified = true;
            _context.Entry(userToUpdate).Property(p => p.ModifiedOn).IsModified = true;
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateUserInfoAsync(User userToUpdate)
        {
            if (!(await IsUserFound(userToUpdate.Guid)))
            {
                throw new UserNotFoundException();
            }

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

        public virtual async Task<IEnumerable<User>> ReadActiveUsersListAsync()
        {
            var users = await _context.Users
                .AsNoTracking()
                .Where(x => x.RevokedOn == null)
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync();
            return users;
        }

        public virtual async Task<User> ReadUserByLoginAsync(string login)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Login == login);
            if (user == null || user == default)
            {
                throw new UserNotFoundException();
            }
            return user;
        }

        public virtual async Task<User> ReadUserByGuidAsync(Guid guid)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Guid == guid);
            if (user == null || user == default)
            {
                throw new UserNotFoundException();
            }
            return user;
        }

        public virtual async Task<IEnumerable<User>> ReadUsersBornBeforeListAsync(DateTime dayBornBefore)
        {
            var users = await _context.Users.AsNoTracking().Where(x => x.Birthday <= dayBornBefore).ToListAsync();
            return users;
        }

        public async Task<User?> ReadActiveUserByCredentials(string login, string password)
        {
            var passwordHash = HashHelper.GetHash(password);
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Login == login && x.PasswordHash == passwordHash && x.RevokedBy == null && x.RevokedOn == null);
            return user;
        }

        public async Task<User?> ReadActiveUserByGuid(Guid userGuid)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Guid == userGuid && x.RevokedOn == null && x.RevokedBy == null);
            return user;
        }

        public virtual async Task DeleteUserSoftlyAsync(User userToDelete)
        {
            if (!(await IsUserFound(userToDelete.Guid)))
            {
                throw new UserNotFoundException();
            }

            _context.Attach(userToDelete);
            _context.Entry(userToDelete).Property(p => p.RevokedBy).IsModified = true;
            _context.Entry(userToDelete).Property(p => p.RevokedOn).IsModified = true;
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteUserFullyAsync(User userToDelete)
        {
            if (!(await IsUserFound(userToDelete.Guid)))
            {
                throw new UserNotFoundException();
            }

            _context.Attach(userToDelete);
            _context.Entry(userToDelete).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateUserActiveStatusAsync(User userToRestore)
        {
            if (!(await IsUserFound(userToRestore.Guid)))
            {
                throw new UserNotFoundException();
            }

            _context.Attach(userToRestore);
            _context.Entry(userToRestore).Property(p => p.RevokedBy).IsModified = true;
            _context.Entry(userToRestore).Property(p => p.RevokedOn).IsModified = true;
            _context.Entry(userToRestore).Property(p => p.ModifiedBy).IsModified = true;
            _context.Entry(userToRestore).Property(p => p.ModifiedOn).IsModified = true;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsUserActive(Guid userGuid)
            => await _context.Users.AnyAsync(x => x.Guid == userGuid && x.RevokedOn == null && x.RevokedBy == null);

        public virtual async Task<bool> IsUserAdmin(Guid userGuid)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Guid == userGuid);
            if (user == default || user == null)
            {
                throw new UserNotFoundException();
            }
            return user.Admin;
        }

        private async Task<bool> IsLoginUnique(string login)
            => !(await _context.Users.AnyAsync(x => x.Login == login));

        private async Task<bool> IsUserFound(Guid userGuid)
            => await _context.Users.AnyAsync(x => x.Guid == userGuid);
    }
}
