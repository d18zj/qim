using System.Collections.Generic;
using Xunit;

namespace Qim.Infrastructure.Tests.Runtime
{
    public class JsonConverterTests
    {
        [Fact]
        public void StringTypeConvert_Test()
        {
            var type = typeof(IEnumerable<>);
            Assert.NotNull(type);
        }
    }
}