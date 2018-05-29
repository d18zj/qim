using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace Qim.AspNetCore.Mvc.FluentMetadata
{
    public class FluentMetadataProvider : IDisplayMetadataProvider, IValidationMetadataProvider
    {
        private readonly IMetadataConfiguratorProvider _provider;

        public FluentMetadataProvider(IMetadataConfiguratorProvider provider)
        {
            _provider = provider;
        }

        public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
        {
            Ensure.NotNull(context, nameof(context));
            if (context.Key.MetadataKind == ModelMetadataKind.Property)
            {
                foreach (var configurator in _provider.GetMetadataConfigurators(context.Key))
                {
                    configurator.Configure(context.DisplayMetadata);
                }
            }


        }

        public void CreateValidationMetadata(ValidationMetadataProviderContext context)
        {
            Ensure.NotNull(context, nameof(context));
            if (context.Key.MetadataKind == ModelMetadataKind.Property)
            {
                foreach (var configurator in _provider.GetMetadataConfigurators(context.Key))
                {
                    configurator.Configure(context.ValidationMetadata);
                }
            }
        }
    }
}