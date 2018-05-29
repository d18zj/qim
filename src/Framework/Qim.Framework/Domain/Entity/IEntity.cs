namespace Qim.Domain.Entity
{
    /// <summary>
    ///     泛型实体接口
    /// </summary>
    /// <typeparam name="TPkey"></typeparam>
    public interface IEntity<TPkey>
    {
        /// <summary>
        ///     PKey
        /// </summary>
        TPkey PId { get; set; }

        bool IsTransient();
    }

    /// <summary>
    ///     字符串主键实体
    /// </summary>
    public interface IEntity : IEntity<string>
    {
        
    }
}