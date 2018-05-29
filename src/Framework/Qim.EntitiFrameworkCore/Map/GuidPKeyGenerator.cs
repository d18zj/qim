using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Qim.EntitiFrameworkCore.Map
{
    public class GuidPKeyGenerator : ValueGenerator<Guid>
    {
        public override Guid Next(EntityEntry entry)
        {
            return SequentialGuidGenerator.Instance.NewGuid(SequentialGuidType.SequentialAtEnd);
        }

        public override bool GeneratesTemporaryValues { get; } = false;
    }
}