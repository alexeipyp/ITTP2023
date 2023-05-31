using API.AutoMapper;
using API.Models.User.Create;
using API.Models.User.Delete;
using API.Models.User.Update;
using AutoMapper;
using BLL.Models.User.Create;
using BLL.Models.User.Delete;
using BLL.Models.User.Update;
using Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.AutoMapper
{
    [TestClass]
    public class HTTPDTOsToBLLModelsMapperProfileTests
    {
        private MapperConfiguration _configuration = null!;
        private IMapper _mapper = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _configuration = new MapperConfiguration(cfg => cfg.AddProfile(new HTTPDTOsToBLLModelsMapperProfile()));
            _mapper = _configuration.CreateMapper();
        }

        [TestMethod]
        public void MapperProfileConfigurationCheck_Success()
        {
            _configuration.AssertConfigurationIsValid();
        }

        [TestMethod]
        public void CreateUserRequestToCreateUserModel_Success()
        {
            var createUserRequest = new CreateUserRequest
            {
                Login = "testlogin",
                Password = "testpassword",
                Gender = 2,
                Birthday = DateTime.Parse("05/11/2013"),
                Admin = false,
                Name = "testname",
            };

            var createUserModel = _mapper.Map<CreateUserModel>(createUserRequest);

            Assert.IsNotNull(createUserModel);
            Assert.AreEqual(createUserModel.Login, createUserModel.Login);
            Assert.AreEqual(createUserModel.Password, createUserRequest.Password);
            Assert.AreEqual(createUserModel.Name, createUserRequest.Name);
            Assert.AreEqual(createUserModel.Gender, createUserRequest.Gender);
            Assert.AreEqual(createUserModel.Birthday, createUserRequest.Birthday);
            Assert.AreEqual(createUserModel.Admin, createUserRequest.Admin);
        }

        [TestMethod]
        public void UpdateUserInfoRequestToUpdateUserInfoModel_Success()
        {
            var updateUserInfoRequest = new UpdateUserInfoRequest
            {
                UserToUpdateGuid = Guid.NewGuid(),
                Name = "testname",
                Gender = 1,
                Birthday = DateTime.Parse("02/06/1992"),
            };

            var updateUserInfoModel = _mapper.Map<UpdateUserInfoModel>(updateUserInfoRequest);

            Assert.IsNotNull(updateUserInfoModel);
            Assert.AreEqual(updateUserInfoModel.UserToUpdateGuid, updateUserInfoRequest.UserToUpdateGuid);
            Assert.AreEqual(updateUserInfoModel.Name, updateUserInfoRequest.Name);
            Assert.AreEqual(updateUserInfoModel.Gender, updateUserInfoRequest.Gender);
            Assert.AreEqual(updateUserInfoModel.Birthday, updateUserInfoRequest.Birthday);
        }

        [TestMethod]
        public void UpdateUserPasswordRequestToUpdateUserPasswordModel_Success()
        {
            var updateUserPasswordRequest = new UpdateUserPasswordRequest
            {
                UserToUpdateGuid = Guid.NewGuid(),
                Password = "testpassword",
            };

            var updateUserPasswordModel = _mapper.Map<UpdateUserPasswordModel>(updateUserPasswordRequest);

            Assert.IsNotNull(updateUserPasswordModel);
            Assert.AreEqual(updateUserPasswordModel.UserToUpdateGuid, updateUserPasswordRequest.UserToUpdateGuid);
            Assert.AreEqual(updateUserPasswordModel.Password, updateUserPasswordRequest.Password);
        }

        [TestMethod]
        public void UpdateUserLoginRequestToUpdateUserLoginModel_Success()
        {
            var updateUserLoginRequest = new UpdateUserLoginRequest
            {
                UserToUpdateGuid = Guid.NewGuid(),
                Login = "testlogin",
            };

            var updateUserLoginModel = _mapper.Map<UpdateUserLoginModel>(updateUserLoginRequest);

            Assert.IsNotNull(updateUserLoginModel);
            Assert.AreEqual(updateUserLoginModel.UserToUpdateGuid, updateUserLoginRequest.UserToUpdateGuid);
            Assert.AreEqual(updateUserLoginModel.Login, updateUserLoginRequest.Login);
        }

        [TestMethod]
        public void UpdateUserActiveStatusRequestToUpdateUserActiveStatusModel_Success()
        {
            var updateUserActiveStatusRequest = new UpdateUserActiveStatusRequest
            {
                UserToUpdateGuid = Guid.NewGuid(),
            };

            var updateUserActiveStatusModel = _mapper.Map<UpdateUserActiveStatusModel>(updateUserActiveStatusRequest);

            Assert.IsNotNull(updateUserActiveStatusModel);
            Assert.AreEqual(updateUserActiveStatusModel.UserToUpdateGuid, updateUserActiveStatusRequest.UserToUpdateGuid);
        }

        [TestMethod]
        public void DeleteUserRequestToDeleteUserModel_Success()
        {
            var deleteUserRequest = new DeleteUserRequest
            {
                UserToDeleteGuid = Guid.NewGuid(),
                IsSoft = true,
            };

            var deleteUserModel = _mapper.Map<DeleteUserModel>(deleteUserRequest);

            Assert.IsNotNull(deleteUserModel);
            Assert.AreEqual(deleteUserModel.UserToDeleteGuid, deleteUserRequest.UserToDeleteGuid);
            Assert.AreEqual(deleteUserModel.IsSoft, deleteUserRequest.IsSoft);
        }
    }
}
