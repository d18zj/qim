using System.Reflection;
using DryIoc;
using Qim.Configuration;

namespace Qim.Ioc.DryIoc
{
    public static class ConfigurationExtensions
    {
        public static readonly PropertiesAndFieldsSelector InjectPropertiesSelector =
            PropertiesAndFields.All(withNonPublic: false, withPrimitive: false, withFields: false, withInfo: GetImportedProperties);

        private static PropertyOrFieldServiceInfo GetImportedProperties(MemberInfo m, Request req)
        {
            if (m.IsDefined(typeof(InjectAttribute), true))
            {
                return PropertyOrFieldServiceInfo.Of(m)
                    .WithDetails(ServiceDetails.Of(ifUnresolved: IfUnresolved.ReturnDefault), req);
            }
            return null;
        }

        public static IIocAppConfiguration UseDryIoc(this IAppConfiguration configuration)
        {
            Ensure.NotNull(configuration, nameof(configuration));
            var contaier = new Container(rules => rules
                //.WithAutoConcreteTypeResolution() 采用自动解决，会导入构造注入失败
                .With(InjectPropertiesSelector)
                .With(FactoryMethod.ConstructorWithResolvableArguments)
                .WithFactorySelector(Rules.SelectLastRegisteredFactory())
                .WithTrackingDisposableTransients()
                .WithImplicitRootOpenScope());

            var register = new IocRegistrar(contaier);
            register.RegisterInstance<IIocRegistrar>(register);
            register.Register<IIocResolver, IocResolver>();
            register.Register<IIocScopeResolverFactory, IocScopeResolverFactory>();

            return configuration.CreateIocAppConfiguration(register, contaier.Resolve<IIocResolver>());
        }
    }
}