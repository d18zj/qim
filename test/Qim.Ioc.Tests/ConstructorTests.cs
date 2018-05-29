using Qim.Ioc.Tests.CaseServices;
using Xunit;

namespace Qim.Ioc.Tests
{
    public class ConstructorTests : TestBase
    {
        private readonly string bedroom = "bedroom";
        private readonly int bedRoomSize = 2;


        [Fact]
        public void Constructor_RegisterParams_Test()
        {
            Reset();
            //如果采用.WithAutoConcreteTypeResolution() 将导致注册的构造参数失败
            Registrar.Register<IRoom, Room>(new { name = bedroom, size = bedRoomSize });
            var room = Resolver.GetService<IRoom>();

            Assert.Equal(bedroom, room.Name);
            Assert.Equal(bedRoomSize, room.Size);
        }

        [Fact]
        public void Constructor_ResolveParams_Test()
        {
            Reset();
            Registrar.Register<IRoom, Room>();
            var room = Resolver.GetService<IRoom>(new { name = bedroom, size = bedRoomSize + 1 });

            Assert.Equal(bedroom, room.Name);
            Assert.Equal(bedRoomSize + 1, room.Size);
        }

        [Fact]
        public void Constructor_RegisterAndResolveParams_Test()
        {
            Reset();
            Registrar.Register<IRoom, Room>(new { name = bedroom, size = bedRoomSize });
            var room = Resolver.GetService<IRoom>(new { name = "room", size = bedRoomSize + 1 });

            Assert.Equal("room", room.Name);
            Assert.Equal(bedRoomSize + 1, room.Size);
        }


    }
}