using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using System.Linq;

namespace Qim.Ioc.Autofac
{
    internal static class Extensions
    {
        public static IEnumerable<TypedParameter> GeTypedParameters(object args)
        {
            if (args == null)
            {
                yield break;
            }
            var type = args.GetType();
            foreach (var propertyInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                yield return new TypedParameter(propertyInfo.PropertyType, propertyInfo.GetValue(args));
            }
        }
        public static IEnumerable<TMember> GetMembers<TMember>(this Type type,
           Func<TypeInfo, IEnumerable<TMember>> getMembers,
           bool includeBase = false)
        {
            var typeInfo = type.GetTypeInfo();
            var members = getMembers(typeInfo);
            if (!includeBase)
                return members;
            var baseType = typeInfo.BaseType;
            return baseType == null || baseType == typeof(object) ? members
                : members.Concat(baseType.GetMembers(getMembers, true));
        }

        //public static MethodInfo GetSetMethodOrNull(this PropertyInfo p, bool includeNonPublic = false)
        //{
        //    ////var methods =
        //    ////    p.DeclaringType.GetTypeInfo()
        //    ////        .DeclaredMethods.Where(m => (includeNonPublic || m.IsPublic) && m.Name == "set_" + p.Name)
        //    ////        .ToArray();
        //    ////return methods.Length == 1 ? methods[0] : null;
        //    //var method = p.SetMethod;
        //    //if (includeNonPublic)
        //    //{
        //    //    return method;
        //    //}
        //    //return method != null && method.IsPublic ? method : null;
        //}

        public static bool IsPrimitive(this Type type, bool orArrayOfPrimitives = false)
        {
            var typeInfo = type.GetTypeInfo();
            return typeInfo.IsPrimitive || typeInfo.IsEnum || type == typeof(string)
                || orArrayOfPrimitives && typeInfo.IsArray && typeInfo.GetElementType().IsPrimitive(true);
        }

        public static bool IsInjectable(this PropertyInfo property, bool withNonPublic = false, bool withPrimitive = false)
        {
            return property.CanWrite && property.GetIndexParameters().Length == 0 // first checks that property is assignable in general and not an indexer
                && (withNonPublic || property.SetMethod?.IsPublic == true)
                && (withPrimitive || !property.PropertyType.IsPrimitive(orArrayOfPrimitives: true));
        }

        public static IEnumerable<PropertyInfo> GetCanInjectPropertyInfos(this Type type)
        {
            return
                type.GetMembers(a => a.DeclaredProperties, true)
                    .Where(p => p.IsInjectable() && p.IsDefined(typeof(InjectAttribute), true));
        }

        public static IRegistrationBuilder<object, TActivatorData, TRegistrationStyle> InjectProperties
            <TActivatorData, TRegistrationStyle>(
                this IRegistrationBuilder<object, TActivatorData, TRegistrationStyle> registrationBuilder, Type implementationType)
        {
            if (implementationType.GetCanInjectPropertyInfos().Any())
            {
                registrationBuilder.OnActivated(e =>
                {
                    var properties = e.Instance.GetType().GetCanInjectPropertyInfos();
                    foreach (var property in properties)
                    {
                        var value = e.Context.ResolveOptional(property.PropertyType);
                        if (value != null)
                        {
                            property.SetValue(e.Instance, value);
                        }
                    }

                });
            }


            return registrationBuilder;
        }

        public static IRegistrationBuilder<object, TActivatorData, TRegistrationStyle> ConfigureLifecycle
            <TActivatorData, TRegistrationStyle>(
                this IRegistrationBuilder<object, TActivatorData, TRegistrationStyle> registrationBuilder,
                LifetimeType lifetimeType, bool classicWeb = false)
        {
            switch (lifetimeType)
            {
                case LifetimeType.Singleton:
                    registrationBuilder.SingleInstance();
                    break;
                case LifetimeType.Scoped:
                    if (classicWeb) //todo InstancePerRequest for asp.net ?
                        registrationBuilder.InstancePerRequest();
                    else
                        registrationBuilder.InstancePerLifetimeScope();
                    break;
                case LifetimeType.Transient:
                    registrationBuilder.InstancePerDependency();
                    break;
            }

            return registrationBuilder;
        }
    }
}