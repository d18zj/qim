using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace Qim.AspNetCore.Mvc.FluentMetadata
{
    public interface IDisplayMetadataConfigurator
    {
        void Configure(DisplayMetadata metadata);
    }
}