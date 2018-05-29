using System;
using Qim.Domain.Repositories;

namespace Qim.EntitiFrameworkCore.Tests.Domain.Repositories
{
    public interface IPostRepository : IRepository<Post, Guid>
    {

    }


    public class PostRepository : EfCoreRepository<Post, Guid>, IPostRepository
    {
        public PostRepository(IDbContextProvider dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}