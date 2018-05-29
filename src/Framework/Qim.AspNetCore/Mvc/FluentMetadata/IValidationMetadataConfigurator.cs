using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace Qim.AspNetCore.Mvc.FluentMetadata
{
    public interface IValidationMetadataConfigurator
    {
        void Configure(ValidationMetadata metadata);
    }
}