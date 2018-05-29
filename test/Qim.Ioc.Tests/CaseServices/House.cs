namespace Qim.Ioc.Tests.CaseServices
{
    public abstract class BaseHouse
    {
        [Inject]
        public IRoom BedRoom { get; set; }

    }

    public abstract class TempHouse : BaseHouse
    {
        
    }

    public class House : TempHouse
    {
        public IRoom DrawingRoom { get; set; }

        [Inject]
        public IRoom Kitchen { get; set; }
    }



    public class HouseProxy
    {
        public HouseProxy(House house)
        {
            House = house;
        }

        public House House { get; }
    }
}