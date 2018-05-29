using AutoMapper;
using Qim.Domain.Entity;
using Qim.Dto;

namespace Qim.AutoMapper
{
    public abstract class CreationAndModificationLogMapProfile<TEntity, TDto, TUser> :
            CreateLogMapProfile<TEntity, TDto, TUser>
        where TEntity : class, ICreationAndModificationLog<TUser>
        where TDto : class, ICreationAndModificationLogDto
        where TUser : BaseUser
    {
        protected override IMappingExpression<TEntity, TDto> MapEntityToDto()
        {
            return base.MapEntityToDto()
                .ForMember(dto => dto.LastModifyBy, opt => opt.MapFrom(e => e.LastModifierUser.UserName));
        }

        protected override IMappingExpression<TDto, TEntity> MapDtoToEntity()
        {
            return base.MapDtoToEntity().ForMember(e => e.LastModifyBy, opt => opt.Ignore());
        }
    }
}