using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using LightInject;

namespace Qim.Ioc.LightInject
{
    internal static class ConstructorArgsHelper
    {
        private static readonly IDictionary<Tuple<Type, string>, ConstructorArgsInfo[]> _argsDictionary = new Dictionary<Tuple<Type, string>, ConstructorArgsInfo[]>();
        private static T[] GetConstructorArgsInfos<T>(object args, Func<PropertyInfo, T> func)
        {
            Ensure.NotNull(args, nameof(args));

            return args.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(func).ToArray();
        }

        public static object[] GetConstructorArgs(Type serviceType, string name, object constructorArgs)
        {

            if (constructorArgs != null)
            {
                return GetConstructorArgsInfos(constructorArgs, p => p.GetValue(constructorArgs)).ToArray();
            }

            var cacheKey = Tuple.Create(serviceType, name);
            ConstructorArgsInfo[] cacheArgs;
            return _argsDictionary.TryGetValue(cacheKey, out cacheArgs) ? cacheArgs.Select(a => a.Value).ToArray() : null;
        }

        public static Delegate GetFactoryDelegate(Type serviceType, Type implementationType, string name,
            object constructorArgs)
        {
            var argsInfoList = GetConstructorArgsInfos(constructorArgs, p => new ConstructorArgsInfo
            {
                Type = p.PropertyType,
                Name = p.Name,
                Value = p.GetValue(constructorArgs)

            });
            if (argsInfoList.Length > 15)
            {
                throw new ArgumentException($"构造函数参数最多15个，当前共有:{argsInfoList.Length}个。");
            }

            //缓存构造参数
            var cacheKey = Tuple.Create(serviceType, name);
            _argsDictionary[cacheKey] = argsInfoList;

            //查找符合参数的构造函数
            var construtor = implementationType.GetConstructor(argsInfoList.Select(a => a.Type).ToArray());
            if (construtor == null)
            {
                throw new ArgumentException($"未找到符合参数类型的构造函数！要构造的类:{implementationType.FullName}。");
            }

            /*创建对象工厂委托：Func<IServiceFactory, T1, T2,T3, TService>
             * T1,T2,T3为构造参数，个数不能超过16-1(IServiceFactory)=15个
             * TService为返回的对象类型:serviceType
             */
            var argsTypes = new List<Type> { typeof(IServiceFactory) };
            argsTypes.AddRange(argsInfoList.Select(a => a.Type));
            var delegateType = Expression.GetFuncType(argsTypes.Concat(new Type[] { serviceType }).ToArray());
            var construtorArgs = argsInfoList.Select(a => Expression.Parameter(a.Type)).ToList();

            var newExp = Expression.New(construtor, construtorArgs);
            construtorArgs.Insert(0, Expression.Parameter(typeof(IServiceFactory)));
            var exp = Expression.Lambda(delegateType, newExp, construtorArgs);
            return exp.Compile();
        }
    }
}