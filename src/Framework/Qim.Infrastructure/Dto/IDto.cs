namespace Qim.Dto
{
    public interface IDto<TPkey>
    {
        TPkey PId { get; set; }
    }

    public interface IDto : IDto<string>
    {

    }
}
