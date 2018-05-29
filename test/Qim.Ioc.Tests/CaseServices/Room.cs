using System;

namespace Qim.Ioc.Tests.CaseServices
{
    public interface IRoom
    {
        string Name { get; }

        int Size { get; }
    }
    public class Room : IRoom
    {
        public Room(string name, int size)
        {
            Name = name;
            Size = size;
        }

        public string Name { get; }
        public int Size { get; }
    }

    public class RoomProxy : IRoom
    {
        private readonly IRoom _room;
        public RoomProxy(Room room)
        {
            _room = room;
        }

        public string Name => _room.Name;
        public int Size => _room.Size;
    }

   
}