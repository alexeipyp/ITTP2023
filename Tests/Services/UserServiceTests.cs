using AutoMapper;
using BLL.AutoMapper;
using BLL.Models.User;
using BLL.Models.User.Create;
using BLL.Models.User.Delete;
using BLL.Models.User.Read;
using BLL.Models.User.Update;
using BLL.Models.User.ViewModels;
using BLL.Services;
using Common.Exceptions;
using DAL.Entities;
using DAL.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Services
{
    [TestClass]
    public class UserServiceTests
    {
        private Mock<UserRepository> _userRepositoryMock = null!;
        private IMapper _mapper = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _userRepositoryMock = new Mock<UserRepository>();
            var mapperConfig = new MapperConfiguration(c => c.AddProfile(new ModelsAndEntitiesMapperProfile()));
            _mapper = mapperConfig.CreateMapper();
        }

        [TestMethod]
        public async Task CreateUser_Success()
        {
            _userRepositoryMock.Setup(r => r.CreateUserAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            _userRepositoryMock.Setup(r => r.IsUserAdmin(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var testCreateUserModel = new CreateUserModel();
            await userService.CreateUserAsync(testCreateUserModel);
        }

        [TestMethod]
        [ExpectedException(typeof(OnlyAdminsException))]
        public async Task CreateUser_Failure_NotAnAdmin()
        {
            _userRepositoryMock.Setup(r => r.CreateUserAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            _userRepositoryMock.Setup(r => r.IsUserAdmin(It.IsAny<Guid>())).Returns(Task.FromResult(false));
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var testCreateUserModel = new CreateUserModel();
            await userService.CreateUserAsync(testCreateUserModel);
        }

        [TestMethod]
        [ExpectedException(typeof(NotUniqueLoginException))]
        public async Task CreateUser_Failure_RepositoryException_NotUniqueLogin()
        {
            _userRepositoryMock.Setup(r => r.CreateUserAsync(It.IsAny<User>())).Throws(new NotUniqueLoginException());
            _userRepositoryMock.Setup(r => r.IsUserAdmin(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var testCreateUserModel = new CreateUserModel();
            await userService.CreateUserAsync(testCreateUserModel);
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public async Task CreateUser_Failure_RepositoryException_RequesterNotFound()
        {
            _userRepositoryMock.Setup(r => r.IsUserAdmin(It.IsAny<Guid>())).Throws(new UserNotFoundException());
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var testCreateUserModel = new CreateUserModel();
            await userService.CreateUserAsync(testCreateUserModel);
        }

        [TestMethod]
        public async Task UpdateUserInfo_Success_SelfUserUpdate()
        {
            _userRepositoryMock.Setup(r => r.UpdateUserInfoAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            _userRepositoryMock.Setup(r => r.IsUserAdmin(It.IsAny<Guid>())).Returns(Task.FromResult(false));
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var testUserGuid = Guid.NewGuid();
            var testUpdateUserInfoModel = new UpdateUserInfoModel
            {
                RequesterGuid = testUserGuid,
                UserToUpdateGuid = testUserGuid,
            };
            await userService.UpdateUserInfoAsync(testUpdateUserInfoModel);
        }

        [TestMethod]
        public async Task UpdateUserInfo_Success_ByAdmin()
        {
            _userRepositoryMock.Setup(r => r.UpdateUserInfoAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            _userRepositoryMock.Setup(r => r.IsUserAdmin(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var testUpdateUserInfoModel = new UpdateUserInfoModel();
            await userService.UpdateUserInfoAsync(testUpdateUserInfoModel);
        }

        [TestMethod]
        [ExpectedException(typeof(OnlyAdminsException))]
        public async Task UpdateUserInfo_Failure_NotAnAdminOrNotSelfUserUpdate()
        {
            _userRepositoryMock.Setup(r => r.IsUserAdmin(It.IsAny<Guid>())).Returns(Task.FromResult(false));
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var testUpdateUserInfoModel = new UpdateUserInfoModel
            {
                RequesterGuid = Guid.NewGuid(),
                UserToUpdateGuid = Guid.NewGuid(),
            };
            await userService.UpdateUserInfoAsync(testUpdateUserInfoModel);
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public async Task UpdateUserInfo_Failure_RepositoryException_RequesterNotFound()
        {
            _userRepositoryMock.Setup(r => r.IsUserAdmin(It.IsAny<Guid>())).Throws(new UserNotFoundException());
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var testUpdateUserInfoModel = new UpdateUserInfoModel
            {
                RequesterGuid = Guid.NewGuid(),
                UserToUpdateGuid = Guid.NewGuid(),
            };
            await userService.UpdateUserInfoAsync(testUpdateUserInfoModel);
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public async Task UpdateUserInfo_Failure_RepositoryException_UserNotFound()
        {
            _userRepositoryMock.Setup(r => r.UpdateUserInfoAsync(It.IsAny<User>())).Throws(new UserNotFoundException());
            _userRepositoryMock.Setup(r => r.IsUserAdmin(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var testUpdateUserInfoModel = new UpdateUserInfoModel();
            await userService.UpdateUserInfoAsync(testUpdateUserInfoModel);
        }

        [TestMethod]
        public async Task UpdateUserPassword_Success_SelfUserUpdate()
        {
            _userRepositoryMock.Setup(r => r.UpdateUserPasswordAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            _userRepositoryMock.Setup(r => r.IsUserAdmin(It.IsAny<Guid>())).Returns(Task.FromResult(false));
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var testUserGuid = Guid.NewGuid();
            var testUpdateUserPasswordModel = new UpdateUserPasswordModel
            {
                RequesterGuid = testUserGuid,
                UserToUpdateGuid = testUserGuid,
            };
            await userService.UpdateUserPasswordAsync(testUpdateUserPasswordModel);
        }

        [TestMethod]
        public async Task UpdateUserPassword_Success_ByAdmin()
        {
            _userRepositoryMock.Setup(r => r.UpdateUserPasswordAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            _userRepositoryMock.Setup(r => r.IsUserAdmin(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var testUpdateUserPasswordModel = new UpdateUserPasswordModel();
            await userService.UpdateUserPasswordAsync(testUpdateUserPasswordModel);
        }

        [TestMethod]
        [ExpectedException(typeof(OnlyAdminsException))]
        public async Task UpdateUserPassword_Failure_NotAnAdminOrNotSelfUserUpdate()
        {
            _userRepositoryMock.Setup(r => r.IsUserAdmin(It.IsAny<Guid>())).Returns(Task.FromResult(false));
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var testUpdateUserPasswordModel = new UpdateUserPasswordModel
            {
                RequesterGuid = Guid.NewGuid(),
                UserToUpdateGuid = Guid.NewGuid(),
            };
            await userService.UpdateUserPasswordAsync(testUpdateUserPasswordModel);
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public async Task UpdateUserPassword_Failure_RepositoryException_RequesterNotFound()
        {
            _userRepositoryMock.Setup(r => r.IsUserAdmin(It.IsAny<Guid>())).Throws(new UserNotFoundException());
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var testUpdateUserPasswordModel = new UpdateUserPasswordModel
            {
                RequesterGuid = Guid.NewGuid(),
                UserToUpdateGuid = Guid.NewGuid(),
            };
            await userService.UpdateUserPasswordAsync(testUpdateUserPasswordModel);
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public async Task UpdateUserPassword_Failure_RepositoryException_UserNotFound()
        {
            _userRepositoryMock.Setup(r => r.UpdateUserPasswordAsync(It.IsAny<User>())).Throws(new UserNotFoundException());
            _userRepositoryMock.Setup(r => r.IsUserAdmin(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var testUpdateUserPasswordModel = new UpdateUserPasswordModel();
            await userService.UpdateUserPasswordAsync(testUpdateUserPasswordModel);
        }

        [TestMethod]
        public async Task UpdateUserLogin_Success_SelfUserUpdate()
        {
            _userRepositoryMock.Setup(r => r.UpdateUserLoginAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            _userRepositoryMock.Setup(r => r.IsUserAdmin(It.IsAny<Guid>())).Returns(Task.FromResult(false));
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var testUserGuid = Guid.NewGuid();
            var testUpdateUserLoginModel = new UpdateUserLoginModel
            {
                RequesterGuid = testUserGuid,
                UserToUpdateGuid = testUserGuid,
            };
            await userService.UpdateUserLoginAsync(testUpdateUserLoginModel);
        }

        [TestMethod]
        public async Task UpdateUserLogin_Success_ByAdmin()
        {
            _userRepositoryMock.Setup(r => r.UpdateUserLoginAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            _userRepositoryMock.Setup(r => r.IsUserAdmin(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var testUpdateUserLoginModel = new UpdateUserLoginModel();
            await userService.UpdateUserLoginAsync(testUpdateUserLoginModel);
        }

        [TestMethod]
        [ExpectedException(typeof(OnlyAdminsException))]
        public async Task UpdateUserLogin_Failure_NotAnAdminOrNotSelfUserUpdate()
        {
            _userRepositoryMock.Setup(r => r.IsUserAdmin(It.IsAny<Guid>())).Returns(Task.FromResult(false));
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var testUpdateUserLoginModel = new UpdateUserLoginModel
            {
                RequesterGuid = Guid.NewGuid(),
                UserToUpdateGuid = Guid.NewGuid(),
            };
            await userService.UpdateUserLoginAsync(testUpdateUserLoginModel);
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public async Task UpdateUserLogin_Failure_RepositoryException_RequesterNotFound()
        {
            _userRepositoryMock.Setup(r => r.IsUserAdmin(It.IsAny<Guid>())).Throws(new UserNotFoundException());
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var testUpdateUserLoginModel = new UpdateUserLoginModel
            {
                RequesterGuid = Guid.NewGuid(),
                UserToUpdateGuid = Guid.NewGuid(),
            };
            await userService.UpdateUserLoginAsync(testUpdateUserLoginModel);
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public async Task UpdateUserLogin_Failure_RepositoryException_UserNotFound()
        {
            _userRepositoryMock.Setup(r => r.UpdateUserLoginAsync(It.IsAny<User>())).Throws(new UserNotFoundException());
            _userRepositoryMock.Setup(r => r.IsUserAdmin(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var testUpdateUserLoginModel = new UpdateUserLoginModel();
            await userService.UpdateUserLoginAsync(testUpdateUserLoginModel);
        }

        [TestMethod]
        [ExpectedException(typeof(NotUniqueLoginException))]
        public async Task UpdateUserLogin_Failure_RepositoryException_NotUniqueLogin()
        {
            _userRepositoryMock.Setup(r => r.UpdateUserLoginAsync(It.IsAny<User>())).Throws(new NotUniqueLoginException());
            _userRepositoryMock.Setup(r => r.IsUserAdmin(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var testUpdateUserLoginModel = new UpdateUserLoginModel();
            await userService.UpdateUserLoginAsync(testUpdateUserLoginModel);
        }

        [TestMethod]
        public async Task ReadActiveUsersList_Success()
        {
            _userRepositoryMock.Setup(r => r.ReadActiveUsersListAsync()).Returns(Task.FromResult((IEnumerable<User>)new List<User>()));
            _userRepositoryMock.Setup(r => r.IsUserAdmin(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var requesterModel = new UserManipulationModel { RequesterGuid = Guid.NewGuid(), };
            await userService.ReadActiveUsersListAsync(requesterModel);
        }

        [TestMethod]
        [ExpectedException(typeof(OnlyAdminsException))]
        public async Task ReadActiveUsersList_Failure_NotAnAdmin()
        {
            _userRepositoryMock.Setup(r => r.ReadActiveUsersListAsync()).Returns(Task.FromResult((IEnumerable<User>)new List<User>()));
            _userRepositoryMock.Setup(r => r.IsUserAdmin(It.IsAny<Guid>())).Returns(Task.FromResult(false));
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var requesterModel = new UserManipulationModel { RequesterGuid = Guid.NewGuid(), };
            await userService.ReadActiveUsersListAsync(requesterModel);
        }

        [TestMethod]
        public async Task ReadCurrentUser_Success()
        {
            _userRepositoryMock.Setup(r => r.ReadUserByGuidAsync(It.IsAny<Guid>())).Returns(Task.FromResult(new User()));
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var requesterModel = new UserManipulationModel { RequesterGuid = Guid.NewGuid(), };
            await userService.ReadCurrentUserAsync(requesterModel);
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public async Task ReadCurrentUser_Failure_RepositoryException_UserNotFound()
        {
            _userRepositoryMock.Setup(r => r.ReadUserByGuidAsync(It.IsAny<Guid>())).Throws(new UserNotFoundException());
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var requesterModel = new UserManipulationModel { RequesterGuid = Guid.NewGuid(), };
            await userService.ReadCurrentUserAsync(requesterModel);
        }

        [TestMethod]
        public async Task ReadUserByGuid_Success()
        {
            _userRepositoryMock.Setup(r => r.ReadUserByGuidAsync(It.IsAny<Guid>())).Returns(Task.FromResult(new User()));
            _userRepositoryMock.Setup(r => r.IsUserAdmin(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var readUserByGuidModel = new ReadUserByGuidModel();
            await userService.ReadUserByGuidAsync(readUserByGuidModel);
        }

        [TestMethod]
        [ExpectedException(typeof(OnlyAdminsException))]
        public async Task ReadUserByGuid_Failure_NotAnAdmin()
        {
            _userRepositoryMock.Setup(r => r.ReadUserByGuidAsync(It.IsAny<Guid>())).Returns(Task.FromResult(new User()));
            _userRepositoryMock.Setup(r => r.IsUserAdmin(It.IsAny<Guid>())).Returns(Task.FromResult(false));
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var readUserByGuidModel = new ReadUserByGuidModel();
            await userService.ReadUserByGuidAsync(readUserByGuidModel);
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public async Task ReadUserByGuid_Failure_RequesterNotFound()
        {
            _userRepositoryMock.Setup(r => r.IsUserAdmin(It.IsAny<Guid>())).Throws(new UserNotFoundException());
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var readUserByGuidModel = new ReadUserByGuidModel();
            await userService.ReadUserByGuidAsync(readUserByGuidModel);
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public async Task ReadUserByGuid_Failure_UserNotFound()
        {
            _userRepositoryMock.Setup(r => r.ReadUserByGuidAsync(It.IsAny<Guid>())).Throws(new UserNotFoundException());
            _userRepositoryMock.Setup(r => r.IsUserAdmin(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var readUserByGuidModel = new ReadUserByGuidModel();
            await userService.ReadUserByGuidAsync(readUserByGuidModel);
        }

        [TestMethod]
        public async Task ReadUsersElderThanList_Success()
        {
            var users = new List<User>
            {
                new User
                {
                    Guid = Guid.NewGuid(),
                    Birthday = DateTime.Parse("26/05/2005")
                },
                new User
                {
                    Guid = Guid.NewGuid(),
                    Birthday = DateTime.Parse("25/05/2005")
                },
                new User
                {
                    Guid = Guid.NewGuid(),
                    Birthday = DateTime.Parse("25/05/1999")
                },
            };
            var selectedUsers = new List<User>();

            _userRepositoryMock.Setup(r => r.ReadUsersBornBeforeListAsync(It.IsAny<DateTime>()))
                .Callback<DateTime>(d =>
                {
                    selectedUsers = users.Where(x => x.Birthday <= d).ToList();
                })
                .Returns(Task.FromResult((IEnumerable<User>)selectedUsers));
            _userRepositoryMock.Setup(r => r.IsUserAdmin(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var dayBirthBefore = DateTime.Parse("26/05/2005");
            var readUsersElderThanModel = new ReadUsersElderThanModel
            {
                Age = 18,
                RequesterGuid = Guid.NewGuid(),
            };

            var res = await userService.ReadUsersElderThanListAsync(readUsersElderThanModel);
            var isOk = true;
            foreach (var user in res)
            {
                if (user.Birthday > dayBirthBefore)
                {
                    isOk = false;
                    break;
                }
            }
            Assert.IsTrue(isOk);
        }

        [TestMethod]
        [ExpectedException(typeof(OnlyAdminsException))]
        public async Task ReadUsersElderThanList_Failure_NotAnAdmin()
        {
            _userRepositoryMock.Setup(r => r.IsUserAdmin(It.IsAny<Guid>())).Throws(new OnlyAdminsException());
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var readUsersElderThanModel = new ReadUsersElderThanModel();
            await userService.ReadUsersElderThanListAsync(readUsersElderThanModel);
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public async Task ReadUsersElderThanList_Failure_RequesterNotFound()
        {
            _userRepositoryMock.Setup(r => r.IsUserAdmin(It.IsAny<Guid>())).Throws(new UserNotFoundException());
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var readUsersElderThanModel = new ReadUsersElderThanModel();
            await userService.ReadUsersElderThanListAsync(readUsersElderThanModel);
        }

        [TestMethod]
        public async Task DeleteUser_Success_Soft()
        {
            _userRepositoryMock.Setup(r => r.IsUserAdmin(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            _userRepositoryMock.Setup(r => r.DeleteUserSoftlyAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var deleteUserModel = new DeleteUserModel
            {
                IsSoft = true,
            };
            await userService.DeleteUserAsync(deleteUserModel);
        }

        [TestMethod]
        public async Task DeleteUser_Success_Full()
        {
            _userRepositoryMock.Setup(r => r.IsUserAdmin(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            _userRepositoryMock.Setup(r => r.DeleteUserFullyAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var deleteUserModel = new DeleteUserModel
            {
                IsSoft = false,
            };
            await userService.DeleteUserAsync(deleteUserModel);
        }

        [TestMethod]
        [ExpectedException(typeof(OnlyAdminsException))]
        public async Task DeleteUser_Failure_NotAnAdmin()
        {
            _userRepositoryMock.Setup(r => r.IsUserAdmin(It.IsAny<Guid>())).Returns(Task.FromResult(false));
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var deleteUserModel = new DeleteUserModel();
            await userService.DeleteUserAsync(deleteUserModel);
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public async Task DeleteUser_Failure_RequesterNotFound()
        {
            _userRepositoryMock.Setup(r => r.IsUserAdmin(It.IsAny<Guid>())).Throws(new UserNotFoundException());
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var deleteUserModel = new DeleteUserModel();
            await userService.DeleteUserAsync(deleteUserModel);
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public async Task DeleteUser_Failure_Soft_RepositoryException_UserNotFound()
        {
            _userRepositoryMock.Setup(r => r.IsUserAdmin(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            _userRepositoryMock.Setup(r => r.DeleteUserSoftlyAsync(It.IsAny<User>())).Throws(new UserNotFoundException());
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var deleteUserModel = new DeleteUserModel
            {
                IsSoft = true,
            };
            await userService.DeleteUserAsync(deleteUserModel);
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public async Task DeleteUser_Failure_Full_RepositoryException_UserNotFound()
        {
            _userRepositoryMock.Setup(r => r.IsUserAdmin(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            _userRepositoryMock.Setup(r => r.DeleteUserFullyAsync(It.IsAny<User>())).Throws(new UserNotFoundException());
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var deleteUserModel = new DeleteUserModel
            {
                IsSoft = false,
            };
            await userService.DeleteUserAsync(deleteUserModel);
            await userService.DeleteUserAsync(deleteUserModel);
        }

        [TestMethod]
        public async Task UpdateUserActiveStatus_Success()
        {
            _userRepositoryMock.Setup(r => r.IsUserAdmin(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            _userRepositoryMock.Setup(r => r.UpdateUserActiveStatusAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var updateUserActiveStatusModel = new UpdateUserActiveStatusModel();
            await userService.UpdateUserActiveStatusAsync(updateUserActiveStatusModel);
        }

        [TestMethod]
        [ExpectedException(typeof(OnlyAdminsException))]
        public async Task UpdateUserActiveStatus_Failure_NotAnAdmin()
        {
            _userRepositoryMock.Setup(r => r.IsUserAdmin(It.IsAny<Guid>())).Returns(Task.FromResult(false));
            _userRepositoryMock.Setup(r => r.UpdateUserActiveStatusAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var updateUserActiveStatusModel = new UpdateUserActiveStatusModel();
            await userService.UpdateUserActiveStatusAsync(updateUserActiveStatusModel);
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public async Task UpdateUserActiveStatus_Failure_RequesterNotFound()
        {
            _userRepositoryMock.Setup(r => r.IsUserAdmin(It.IsAny<Guid>())).Throws(new UserNotFoundException());
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var updateUserActiveStatusModel = new UpdateUserActiveStatusModel();
            await userService.UpdateUserActiveStatusAsync(updateUserActiveStatusModel);
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public async Task UpdateUserActiveStatus_Failure_RepositoryException_UserNotFound()
        {
            _userRepositoryMock.Setup(r => r.IsUserAdmin(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            _userRepositoryMock.Setup(r => r.UpdateUserActiveStatusAsync(It.IsAny<User>())).Throws(new UserNotFoundException());
            var userService = new UserService(_mapper, _userRepositoryMock.Object);

            var updateUserActiveStatusModel = new UpdateUserActiveStatusModel();
            await userService.UpdateUserActiveStatusAsync(updateUserActiveStatusModel);
        }

    }
}
