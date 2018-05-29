using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Qim.EntitiFrameworkCore.Map
{
    public class StringPKeyGenerator : ValueGenerator<string>
    {
        public override string Next(EntityEntry entry)
        {
            return SequentialGuidGenerator.Instance.NewGuid(SequentialGuidType.SequentialAsString).ToString("N");
        }

        public override bool GeneratesTemporaryValues { get; } = false;
    }
}