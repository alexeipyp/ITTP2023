using AutoMapper;
using BLL.AutoMapper;
using BLL.Models.User;
using BLL.Models.User.Create;
using BLL.Models.User.Delete;
using BLL.Models.User.Read;
using BLL.Models.User.Update;
using BLL.Models.User.ViewModels;
using Common.Exceptions;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IMapper mapper, UserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task CreateUserAsync(CreateUserModel model)
        {
            await CheckOnlyAdminsPermission(model);

            var userToCreate = _mapper.Map<DAL.Entities.User>(model);
            await _userRepository.CreateUserAsync(userToCreate);
        }

        public async Task UpdateUserInfoAsync(UpdateUserInfoModel model)
        {
            await CheckUpdatePermission(model);

            var userToUpdate = _mapper.Map<DAL.Entities.User>(model);
            await _userRepository.UpdateUserInfoAsync(userToUpdate);
        }

        public async Task UpdateUserPasswordAsync(UpdateUserPasswordModel model)
        {
            await CheckUpdatePermission(model);

            var userToUpdate = _mapper.Map<DAL.Entities.User>(model);
            await _userRepository.UpdateUserPasswordAsync(userToUpdate);
        }

        public async Task UpdateUserLoginAsync(UpdateUserLoginModel model)
        {
            await CheckUpdatePermission(model);

            var userToUpdate = _mapper.Map<DAL.Entities.User>(model);
            await _userRepository.UpdateUserLoginAsync(userToUpdate);
        }

        public async Task<IEnumerable<UserDetailedViewModel>> ReadActiveUsersListAsync(UserManipulationModel model)
        {
            await CheckOnlyAdminsPermission(model);

            var users = await _userRepository.ReadActiveUsersListAsync();
            var userModels = users.Select(x => _mapper.Map<UserDetailedViewModel>(x)).ToList();
            return userModels;
        }

        public async Task<UserDetailedViewModel> ReadCurrentUserAsync(UserManipulationModel model)
        {
            var user = await _userRepository.ReadUserByGuidAsync(model.RequesterGuid);
            var userModel = _mapper.Map<UserDetailedViewModel>(user);
            return userModel;
        }

        public async Task<UserViewModel> ReadUserByGuidAsync(ReadUserByGuidModel model)
        {
            await CheckOnlyAdminsPermission(model);

            var user = await _userRepository.ReadUserByGuidAsync(model.UserToReadGuid);
            var userModel = _mapper.Map<UserViewModel>(user);
            return userModel;
        }

        public async Task<IEnumerable<UserFullViewModel>> ReadUsersElderThanListAsync(ReadUsersElderThanModel model)
        {
            await CheckOnlyAdminsPermission(model);

            var dayBornBefore = DateTime.UtcNow.AddYears(-model.Age);
            var users = await _userRepository.ReadUsersBornBeforeListAsync(dayBornBefore);
            var userModels = users.Select(x => _mapper.Map<UserFullViewModel>(x)).ToList();
            return userModels;
        }

        public async Task DeleteUserAsync(DeleteUserModel model)
        {
            await CheckOnlyAdminsPermission(model);

            var userToDelete = _mapper.Map<DAL.Entities.User>(model);

            if (model.IsSoft)
            {
                await _userRepository.DeleteUserSoftlyAsync(userToDelete);
            }
            else
            {
                await _userRepository.DeleteUserFullyAsync(userToDelete);
            }
        }

        public async Task UpdateUserActiveStatusAsync(UpdateUserActiveStatusModel model)
        {
            await CheckOnlyAdminsPermission(model);

            var userToRestore = _mapper.Map<DAL.Entities.User>(model);
            await _userRepository.UpdateUserActiveStatusAsync(userToRestore);
        }

        private async Task CheckUpdatePermission(UpdateUserModel model)
        {
            if (model.RequesterGuid != model.UserToUpdateGuid)
            {
                if (!(await _userRepository.IsUserAdmin(model.RequesterGuid)))
                {
                    throw new OnlyAdminsException();
                }
            }
        }

        private async Task CheckOnlyAdminsPermission(UserManipulationModel model)
        {
            if (!(await _userRepository.IsUserAdmin(model.RequesterGuid)))
            {
                throw new OnlyAdminsException();
            }
        }
    }
}
