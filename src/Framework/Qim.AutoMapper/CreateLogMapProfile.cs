using AutoMapper;
using Qim.Domain.Entity;
using Qim.Dto;

namespace Qim.AutoMapper
{
    public abstract class CreateLogMapProfile<TEntity, TDto, TUser> : BaseMapProfile<TEntity, TDto>
        where TEntity : class, ICreationLog<TUser>
        where TDto : class, ICreationLogDto
        where TUser : BaseUser
    {
        protected override IMappingExpression<TEntity, TDto> MapEntityToDto()
        {
            return base.MapEntityToDto().ForMember(dto => dto.CreateBy, opt => opt.MapFrom(e => e.CreatorUser.UserName));
        }

        protected override IMappingExpression<TDto, TEntity> MapDtoToEntity()
        {
            return base.MapDtoToEntity()
                    .ForMember(e => e.CreateBy, opt => opt.Ignore())
                ;
        }
    }
}