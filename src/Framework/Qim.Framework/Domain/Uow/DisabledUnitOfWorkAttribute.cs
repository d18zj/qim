using System;

namespace Qim.Domain.Uow
{
    [AttributeUsage(AttributeTargets.Method)]
    public class DisabledUnitOfWorkAttribute: Attribute
    {
        
    }
}