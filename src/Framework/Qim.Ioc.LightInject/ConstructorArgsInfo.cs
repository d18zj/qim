using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Qim.Ioc.LightInject
{
    /// <summary>
    ///     参数信息
    /// </summary>
    internal class ConstructorArgsInfo
    {
        /// <summary>
        ///     参数类型
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        ///  参数值
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        ///     参数名称
        /// </summary>
        public string Name { get; set; }

        

         
    }
}