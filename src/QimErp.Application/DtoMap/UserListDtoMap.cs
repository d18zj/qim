using Qim.AutoMapper;
using QimErp.Domain;
using QimErp.Domain.Entity;
using QimErp.ServiceContracts.Dto;

namespace QimErp.Application.DtoMap
{
    public class UserListDtoMap : BaseMapProfile<User, UserListDto>
    {
    }
}