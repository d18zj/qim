using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Qim.AspNetCore.Mvc.FluentMetadata;

namespace Qim.AspNetCore.Mvc
{
    public class MvcOptionsSetup : IConfigureOptions<MvcOptions>
    {
        private readonly IMetadataConfiguratorProvider _provider;

        public MvcOptionsSetup(IMetadataConfiguratorProvider provider)
        {
            _provider = provider;
        }

        public void Configure(MvcOptions options)
        {
            Ensure.NotNull(options, nameof(options));

            options.ModelMetadataDetailsProviders.Add(new FluentMetadataProvider(_provider));
        }
    }
}