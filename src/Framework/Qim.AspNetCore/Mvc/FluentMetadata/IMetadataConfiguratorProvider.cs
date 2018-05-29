using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace Qim.AspNetCore.Mvc.FluentMetadata
{
    public interface IMetadataConfiguratorProvider
    {
        IEnumerable<IMetadataConfigurator> GetMetadataConfigurators(ModelMetadataIdentity identity);
    }
}