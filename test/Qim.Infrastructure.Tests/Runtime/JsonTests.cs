using System;
using Qim.Runtime.Serialization;
using Xunit;

namespace Qim.Infrastructure.Tests.Runtime
{
    public class JsonTests
    {

        private readonly IObjectSerializer _serializer;
        public JsonTests()
        {
            _serializer = new JsonNetSerializer();
        }

        [Fact]
        public void Json_Strings_Test()
        {
            var room = new Room {Name = "bedroom", Size = 5};
            var json = _serializer.Serialize(room);
            Assert.True(json.Length > 0);

            var result = _serializer.Deserialize<Room>(json);

            Assert.Equal(room.Name, result.Name);
            Assert.Equal(room.Size, result.Size);
        }

        [Fact]
        public void Json_Binary_Test()
        {
            var room = new Room { Name = "bedroom", Size = 5 };
            var bson = _serializer.SerializeBinary(room);
            Assert.True(bson.Length > 0);

            var result = _serializer.Deserialize<Room>(bson);

            Assert.Equal(room.Name, result.Name);
            Assert.Equal(room.Size, result.Size);
        }

        [Fact]
        public void Null_Serialize_Test()
        {
            //var bson = 
            //Assert.Equal(bson.Length, 0);
            Assert.ThrowsAny<Exception>(() =>
            {
                _serializer.SerializeBinary<Room>(null);
            });

            var json = _serializer.Serialize<Room>(null);
            Assert.Equal("null", json);
        }

        [Fact]
        public void Null_Deserialize_Test()
        {
            byte[] arr=new byte[]{};
            var room = _serializer.Deserialize<Room>(arr);

            Assert.Null(room);

            Assert.ThrowsAny<Exception>(() =>
            {
                string a = null;
                _serializer.Deserialize<Room>(a);
            });

            string str = string.Empty;
            var room2 = _serializer.Deserialize<Room>(str);
            Assert.Null(room2);
        }

        private class Room
        {
            public string Name { get; set; }

            public int Size { get; set; }
        }
    }
}