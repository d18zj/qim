namespace Qim.Domain.Entity
{
    public interface ICreationAndModificationLog : ICreationLog, IModificationLog
    {

    }

    public interface ICreationAndModificationLog<TUser> : ICreationAndModificationLog, ICreationLog<TUser>,
            IModificationLog<TUser>
        where TUser : BaseUser
    {
    }
}