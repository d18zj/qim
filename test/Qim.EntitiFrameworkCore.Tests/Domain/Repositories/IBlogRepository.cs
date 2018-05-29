using Qim.Domain.Repositories;

namespace Qim.EntitiFrameworkCore.Tests.Domain.Repositories
{
    public interface IBlogRepository : IRepository<Blog>
    {

    }


    public class BlogRepository : EfCoreRepository<Blog,string>, IBlogRepository
    {
        public BlogRepository(IDbContextProvider dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}