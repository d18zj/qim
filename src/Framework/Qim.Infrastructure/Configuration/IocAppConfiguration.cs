using System;
using System.Collections.Generic;
using Qim.Ioc;

namespace Qim.Configuration
{
    internal class IocAppConfiguration : AppConfiguration, IIocAppConfiguration
    {
        public IocAppConfiguration(IDictionary<string, object> dicts, IIocRegistrar registrar, IIocResolver resolver)
        {
            ConfigDictionary = dicts;
            Registrar = registrar;
            Resolver = resolver;
        }

        public IIocRegistrar Registrar
        {
            get
            {
                return Get<IIocRegistrar>("Registrar", null);
            }
            set
            {
                Set("Registrar", value);
            }
        }

        public IIocResolver Resolver
        {
            get
            {
                return Get<IIocResolver>("Resolver", null);
            }
            set { Set("Resolver", value); }
        }

    }
}