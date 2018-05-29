namespace Qim.Ioc
{
    /// <summary>
    ///     生命周期类型
    /// </summary>
    public enum LifetimeType
    {
        /// <summary>
        ///     瞬态
        /// </summary>
        Transient,

        /// <summary>
        ///     单一实例
        /// </summary>
        Singleton,

        /// <summary>
        ///     范围（每次请求）
        /// </summary>
        Scoped
    }
}