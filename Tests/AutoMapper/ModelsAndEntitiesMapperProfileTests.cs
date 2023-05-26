using AutoMapper;
using BLL.AutoMapper;
using BLL.Models.User.Create;
using BLL.Models.User.Delete;
using BLL.Models.User.Update;
using BLL.Models.User.ViewModels;
using Common.Utils;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.AutoMapper
{
    [TestClass]
    public class ModelsAndEntitiesMapperProfileTests
    {
        private MapperConfiguration _configuration = null!;
        private IMapper _mapper = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _configuration = new MapperConfiguration(cfg => cfg.AddProfile(new ModelsAndEntitiesMapperProfile()));
            _mapper = _configuration.CreateMapper();
        }

        [TestMethod]
        public void MapperProfileConfigurationCheck_Success()
        {
            _configuration.AssertConfigurationIsValid();
        }

        [TestMethod]
        public void CreateUserModelToUser_Success()
        {
            var createUserModel = new CreateUserModel
            {
                RequesterGuid = Guid.NewGuid(),
                Login = "testlogin",
                Password = "testpassword",
                Name = "testname",
                Gender = 1,
                Birthday = DateTime.Parse("01/01/1970").ToUniversalTime(),
                Admin = false,
                CreatedBy = "Admin",
            };

            var user = _mapper.Map<DAL.Entities.User>(createUserModel);

            Assert.IsNotNull(user);
            Assert.AreEqual(createUserModel.Login, user.Login);
            Assert.AreEqual(HashHelper.GetHash(createUserModel.Password), user.PasswordHash);
            Assert.AreEqual(createUserModel.Name, user.Name);
            Assert.AreEqual(createUserModel.Gender, user.Gender);
            Assert.AreEqual(createUserModel.Birthday, user.Birthday);
            Assert.AreEqual(createUserModel.Admin, user.Admin);
            Assert.AreEqual(createUserModel.CreatedBy, user.CreatedBy);
            Assert.AreNotEqual(user.Guid, default);
            Assert.AreNotEqual(user.CreatedOn, default);
        }

        [TestMethod]
        public void UpdateUserInfoModelToUser_Success()
        {
            var updateUserInfoModel = new UpdateUserInfoModel
            {
                RequesterGuid = Guid.NewGuid(),
                UserToUpdateGuid = Guid.NewGuid(),
                ModifiedBy = "Admin",
                Birthday = DateTime.Parse("01/01/1970").ToUniversalTime(),
                Gender = 0,
                Name = "newtestname",
            };

            var user = _mapper.Map<DAL.Entities.User>(updateUserInfoModel);

            Assert.IsNotNull(user);
            Assert.AreEqual(updateUserInfoModel.UserToUpdateGuid, user.Guid);
            Assert.AreEqual(updateUserInfoModel.ModifiedBy, user.ModifiedBy);
            Assert.AreNotEqual(user.ModifiedOn, default);
            Assert.AreEqual(updateUserInfoModel.Birthday, user.Birthday);
            Assert.AreEqual(updateUserInfoModel.Gender, user.Gender);
            Assert.AreEqual(updateUserInfoModel.Name, user.Name);
        }

        [TestMethod]
        public void UpdateUserPasswordModelToUser_Success()
        {
            var updateUserPasswordModel = new UpdateUserPasswordModel
            {
                RequesterGuid = Guid.NewGuid(),
                UserToUpdateGuid = Guid.NewGuid(),
                ModifiedBy = "Admin",
                Password = "newtestpassword",
            };

            var user = _mapper.Map<DAL.Entities.User>(updateUserPasswordModel);

            Assert.IsNotNull(user);
            Assert.AreEqual(updateUserPasswordModel.UserToUpdateGuid, user.Guid);
            Assert.AreEqual(updateUserPasswordModel.ModifiedBy, user.ModifiedBy);
            Assert.AreNotEqual(user.ModifiedOn, default);
            Assert.AreEqual(HashHelper.GetHash(updateUserPasswordModel.Password), user.PasswordHash);
        }

        [TestMethod]
        public void UpdateUserLoginModelToUser_Success()
        {
            var updateUserLoginModel = new UpdateUserLoginModel
            {
                RequesterGuid = Guid.NewGuid(),
                UserToUpdateGuid = Guid.NewGuid(),
                ModifiedBy = "Admin",
                Login = "newtestlogin",
            };

            var user = _mapper.Map<DAL.Entities.User>(updateUserLoginModel);

            Assert.IsNotNull(user);
            Assert.AreEqual(updateUserLoginModel.UserToUpdateGuid, user.Guid);
            Assert.AreEqual(updateUserLoginModel.ModifiedBy, user.ModifiedBy);
            Assert.AreNotEqual(user.ModifiedOn, default);
            Assert.AreEqual(updateUserLoginModel.Login, user.Login);
        }

        [TestMethod]
        public void UpdateUserActiveStatusModelToUser_Success()
        {
            var updateUserActiveStatusModel = new UpdateUserActiveStatusModel
            {
                RequesterGuid = Guid.NewGuid(),
                UserToUpdateGuid = Guid.NewGuid(),
                ModifiedBy = "Admin",
            };

            var user = _mapper.Map<DAL.Entities.User>(updateUserActiveStatusModel);

            Assert.IsNotNull(user);
            Assert.AreEqual(updateUserActiveStatusModel.UserToUpdateGuid, user.Guid);
            Assert.AreEqual(updateUserActiveStatusModel.ModifiedBy, user.ModifiedBy);
            Assert.AreNotEqual(user.ModifiedOn, default);
        }

        [TestMethod]
        public void DeleteUserModelToUser_Success()
        {
            var deleteUserModel = new DeleteUserModel
            {
                RequesterGuid = Guid.NewGuid(),
                UserToDeleteGuid = Guid.NewGuid(),
                IsSoft = true,
                RevokedBy = "Admin",
            };

            var user = _mapper.Map<DAL.Entities.User>(deleteUserModel);

            Assert.IsNotNull(user);
            Assert.AreEqual(deleteUserModel.UserToDeleteGuid, user.Guid);
            Assert.AreEqual(deleteUserModel.RevokedBy, user.RevokedBy);
            Assert.AreNotEqual(user.RevokedOn, default);
        }

        [TestMethod]
        public void UserToUserDetailedViewModel_Success()
        {
            var user = GetUser();

            var userDetailedViewModel = _mapper.Map<UserDetailedViewModel>(user);

            Assert.IsNotNull(userDetailedViewModel);
            Assert.AreEqual(userDetailedViewModel.Guid, user.Guid);
            Assert.AreEqual(userDetailedViewModel.Login, user.Login);
            Assert.AreEqual(userDetailedViewModel.Name, user.Name);
            Assert.AreEqual(userDetailedViewModel.Gender, user.Gender);
            Assert.AreEqual(userDetailedViewModel.Birthday, user.Birthday);
            Assert.AreEqual(userDetailedViewModel.Admin, user.Admin);
            Assert.AreEqual(userDetailedViewModel.CreatedBy, user.CreatedBy);
            Assert.AreEqual(userDetailedViewModel.CreatedOn, user.CreatedOn);
            Assert.AreEqual(userDetailedViewModel.ModifiedBy, user.ModifiedBy);
            Assert.AreEqual(userDetailedViewModel.ModifiedOn, user.ModifiedOn);
        }

        [TestMethod]
        public void UserToUserFullViewModel_Success()
        {
            var user = GetUser();

            var userFullViewModel = _mapper.Map<UserFullViewModel>(user);

            Assert.IsNotNull(userFullViewModel);
            Assert.AreEqual(userFullViewModel.Guid, user.Guid);
            Assert.AreEqual(userFullViewModel.Login, user.Login);
            Assert.AreEqual(userFullViewModel.Name, user.Name);
            Assert.AreEqual(userFullViewModel.Gender, user.Gender);
            Assert.AreEqual(userFullViewModel.Birthday, user.Birthday);
            Assert.AreEqual(userFullViewModel.Admin, user.Admin);
            Assert.AreEqual(userFullViewModel.CreatedBy, user.CreatedBy);
            Assert.AreEqual(userFullViewModel.CreatedOn, user.CreatedOn);
            Assert.AreEqual(userFullViewModel.ModifiedBy, user.ModifiedBy);
            Assert.AreEqual(userFullViewModel.ModifiedOn, user.ModifiedOn);
            Assert.AreEqual(userFullViewModel.RevokedBy, user.RevokedBy);
            Assert.AreEqual(userFullViewModel.RevokedOn, user.RevokedOn);
        }

        [TestMethod]
        public void UserToUserViewModel_Success()
        {
            var user = GetUser();

            var userViewModel = _mapper.Map<UserViewModel>(user);

            Assert.IsNotNull(userViewModel);
            Assert.AreEqual(userViewModel.Name, user.Name);
            Assert.AreEqual(userViewModel.Gender, user.Gender);
            Assert.AreEqual(userViewModel.Birthday, user.Birthday);
            Assert.AreEqual(userViewModel.IsActive, true);
        }

        private User GetUser()
            => new User
            {
                Guid = Guid.NewGuid(),
                Login = "testlogin",
                PasswordHash = "testpasshash",
                Name = "testname",
                Gender = 1,
                Birthday = DateTime.Parse("01/01/1970").ToUniversalTime(),
                Admin = false,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "Admin",
            };
    }
}
