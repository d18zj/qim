﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Qim.Reflection;

namespace Qim.AspNetCore.Mvc.FluentMetadata
{
    public class MetadataConfiguratorProvider : IMetadataConfiguratorProvider
    {
        private readonly object _obj = new object();
        private readonly ITypeFinder _typeFinder;
        private IDictionary<Type, List<IModelMetadataConfiguration>> _configurations;
        public MetadataConfiguratorProvider(ITypeFinder typeFinder)
        {
            _typeFinder = typeFinder;
        }


        protected IDictionary<Type, List<IModelMetadataConfiguration>> GetAllConfigurations()
        {
            if (_configurations == null)
            {
                lock (_obj)
                {
                    if (_configurations != null) return _configurations;

                    var types = _typeFinder.FindClassesOfType<IModelMetadataConfiguration>();
                    _configurations = new Dictionary<Type, List<IModelMetadataConfiguration>>();
                    foreach (var type in types)
                    {
                        var item = (IModelMetadataConfiguration)Activator.CreateInstance(type);
                        List<IModelMetadataConfiguration> list;
                        if (_configurations.TryGetValue(item.ModelType, out list))
                        {
                            list.Add(item);
                        }
                        else
                        {
                            _configurations.Add(item.ModelType, new List<IModelMetadataConfiguration> { item });
                        }

                    }
                }

            }
            return _configurations;
        }


        public IEnumerable<IMetadataConfigurator> GetMetadataConfigurators(ModelMetadataIdentity identity)
        {
            Ensure.NotNull(identity, nameof(identity));
            if (identity.MetadataKind == ModelMetadataKind.Type)
            {
                throw new InvalidOperationException("The identity's MetadataKind must by ModelMetadataKind.Type.");
            }
            var all = GetAllConfigurations();
            List<IModelMetadataConfiguration> list;
            if (!all.TryGetValue(identity.ContainerType, out list))
            {
                yield break;
            }
            foreach (var keyPaires in list.Select(a => a.MetadataConfigurators))
            {
                IMetadataConfigurator configurator;
                if (keyPaires.TryGetValue(identity, out configurator))
                {
                    yield return configurator;
                }
            }

        }
    }
}