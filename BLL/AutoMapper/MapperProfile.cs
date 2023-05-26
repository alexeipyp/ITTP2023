using AutoMapper;
using BLL.Models.User.Create;
using BLL.Models.User.Delete;
using BLL.Models.User.Read;
using BLL.Models.User.Update;
using BLL.Models.User.ViewModels;
using Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.AutoMapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CreateUserModel, DAL.Entities.User>()
                .ForMember(d => d.Guid, m => m.MapFrom(s => Guid.NewGuid()))
                .ForMember(d => d.PasswordHash, m => m.MapFrom(s => HashHelper.GetHash(s.Password)))
                .ForMember(d => d.Birthday, m => m.MapFrom(s => s.Birthday.HasValue ? s.Birthday!.Value.ToUniversalTime() : (DateTime?)null))
                .ForMember(d => d.CreatedOn, m => m.MapFrom(s => DateTime.UtcNow))
                ;

            CreateMap<UpdateUserModel, DAL.Entities.User>()
                .ForMember(d => d.Guid, m => m.MapFrom(s => s.UserToUpdateGuid))
                .ForMember(d => d.ModifiedOn, m => m.MapFrom(s => DateTime.UtcNow))
                ;
            CreateMap<UpdateUserInfoModel, DAL.Entities.User>()
                .IncludeBase<UpdateUserModel, DAL.Entities.User>()
                ;
            CreateMap<UpdateUserLoginModel, DAL.Entities.User>()
                .IncludeBase<UpdateUserModel, DAL.Entities.User>()
                ;
            CreateMap<UpdateUserPasswordModel, DAL.Entities.User>()
                .IncludeBase<UpdateUserModel, DAL.Entities.User>()
                ;
            CreateMap<UpdateUserActiveStatusModel, DAL.Entities.User>()
                .IncludeBase<UpdateUserModel, DAL.Entities.User>()
                ;

            CreateMap<DeleteUserModel, DAL.Entities.User>()
                .ForMember(d => d.Guid, m => m.MapFrom(s => s.UserToDeleteGuid))
                ;

            CreateMap<DAL.Entities.User, UserDetailedViewModel>();
            CreateMap<DAL.Entities.User, UserFullViewModel>();
            CreateMap<DAL.Entities.User, UserViewModel>()
                .ForMember(d => d.IsActive, m => m.MapFrom(s => s.RevokedBy == null && s.RevokedOn == null))
                ;
        }
    }
}
