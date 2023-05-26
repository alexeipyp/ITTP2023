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
    public class ModelsAndEntitiesMapperProfile : Profile
    {
        public ModelsAndEntitiesMapperProfile()
        {
            CreateMap<CreateUserModel, DAL.Entities.User>()
                .ForMember(d => d.Guid, m => m.MapFrom(s => Guid.NewGuid()))
                .ForMember(d => d.PasswordHash, m => m.MapFrom(s => HashHelper.GetHash(s.Password)))
                .ForMember(d => d.Birthday, m => m.MapFrom(s => s.Birthday.HasValue ? s.Birthday!.Value.ToUniversalTime() : (DateTime?)null))
                .ForMember(d => d.CreatedOn, m => m.MapFrom(s => DateTime.UtcNow))
                .ForMember(d => d.ModifiedOn, m => m.Ignore())
                .ForMember(d => d.ModifiedBy, m => m.Ignore())
                .ForMember(d => d.RevokedOn, m => m.Ignore())
                .ForMember(d => d.RevokedBy, m => m.Ignore())
                ;

            CreateMap<UpdateUserModel, DAL.Entities.User>()
                .ForMember(d => d.Guid, m => m.MapFrom(s => s.UserToUpdateGuid))
                .ForMember(d => d.ModifiedOn, m => m.MapFrom(s => DateTime.UtcNow))
                .ForMember(d => d.Login, m => m.Ignore())
                .ForMember(d => d.PasswordHash, m => m.Ignore())
                .ForMember(d => d.Name, m => m.Ignore())
                .ForMember(d => d.Gender, m => m.Ignore())
                .ForMember(d => d.Birthday, m => m.Ignore())
                .ForMember(d => d.Admin, m => m.Ignore())
                .ForMember(d => d.CreatedOn, m => m.Ignore())
                .ForMember(d => d.CreatedBy, m => m.Ignore())
                .ForMember(d => d.RevokedOn, m => m.Ignore())
                .ForMember(d => d.RevokedBy, m => m.Ignore())
                .IncludeAllDerived()
                ;
            CreateMap<UpdateUserInfoModel, DAL.Entities.User>()
                .ForMember(d => d.Birthday, m => m.MapFrom(s => s.Birthday.HasValue ? s.Birthday!.Value.ToUniversalTime() : (DateTime?)null))
                .ForMember(d => d.Gender, m => m.MapFrom(s => s.Gender))
                .ForMember(d => d.Name, m => m.MapFrom(s => s.Name))
                ;
            CreateMap<UpdateUserLoginModel, DAL.Entities.User>()
                .ForMember(d => d.Login, m => m.MapFrom(s => s.Login))
                ;
            CreateMap<UpdateUserPasswordModel, DAL.Entities.User>()
                .ForMember(d => d.PasswordHash, m => m.MapFrom(s => HashHelper.GetHash(s.Password)))
                ;
            CreateMap<UpdateUserActiveStatusModel, DAL.Entities.User>()
                ;

            CreateMap<DeleteUserModel, DAL.Entities.User>()
                .ForMember(d => d.Guid, m => m.MapFrom(s => s.UserToDeleteGuid))
                .ForMember(d => d.RevokedOn, m => m.MapFrom(s => DateTime.UtcNow))
                .ForMember(d => d.Login, m => m.Ignore())
                .ForMember(d => d.PasswordHash, m => m.Ignore())
                .ForMember(d => d.Name, m => m.Ignore())
                .ForMember(d => d.Gender, m => m.Ignore())
                .ForMember(d => d.Birthday, m => m.Ignore())
                .ForMember(d => d.Admin, m => m.Ignore())
                .ForMember(d => d.CreatedOn, m => m.Ignore())
                .ForMember(d => d.CreatedBy, m => m.Ignore())
                .ForMember(d => d.ModifiedOn, m => m.Ignore())
                .ForMember(d => d.ModifiedBy, m => m.Ignore())
                ;

            CreateMap<DAL.Entities.User, UserDetailedViewModel>();
            CreateMap<DAL.Entities.User, UserFullViewModel>();
            CreateMap<DAL.Entities.User, UserViewModel>()
                .ForMember(d => d.IsActive, m => m.MapFrom(s => s.RevokedBy == null && s.RevokedOn == null))
                ;
        }
    }
}
