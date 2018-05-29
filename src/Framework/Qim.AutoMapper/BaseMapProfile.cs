using AutoMapper;

namespace Qim.AutoMapper
{
    public abstract class BaseMapProfile<TEntity, TDto> : Profile where TEntity : class where TDto : class
    {
        protected BaseMapProfile()
        {
            MapEntityToDto();
            MapDtoToEntity();
        }

        protected virtual IMappingExpression<TEntity, TDto> MapEntityToDto()
        {
            return CreateMap<TEntity, TDto>();
        }

        protected virtual IMappingExpression<TDto, TEntity> MapDtoToEntity()
        {
            return CreateMap<TDto, TEntity>();
        }
    }
}