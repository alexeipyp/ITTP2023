using API.Models.User.Create;
using API.Models.User.Delete;
using API.Models.User.Update;
using AutoMapper;
using BLL.Models.User;
using BLL.Models.User.Create;
using BLL.Models.User.Delete;
using BLL.Models.User.Read;
using BLL.Models.User.Update;
using BLL.Models.User.ViewModels;
using BLL.Services;
using Common.Consts;
using Common.Exceptions;
using Common.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [ApiExplorerSettings(GroupName = "Api")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IMapper _mapper;

        public UserController(UserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize]
        public async Task CreateUser(CreateUserRequest request)
        {
            var requesterGuid = User.GetClaimValue<Guid>(ClaimNames.RequesterGuid);
            var requesterLogin = User.GetClaimValue<string>(ClaimNames.RequesterLogin);
            if (requesterGuid != default && requesterLogin != default)
            {
                var createUserModel = _mapper.Map<CreateUserModel>(request, o => o.AfterMap((s, d) =>
                {
                    d.RequesterGuid = requesterGuid;
                    d.CreatedBy = requesterLogin;
                }));
                await _userService.CreateUserAsync(createUserModel);
            }
            else
            {
                throw new UnauthorizedException();
            }
        }

        [HttpPost]
        [Authorize]
        public async Task UpdateUserInfo(UpdateUserInfoRequest request)
        {
            var requesterGuid = User.GetClaimValue<Guid>(ClaimNames.RequesterGuid);
            var requesterLogin = User.GetClaimValue<string>(ClaimNames.RequesterLogin);
            if (requesterGuid != default && requesterLogin != default)
            {
                var updateUserInfoModel = _mapper.Map<UpdateUserInfoModel>(request, o => o.AfterMap((s, d) =>
                {
                    d.RequesterGuid = requesterGuid;
                    d.ModifiedBy = requesterLogin;
                }));
                await _userService.UpdateUserInfoAsync(updateUserInfoModel);
            }
            else
            {
                throw new UnauthorizedException();
            }
        }

        [HttpPost]
        [Authorize]
        public async Task UpdateCurrentUserInfo(UpdateCurrentUserInfoRequest request)
        {
            var requesterGuid = User.GetClaimValue<Guid>(ClaimNames.RequesterGuid);
            var requesterLogin = User.GetClaimValue<string>(ClaimNames.RequesterLogin);
            if (requesterGuid != default && requesterLogin != default)
            {
                var updateUserInfoModel = _mapper.Map<UpdateUserInfoModel>(request, o => o.AfterMap((s, d) =>
                {
                    d.RequesterGuid = requesterGuid;
                    d.ModifiedBy = requesterLogin;
                    d.UserToUpdateGuid = requesterGuid;
                }));
                await _userService.UpdateUserInfoAsync(updateUserInfoModel);
            }
            else
            {
                throw new UnauthorizedException();
            }
        }

        [HttpPost]
        [Authorize]
        public async Task UpdateUserPassword(UpdateUserPasswordRequest request)
        {
            var requesterGuid = User.GetClaimValue<Guid>(ClaimNames.RequesterGuid);
            var requesterLogin = User.GetClaimValue<string>(ClaimNames.RequesterLogin);
            if (requesterGuid != default && requesterLogin != default)
            {
                var updateUserPasswordModel = _mapper.Map<UpdateUserPasswordModel>(request, o => o.AfterMap((s, d) =>
                {
                    d.RequesterGuid = requesterGuid;
                    d.ModifiedBy = requesterLogin;
                }));
                await _userService.UpdateUserPasswordAsync(updateUserPasswordModel);
            }
            else
            {
                throw new UnauthorizedException();
            }
        }

        [HttpPost]
        [Authorize]
        public async Task UpdateCurrentUserPassword([RegularExpression(@"[0-9A-Za-z]+",
            ErrorMessage = "Запрещены все символы кроме латинских букв и цифр")] string password)
        {
            var requesterGuid = User.GetClaimValue<Guid>(ClaimNames.RequesterGuid);
            var requesterLogin = User.GetClaimValue<string>(ClaimNames.RequesterLogin);
            if (requesterGuid != default && requesterLogin != default)
            {
                var updateUserPasswordModel = new UpdateUserPasswordModel
                {
                    RequesterGuid = requesterGuid,
                    UserToUpdateGuid = requesterGuid,
                    ModifiedBy = requesterLogin,
                    Password = password,
                };

                await _userService.UpdateUserPasswordAsync(updateUserPasswordModel);
            }
            else
            {
                throw new UnauthorizedException();
            }
        }

        [HttpPost]
        [Authorize]
        public async Task UpdateUserLogin(UpdateUserLoginRequest request)
        {
            var requesterGuid = User.GetClaimValue<Guid>(ClaimNames.RequesterGuid);
            var requesterLogin = User.GetClaimValue<string>(ClaimNames.RequesterLogin);
            if (requesterGuid != default && requesterLogin != default)
            {
                var updateUserLoginModel = _mapper.Map<UpdateUserLoginModel>(request, o => o.AfterMap((s, d) =>
                {
                    d.RequesterGuid = requesterGuid;
                    d.ModifiedBy = requesterLogin;
                }));
                await _userService.UpdateUserLoginAsync(updateUserLoginModel);
            }
            else
            {
                throw new UnauthorizedException();
            }
        }

        [HttpPost]
        [Authorize]
        public async Task UpdateCurrentUserLogin([RegularExpression(@"[0-9A-Za-z]+",
            ErrorMessage = "Запрещены все символы кроме латинских букв и цифр")] string login)
        {
            var requesterGuid = User.GetClaimValue<Guid>(ClaimNames.RequesterGuid);
            var requesterLogin = User.GetClaimValue<string>(ClaimNames.RequesterLogin);
            if (requesterGuid != default && requesterLogin != default)
            {
                var updateUserLoginModel = new UpdateUserLoginModel
                {
                    RequesterGuid = requesterGuid,
                    UserToUpdateGuid = requesterGuid,
                    ModifiedBy = requesterLogin,
                    Login = login,
                };
                await _userService.UpdateUserLoginAsync(updateUserLoginModel);
            }
            else
            {
                throw new UnauthorizedException();
            }
        }

        [HttpPost]
        [Authorize]
        public async Task UpdateUserActiveStatus(UpdateUserActiveStatusRequest request)
        {
            var requesterGuid = User.GetClaimValue<Guid>(ClaimNames.RequesterGuid);
            var requesterLogin = User.GetClaimValue<string>(ClaimNames.RequesterLogin);
            if (requesterGuid != default && requesterLogin != default)
            {
                var updateUserActiveStatusModel = _mapper.Map<UpdateUserActiveStatusModel>(request, o => o.AfterMap((s, d) =>
                {
                    d.RequesterGuid = requesterGuid;
                    d.ModifiedBy = requesterLogin;
                }));
                await _userService.UpdateUserActiveStatusAsync(updateUserActiveStatusModel);
            }
            else
            {
                throw new UnauthorizedException();
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<UserDetailedViewModel> ReadCurrentUser()
        {
            var requesterGuid = User.GetClaimValue<Guid>(ClaimNames.RequesterGuid);
            if (requesterGuid != default)
            {
                return await _userService.ReadCurrentUserAsync(new UserManipulationModel { RequesterGuid = requesterGuid, });
            }
            else
            {
                throw new UnauthorizedException();
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<UserDetailedViewModel>> ReadActiveUsers()
        {
            var requesterGuid = User.GetClaimValue<Guid>(ClaimNames.RequesterGuid);
            if (requesterGuid != default)
            {
                return await _userService.ReadActiveUsersListAsync(new UserManipulationModel { RequesterGuid = requesterGuid, });
            }
            else
            {
                throw new UnauthorizedException();
            }
        }

        [HttpGet]
        [Route("{userToReadLogin}")]
        [Authorize]
        public async Task<UserViewModel> ReadUser([RegularExpression(@"[0-9A-Za-z]+",
            ErrorMessage = "Запрещены все символы кроме латинских букв и цифр")] string userToReadLogin)
        {
            var requesterGuid = User.GetClaimValue<Guid>(ClaimNames.RequesterGuid);
            if (requesterGuid != default)
            {
                var readUserModel = new ReadUserByLoginModel 
                { 
                    RequesterGuid = requesterGuid, 
                    UserToReadLogin = userToReadLogin,
                };
                return await _userService.ReadUserByLoginAsync(readUserModel);
            }
            else
            {
                throw new UnauthorizedException();
            }
        }

        [HttpGet]
        [Route("{age}")]
        [Authorize]
        public async Task<IEnumerable<UserFullViewModel>> ReadUsersElderThan([Range(1, 110, ErrorMessage = "Недопустимый возраст")]  int age)
        {
            var requesterGuid = User.GetClaimValue<Guid>(ClaimNames.RequesterGuid);
            if (requesterGuid != default)
            {
                var readUserElderThanModel = new ReadUsersElderThanModel
                {
                    RequesterGuid = requesterGuid,
                    Age = age,
                };
                return await _userService.ReadUsersElderThanListAsync(readUserElderThanModel);
            }
            else
            {
                throw new UnauthorizedException();
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task DeleteUser(DeleteUserRequest request)
        {
            var requesterGuid = User.GetClaimValue<Guid>(ClaimNames.RequesterGuid);
            var requesterLogin = User.GetClaimValue<string>(ClaimNames.RequesterLogin);
            if (requesterGuid != default && requesterLogin != default)
            {
                var deleteUserModel = _mapper.Map<DeleteUserModel>(request, o => o.AfterMap((s, d) =>
                {
                    d.RequesterGuid = requesterGuid;
                    d.RevokedBy = requesterLogin;
                }));
                await _userService.DeleteUserAsync(deleteUserModel);
            }
            else
            {
                throw new UnauthorizedException();
            }
        }
    }
}