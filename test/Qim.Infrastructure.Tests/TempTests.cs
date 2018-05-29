using System.Collections.Generic;
using Xunit;

namespace Qim.Infrastructure.Tests
{
    public class TempTests
    {

        [Fact]
        public void Attribute_Equal_Test()
        {
            var attribute1 = new FactAttribute() { Skip = "1" };
            var attribute2 = new FactAttribute() { Skip = "1" };
            var attribute3 = new FactAttribute();

            var list = new List<object>() { attribute1 };

            Assert.Contains(attribute2, list);
            Assert.DoesNotContain(attribute3, list);

            Assert.Equal(attribute1, attribute2);

            
            Assert.NotEqual(attribute3, attribute1);

        }
    }
}