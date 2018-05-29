namespace Qim.Domain.Entity
{
    public interface IAggregateRoot : IAggregateRoot<string>
    {

    }
    public interface IAggregateRoot<TPrimaryKey> : IEntity<TPrimaryKey>
    {

    }
}