using API.Models.User.Create;
using API.Models.User.Delete;
using API.Models.User.Update;
using AutoMapper;
using BLL.Models.User.Create;
using BLL.Models.User.Delete;
using BLL.Models.User.Read;
using BLL.Models.User.Update;

namespace API.AutoMapper
{
    public class HTTPDTOsToBLLModelsMapperProfile : Profile
    {
        public HTTPDTOsToBLLModelsMapperProfile()
        {
            CreateMap<CreateUserRequest, CreateUserModel>()
                .ForMember(d => d.CreatedBy, m => m.Ignore())
                .ForMember(d => d.RequesterGuid, m => m.Ignore())
                ;

            CreateMap<UpdateUserRequest, UpdateUserModel>()
                .ForMember(d => d.RequesterGuid, m => m.Ignore())
                .ForMember(d => d.ModifiedBy, m => m.Ignore())
                .IncludeAllDerived()
                ;
            CreateMap<UpdateUserInfoRequest, UpdateUserInfoModel>()
                ;
            CreateMap<UpdateCurrentUserInfoRequest, UpdateUserInfoModel>()
                .ForMember(d => d.ModifiedBy, m => m.Ignore())
                .ForMember(d => d.RequesterGuid, m => m.Ignore())
                .ForMember(d => d.UserToUpdateGuid, m => m.Ignore())
                ;
            CreateMap<UpdateUserPasswordRequest, UpdateUserPasswordModel>()
                ;
            CreateMap<UpdateUserLoginRequest, UpdateUserLoginModel>()
                ;
            CreateMap<UpdateUserActiveStatusRequest, UpdateUserActiveStatusModel>()
                ;

            CreateMap<DeleteUserRequest, DeleteUserModel>()
                .ForMember(d => d.RequesterGuid, m => m.Ignore())
                .ForMember(d => d.RevokedBy, m => m.Ignore())
                ;
        }
    }
}
