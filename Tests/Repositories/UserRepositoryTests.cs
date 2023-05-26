using Common.Exceptions;
using DAL.Entities;
using DAL.Repositories;
using DAL;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Repositories
{
    [TestClass]
    public class UserRepositoryTests
    {
        private DataContext _context = null!;
        private UserRepository _userRepository = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            var dbContextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase("DbTest").Options;
            _context = new DataContext(dbContextOptions);
            _context.Database.EnsureCreated();

            _userRepository = new UserRepository(_context);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod]
        public async Task CreateUser_Success()
        {
            var userToCreate = new User
            {
                Guid = Guid.NewGuid(),
                Login = "testuserlogin",
                PasswordHash = "testuserpassword",
                Name = "testusername",
                Gender = 2,
                Birthday = DateTime.UtcNow,
                Admin = false,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "Admin",
            };

            await _userRepository.CreateUserAsync(userToCreate);

            var createdUser = await _context.Users.FirstOrDefaultAsync(x => x.Guid == userToCreate.Guid);
            Assert.IsTrue(userToCreate.Equals(createdUser));
        }

        [TestMethod]
        [ExpectedException(typeof(NotUniqueLoginException))]
        public async Task CreateUser_Failure_NotUniqueLogin()
        {
            var userToCreate = new User
            {
                Guid = Guid.NewGuid(),
                Login = "Admin",
                PasswordHash = "testuserpassword",
                Name = "testusername",
                Gender = 2,
                Birthday = DateTime.UtcNow,
                Admin = false,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "Admin",
            };

            await _userRepository.CreateUserAsync(userToCreate);
        }

        [TestMethod]
        public async Task UpdateUserLogin_Success()
        {
            var user = await GetActiveUserAsync();

            var userToUpdate = new User
            {
                Guid = user.Guid,
                Login = "NewLogin",
                ModifiedOn = DateTime.UtcNow,
                ModifiedBy = "Admin",
            };

            await _userRepository.UpdateUserLoginAsync(userToUpdate);

            var updatedUser = await _context.Users.FirstOrDefaultAsync(x => x.Guid == userToUpdate.Guid);
            Assert.IsTrue(updatedUser != null && userToUpdate.Login == updatedUser.Login);
        }

        [TestMethod]
        [ExpectedException(typeof(NotUniqueLoginException))]
        public async Task UpdateUserLogin_Failure_NotUniqueLogin()
        {
            var user = await GetActiveUserAsync();

            var userToUpdate = new User
            {
                Guid = user.Guid,
                Login = "Admin",
                ModifiedOn = DateTime.UtcNow,
                ModifiedBy = "Admin",
            };

            await _userRepository.UpdateUserLoginAsync(userToUpdate);
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public async Task UpdateUserLogin_Failure_UserNotFound()
        {
            var userToUpdate = new User
            {
                Guid = Guid.NewGuid(),
                Login = "Admin",
                ModifiedOn = DateTime.UtcNow,
                ModifiedBy = "Admin",
            };

            await _userRepository.UpdateUserLoginAsync(userToUpdate);
        }

        [TestMethod]
        public async Task UpdateUserPassword_Success()
        {
            var user = await GetActiveUserAsync();

            var userToUpdate = new User
            {
                Guid = user.Guid,
                PasswordHash = "newpasswordhash",
                ModifiedOn = DateTime.UtcNow,
                ModifiedBy = "Admin",
            };

            await _userRepository.UpdateUserPasswordAsync(userToUpdate);

            var updatedUser = await _context.Users.FirstOrDefaultAsync(x => x.Guid == userToUpdate.Guid);
            Assert.IsTrue(updatedUser != null && userToUpdate.PasswordHash == updatedUser.PasswordHash);
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public async Task UpdateUserPassword_Failure_UserNotFound()
        {
            var userToUpdate = new User
            {
                Guid = Guid.NewGuid(),
                PasswordHash = "newpasswordhash",
                ModifiedOn = DateTime.UtcNow,
                ModifiedBy = "Admin",
            };

            await _userRepository.UpdateUserPasswordAsync(userToUpdate);
        }

        [TestMethod]
        public async Task UpdateUserInfo_Success()
        {
            var user = await GetActiveUserAsync();

            var userToUpdate = new User
            {
                Guid = user.Guid,
                Name = "newtestname",
                Gender = 1,
                Birthday = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow,
                ModifiedBy = "Admin",
            };

            await _userRepository.UpdateUserInfoAsync(userToUpdate);

            var updatedUser = await _context.Users.FirstOrDefaultAsync(x => x.Guid == userToUpdate.Guid);
            Assert.IsTrue(updatedUser != null &&
                userToUpdate.Name == updatedUser.Name &&
                userToUpdate.Gender == updatedUser.Gender &&
                userToUpdate.Birthday == updatedUser.Birthday);
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public async Task UpdateUserInfo_Failure_UserNotFound()
        {
            var userToUpdate = new User
            {
                Guid = Guid.NewGuid(),
                Name = "newtestname",
                Gender = 1,
                Birthday = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow,
                ModifiedBy = "Admin",
            };

            await _userRepository.UpdateUserInfoAsync(userToUpdate);
        }

        [TestMethod]
        public async Task UpdateUserInfo_OnlyName_Success()
        {
            var user = await GetActiveUserAsync();

            var userToUpdate = new User
            {
                Guid = user.Guid,
                Name = "newtestname",
                ModifiedOn = DateTime.UtcNow,
                ModifiedBy = "Admin",
            };

            await _userRepository.UpdateUserInfoAsync(userToUpdate);

            var updatedUser = await _context.Users.FirstOrDefaultAsync(x => x.Guid == userToUpdate.Guid);
            Assert.IsTrue(updatedUser != null && userToUpdate.Name == updatedUser.Name);
        }

        [TestMethod]
        public async Task UpdateUserInfo_OnlyGender_Success()
        {
            var user = await GetActiveUserAsync();

            var userToUpdate = new User
            {
                Guid = user.Guid,
                Gender = 1,
                ModifiedOn = DateTime.UtcNow,
                ModifiedBy = "Admin",
            };

            await _userRepository.UpdateUserInfoAsync(userToUpdate);

            var updatedUser = await _context.Users.FirstOrDefaultAsync(x => x.Guid == userToUpdate.Guid);
            Assert.IsTrue(updatedUser != null && userToUpdate.Gender == updatedUser.Gender);
        }

        [TestMethod]
        public async Task UpdateUserInfo_OnlyBirthday_Success()
        {
            var user = await GetActiveUserAsync();

            var userToUpdate = new User
            {
                Guid = user.Guid,
                Birthday = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow,
                ModifiedBy = "Admin",
            };

            await _userRepository.UpdateUserInfoAsync(userToUpdate);

            var updatedUser = await _context.Users.FirstOrDefaultAsync(x => x.Guid == userToUpdate.Guid);
            Assert.IsTrue(updatedUser != null && userToUpdate.Birthday == updatedUser.Birthday);
        }

        [TestMethod]
        public async Task ReadActiveUsersList_Success()
        {
            await CreateUsersRangeAsync();

            var users = await _userRepository.ReadActiveUsersListAsync();

            bool isOk = true;
            DateTime previousUserCreatedOn = default;
            foreach (var user in users)
            {
                if (user.RevokedOn != null)
                {
                    isOk = false;
                    break;
                }
                if (previousUserCreatedOn != default)
                {
                    if (user.CreatedOn > previousUserCreatedOn)
                    {
                        isOk = false;
                        break;
                    }
                }
            }

            Assert.IsTrue(isOk);
        }

        [TestMethod]
        public async Task ReadUserByGuid_Success()
        {
            var userToRead = await GetActiveUserAsync();

            var user = await _userRepository.ReadUserByGuidAsync(userToRead.Guid);
            Assert.IsNotNull(user);
            Assert.IsTrue(user.Guid == userToRead.Guid);
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public async Task ReadUserByGuid_Failure_UserNotFound()
        {
            var user = await _userRepository.ReadUserByGuidAsync(Guid.NewGuid());
        }

        [TestMethod]
        public async Task ReadUsersBornBeforeList_Success()
        {
            await CreateUsersRangeAsync();

            var testDate = DateTime.Parse("01/01/2009");

            var users = await _userRepository.ReadUsersBornBeforeListAsync(testDate);

            bool isOk = true;
            foreach (var user in users)
            {
                if (user.Birthday > testDate)
                {
                    isOk = false;
                    break;
                }
            }

            Assert.IsTrue(isOk);
        }

        [TestMethod]
        public async Task DeleteUserSoftly_Success()
        {
            var user = await GetActiveUserAsync();

            var userToDelete = new User
            {
                Guid = user.Guid,
                RevokedBy = "Admin",
                RevokedOn = DateTime.UtcNow,
            };

            await _userRepository.DeleteUserSoftlyAsync(userToDelete);

            var softlyDeletedUser = await _context.Users.FirstOrDefaultAsync(x => x.Guid == userToDelete.Guid);
            Assert.IsNotNull(softlyDeletedUser);
            Assert.IsTrue(softlyDeletedUser.RevokedBy == userToDelete.RevokedBy
                && softlyDeletedUser.RevokedOn == userToDelete.RevokedOn);
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public async Task DeleteUserSoftly_Failure_UserNotFound()
        {
            var userToDelete = new User
            {
                Guid = Guid.NewGuid(),
                RevokedBy = "Admin",
                RevokedOn = DateTime.UtcNow,
            };

            await _userRepository.DeleteUserSoftlyAsync(userToDelete);
        }

        [TestMethod]
        public async Task DeleteUserFully_Success()
        {
            var user = await GetActiveUserAsync();

            var userToDelete = new User
            {
                Guid = user.Guid,
            };

            await _userRepository.DeleteUserFullyAsync(userToDelete);

            var fullyDeletedUser = await _context.Users.FirstOrDefaultAsync(x => x.Guid == userToDelete.Guid);
            Assert.IsNull(fullyDeletedUser);
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public async Task DeleteUserFully_Failure_UserNotFound()
        {
            var userToDelete = new User
            {
                Guid = Guid.NewGuid(),
            };

            await _userRepository.DeleteUserFullyAsync(userToDelete);
        }

        [TestMethod]
        public async Task UpdateUserActiveStatus_Success()
        {
            var revokedUser = await GetRevokedUserAsync();

            var userToReactivate = new User
            {
                Guid = revokedUser.Guid,
                ModifiedBy = "Admin",
                ModifiedOn = DateTime.UtcNow,
            };

            await _userRepository.UpdateUserActiveStatusAsync(userToReactivate);

            var reactivatedUser = await _context.Users.FirstOrDefaultAsync(x => x.Guid == revokedUser.Guid);
            Assert.IsNotNull(reactivatedUser);
            Assert.IsTrue(reactivatedUser.RevokedOn == null && reactivatedUser.RevokedBy == null);
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public async Task UpdateUserActivateStatus_Failure_UserNotFound()
        {
            var userToReactivate = new User
            {
                Guid = Guid.NewGuid(),
                ModifiedBy = "Admin",
                ModifiedOn = DateTime.UtcNow,
            };

            await _userRepository.UpdateUserActiveStatusAsync(userToReactivate);
        }


        [TestMethod]
        public async Task IsUserActive_Success()
        {
            await CreateUsersRangeAsync();

            var activeUser = await GetActiveUserAsync();
            var revokedUser = await GetRevokedUserAsync();

            Assert.IsTrue(await _userRepository.IsUserActive(activeUser.Guid));
            Assert.IsFalse(await _userRepository.IsUserActive(revokedUser.Guid));
        }

        [TestMethod]
        public async Task IsUserAdmin_Success()
        {
            var notAdmin = await GetActiveUserAsync();
            var admin = await _context.Users.FirstOrDefaultAsync(x => x.Admin == true);

            Assert.IsTrue(await _userRepository.IsUserAdmin(admin!.Guid));
            Assert.IsFalse(await _userRepository.IsUserAdmin(notAdmin.Guid));
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public async Task IsUserAdmin_Failure_UserNotFound()
        {
            await _userRepository.IsUserAdmin(Guid.NewGuid());
        }

        private async Task CreateUsersRangeAsync()
        {
            var usersToCreate = new List<User>
            {
                new User
                {
                    Guid = Guid.NewGuid(),
                    Login = "testuser1",
                    PasswordHash = "testuser1",
                    Name = "testuser1",
                    Birthday = DateTime.Parse("01/01/1950"),
                    CreatedOn = DateTime.Parse("19/05/2023"),
                },
                new User
                {
                    Guid = Guid.NewGuid(),
                    Login = "testuser2",
                    PasswordHash = "testuser2",
                    Name = "testuser2",
                    Birthday = DateTime.Parse("01/01/1990"),
                    CreatedOn = DateTime.Parse("20/05/2023"),
                },
                new User
                {
                    Guid = Guid.NewGuid(),
                    Login = "testuser3",
                    PasswordHash = "testuser3",
                    Name = "testuser3",
                    Birthday = DateTime.Parse("01/01/2010"),
                    CreatedOn = DateTime.Parse("21/05/2023"),
                    RevokedOn = DateTime.UtcNow,
                    RevokedBy = "Admin",
                },
            };

            await _context.Users.AddRangeAsync(usersToCreate);
            await _context.SaveChangesAsync();
        }

        private async Task<User> GetActiveUserAsync()
        {
            var userToCreate = new User
            {
                Guid = Guid.NewGuid(),
                Login = "testuserlogin",
                PasswordHash = "testuserpassword",
                Name = "testusername",
                Gender = 2,
                Birthday = DateTime.UtcNow,
                Admin = false,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "Admin",
            };

            await _context.Users.AddAsync(userToCreate);
            await _context.SaveChangesAsync();

            _context.Entry(userToCreate).State = EntityState.Detached;
            return userToCreate;
        }

        private async Task<User> GetRevokedUserAsync()
        {
            var userToCreate = new User
            {
                Guid = Guid.NewGuid(),
                Login = "testrevokeduserlogin",
                PasswordHash = "testrevokeduserpassword",
                Name = "testrevokedusername",
                Gender = 2,
                Birthday = DateTime.UtcNow,
                Admin = false,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "Admin",
                RevokedOn = DateTime.UtcNow,
                RevokedBy = "Admin",
            };

            await _context.Users.AddAsync(userToCreate);
            await _context.SaveChangesAsync();

            _context.Entry(userToCreate).State = EntityState.Detached;
            return userToCreate;
        }
    }
}
