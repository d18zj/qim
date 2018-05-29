using Qim.WorkContext;

namespace Qim.EntitiFrameworkCore.Tests
{
    public class TestSession : IQimSession
    {

        public string UserId { get; set; }
        public int? TenantId { get; set; }
        public string CultureName { get; set; }
    }
}