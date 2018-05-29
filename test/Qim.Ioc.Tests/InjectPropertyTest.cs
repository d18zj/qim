using Qim.Ioc.Tests.CaseServices;
using Xunit;

namespace Qim.Ioc.Tests
{
    public class InjectPropertyTest : TestBase
    {

        public InjectPropertyTest()
        {
            Reset();
            Registrar.Register<IRoom, Room>(new { name = "bedroom", size = 2 });
            Registrar.Register<House>();
            Registrar.Register<HouseProxy>();

        }

        [Fact]
        public void Property_Inject_Test()
        {
            
            var house = Resolver.GetService<House>();


            Assert.NotNull(house.BedRoom);

            Assert.Equal("bedroom", house.BedRoom.Name);
            Assert.Equal(2, house.BedRoom.Size);

            Assert.Null(house.DrawingRoom);
        }

        [Fact]
        public void ConstructorInjecti_PropertyInject_Test()
        {
            var proxy = Resolver.GetService<HouseProxy>();
            var house = proxy.House;

            Assert.NotNull(house.BedRoom);

            Assert.Equal("bedroom", house.BedRoom.Name);
            Assert.Equal(2, house.BedRoom.Size);

            Assert.Null(house.DrawingRoom);

        }
    }
}