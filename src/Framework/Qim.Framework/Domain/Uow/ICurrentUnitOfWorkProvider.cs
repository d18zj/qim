namespace Qim.Domain.Uow
{
    public interface ICurrentUnitOfWorkProvider
    {
        /// <summary>
        /// Gets/sets current <see cref="IUnitOfWork"/>.
        /// </summary>
        IUnitOfWork Current { get; set; }
    }
}