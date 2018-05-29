using System;
using Microsoft.EntityFrameworkCore;

namespace Qim.EntitiFrameworkCore.Map
{
    /// <summary>
    ///     执行实体到数据库的Map
    /// </summary>
    public interface IEntityMapConfig
    {
        /// <summary>
        ///     返回实体类型
        /// </summary>
        Type EntityType { get; }

        /// <summary>
        ///     进行实体映射
        /// </summary>
        /// <param name="modelBuilder"></param>
        void Map(ModelBuilder modelBuilder);
    }
}