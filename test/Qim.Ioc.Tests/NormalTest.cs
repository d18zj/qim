using System;
using System.Linq;
using Qim.Ioc.Tests.CaseServices;
using Xunit;

namespace Qim.Ioc.Tests
{
    public class NormalTest : TestBase
    {
        [Fact]
        public void GetOptional_Service_Test()
        {
            Reset();
            var room = Resolver.GetOptionalService<IRoom>();
            Assert.Null(room);

            Assert.ThrowsAny<Exception>(() => { Resolver.GetService<IRoom>(); });
        }


        [Fact]
        public void Register_FactoryDelegate_Test()
        {
            Reset();
            Registrar.RegisterDelegate(typeof(Room), r => new Room("bedroom", 2));
            Registrar.RegisterDelegate(typeof(RoomProxy), r => new RoomProxy(r.GetService<Room>()));
            var room = Resolver.GetService<Room>();


            var proxy = Resolver.GetService<RoomProxy>();

            Assert.Equal(2, room.Size);
            Assert.Equal(room.Size, proxy.Size);
            Assert.Equal(room.Name, proxy.Name);
        }

        [Fact]
        public void Register_WithName_Test()
        {
            Reset();
            //Registrar.RegisterDelegate(typeof(Room), r => new Room("bedroom", 2));
            Registrar.Register<Room>(constructorArgsAsAnonymousType: new {name = "room", size = 5});
            Registrar.Register<IRoom, RoomProxy>("room1");
            Registrar.Register<IRoom, Room>("room2",
                constructorArgsAsAnonymousType: new {name = "bedroom", size = 2}); //


            var room1 = Resolver.GetService<IRoom>("room1");
            var room2 = Resolver.GetService<IRoom>("room2");

            Assert.Equal("room", room1.Name);

            Assert.Equal(5, room1.Size);
            Assert.Equal(2, room2.Size);
        }

        [Fact]
        public void Replace_Service_Test()
        {
            Reset();
            Registrar.Register<Room>(constructorArgsAsAnonymousType: new {name = "room", size = 5});
            Registrar.Register<IRoom, RoomProxy>();

            var room = Resolver.GetService<IRoom>();
            Assert.IsType<RoomProxy>(room);

            Registrar.Replace(typeof(IRoom), typeof(Room),
                constructorArgsAsAnonymousType: new {name = "bedroom", size = 2});

            var room2 = Resolver.GetService<IRoom>();
            Assert.IsType<Room>(room2);
        }

        [Fact]
        public void Resolve_After_Register_Test()
        {
            Reset();
            Registrar.Register<Room>(constructorArgsAsAnonymousType: new {name = "room", size = 5});
            var room = Resolver.GetService<Room>();
            Registrar.Register<IRoom, Room>(new {name = "bedroom", size = 2}); //
            var room2 = Resolver.GetService<IRoom>();

            Assert.Equal(5, room.Size);

            Assert.Equal(2, room2.Size);
        }

        [Fact]
        public void Resolver_ManyServices_Test()
        {
            Reset();
            Registrar.RegisterDelegate(typeof(Room), r => new Room("room", 5));
            Registrar.Register<IRoom, Room>(new {name = "bedroom", size = 2}); //
            Registrar.Register<IRoom, RoomProxy>("room2"); //

            var services = Resolver.GetAllServices<IRoom>().ToArray();

            Assert.Equal(2, services.Length);

            //数组顺序不确定
            if (services[0].Name == "room")
            {
                Assert.Equal(5, services[0].Size);
                Assert.Equal(2, services[1].Size);
            }
            else
            {
                Assert.Equal(2, services[0].Size);
                Assert.Equal(5, services[1].Size);
            }
        }

        [Fact]
        public void UnRegister_Resolve_Test()
        {
            Reset();
            Registrar.Register<Room>(constructorArgsAsAnonymousType: new {name = "room", size = 5});

            Assert.ThrowsAny<Exception>(() => { Resolver.GetService<RoomProxy>(); });
        }
    }
}