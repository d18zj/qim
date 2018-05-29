namespace Qim.Domain.Entity
{
    public interface IMustHaveTenant
    {
        /// <summary>
        /// TenantId of this entity.
        /// </summary>
        int TenantId { get; set; }
    }
}