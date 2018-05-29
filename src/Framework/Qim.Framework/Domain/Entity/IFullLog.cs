namespace Qim.Domain.Entity
{
    public interface IFullLog : IDeletionLog,ICreationAndModificationLog
    {
        
    }


    public interface IFullLog<TUser> : IFullLog, ICreationAndModificationLog<TUser>, IDeletionLog<TUser>
        where TUser : BaseUser
    {
        
    }

}