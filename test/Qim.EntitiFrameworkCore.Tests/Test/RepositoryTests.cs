using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Qim.Domain.Uow;
using Qim.EntitiFrameworkCore.Tests.Domain;
using Qim.EntitiFrameworkCore.Tests.Domain.Repositories;
using Qim.Timing;
using Xunit;

namespace Qim.EntitiFrameworkCore.Tests.Test
{
    public class RepositoryTests : TestBase
    {
        public RepositoryTests()
        {
            _postRepository = Resolver.GetService<IPostRepository>();
            _blogRepository = Resolver.GetService<IBlogRepository>();
            _uowManager = Resolver.GetService<IUnitOfWorkManager>();
            Session.TenantId = 1;
            Session.UserId = "admin";
        }

        private readonly IBlogRepository _blogRepository;
        private readonly IPostRepository _postRepository;
        private readonly IUnitOfWorkManager _uowManager;

        private void InsertData(Blog blog)
        {
            using (var uow = _uowManager.Begin())
            {
                _blogRepository.Insert(blog);
                uow.Commit();
            }
        }

        private async Task InsertDataAsync(Blog blog)
        {
            using (var uow = _uowManager.Begin())
            {
                await _blogRepository.InsertAsync(blog);
                await uow.CommitAsync();
            }
        }

        private Blog GetBlog()
        {
            var blog = new Blog("blog01", "http://blog01.myblogs.com");
            blog.Posts.Add(new Post("title01", "empty"));
            return blog;
        }

        private void RemoveBlog(Blog blog)
        {
            UsingDbContext(context =>
            {
                //var posts = context.Posts.Where(a => a.BlogId == blog.PId);

                context.Posts.RemoveRange(blog.Posts);

                context.Blogs.Remove(blog);
            });
        }

        [Fact]
        public void Insert_ReleatedEntity_Test()
        {
            Blog blog1 = GetBlog();
            DateTime now = Clock.Now;
            Assert.True(blog1.IsTransient());

            InsertData(blog1);
            Assert.False(blog1.IsTransient());

            UsingDbContext(context =>
            {
                var blog = context.Blogs.Find(blog1.PId);

                Assert.NotNull(blog);
                Assert.Equal(1, blog.TenantId);
                Assert.Equal("admin", blog.CreateBy);
                Assert.True(blog.CreateOn >= now);

                var post = context.Posts.FirstOrDefault(a => a.BlogId == blog.PId);

                Assert.NotNull(post);
                Assert.True(post.PId != Guid.Empty);
                Assert.Equal("admin", post.CreateBy);
                Assert.True(post.CreateOn >= now);

                context.Posts.Remove(post);

                context.Blogs.Remove(blog);
                //test tenantId,
            });
        }

        [Fact]
        public void Select_WithTenantId_Test()
        {
            var blog1 = GetBlog();
            InsertData(blog1);

            using (var uow = _uowManager.Begin())
            {
                var blog = _blogRepository.FirstOrDefault(a => a.PId == blog1.PId);

                Assert.NotNull(blog);
                Assert.Equal(blog.Name, blog1.Name);
            }
            //更改session的TenantId
            Session.TenantId = 2;

            using (var uow = _uowManager.Begin())
            {
                var blog = _blogRepository.FirstOrDefault(a => a.PId == blog1.PId);
                Assert.Null(blog);

                using (_uowManager.Current.SetTenantId(1))
                {
                    blog = _blogRepository.FirstOrDefault(a => a.PId == blog1.PId);
                    Assert.NotNull(blog);
                }

                //测试是否释放
                blog = _blogRepository.FirstOrDefault(a => a.PId == blog1.PId);
                Assert.Null(blog);
            }

            RemoveBlog(blog1);
        }

        [Fact]
        public void Update_ReleatedEntity_NoTrack_Test()
        {
            var type = SequentialGuidGenerator.Instance.GuidType;
            var blog = GetBlog();
            var post = blog.Posts.First();
            Post newPost = null;
            var url = blog.Url;
            var now = Clock.Now;

            InsertData(blog);

            Assert.Null(blog.LastModifyBy);
            Assert.Null(blog.LastModifyOn);
            Assert.Null(post.LastModifyBy);
            Assert.Null(post.LastModifyOn);

            using (var uow = _uowManager.Begin())
            {
                //由于是新建的实体（不是从仓储查询的），修改不能被跟踪到，一定要在仓储update下才能被跟踪
                blog.Name = "newblog01";
                post.Title = "newtitle";

                //新加的实体可以被跟踪到
                newPost = new Post("title02", "empty");
                blog.Posts.Add(newPost);

                _blogRepository.Update(blog); //一定要这句才能跟踪到修改状态
                _postRepository.Update(post);
                uow.Commit();
            }

            UsingDbContext(context =>
            {
                var blog1 = context.Blogs.FirstOrDefault(a => a.PId == blog.PId);

                Assert.NotNull(blog1);
                Assert.Equal("newblog01", blog1.Name);
                Assert.Equal(blog1.Url, url);
                Assert.Equal("admin", blog1.LastModifyBy);
                Assert.True(blog1.LastModifyOn >= now);

                var posts = context.Posts.Where(a => a.BlogId == blog.PId).OrderBy(a=>a.PId).ToList();
                Assert.Equal(2, posts.Count);

                var post1 = posts.First(); //
                Assert.Equal("newtitle", post1.Title);
                Assert.Equal("admin", post1.LastModifyBy);
                Assert.True(post1.LastModifyOn >= now);

                var post2 = posts[1];
                Assert.Equal(post2.PId, newPost.PId);
                Assert.Equal("title02", post2.Title);
                Assert.Equal("admin", post2.CreateBy);
                Assert.True(post2.CreateOn >= now);
                Assert.Null(post2.LastModifyBy);
                Assert.Null(post2.LastModifyOn);

                context.Posts.RemoveRange(posts);
                context.Blogs.Remove(blog1);
            });


        }

        [Fact]
        public void Update_ReleatedEntity_Track_Test()
        {
            var blog = GetBlog();
            InsertData(blog);
            Post newPost = null;
            var url = blog.Url;
            var now = Clock.Now;

            using (var uow = _uowManager.Begin())
            {
                var blog1 = _blogRepository.GetTableIncluding(null, m => m.Posts).FirstOrDefault(a => a.PId == blog.PId);
                Assert.NotNull(blog1);
                Assert.True(blog1.Posts.Count > 0);

                var post1 = blog1.Posts.First();

                blog1.Name = "newblog01";
                post1.Title = "newtitle";

                newPost = new Post("title02", "empty");
                blog1.Posts.Add(newPost);

                uow.Commit();
            }

            UsingDbContext(context =>
            {
                var blog1 = context.Blogs.FirstOrDefault(a => a.PId == blog.PId);

                Assert.NotNull(blog1);
                Assert.Equal("newblog01", blog1.Name);
                Assert.Equal(blog1.Url, url);
                Assert.Equal("admin", blog1.LastModifyBy);
                Assert.True(blog1.LastModifyOn >= now);

                var posts = context.Posts.Where(a => a.BlogId == blog.PId).ToList();
                Assert.Equal(2, posts.Count);

                var post1 = posts.First(); //
                Assert.Equal("newtitle", post1.Title);
                Assert.Equal("admin", post1.LastModifyBy);
                Assert.True(post1.LastModifyOn >= now);

                var post2 = posts[1];
                Assert.Equal(post2.PId, newPost.PId);
                Assert.Equal("title02", post2.Title);
                Assert.Equal("admin", post2.CreateBy);
                Assert.True(post2.CreateOn >= now);
                Assert.Null(post2.LastModifyBy);
                Assert.Null(post2.LastModifyOn);

                context.Posts.RemoveRange(posts);
                context.Blogs.Remove(blog1);
            });



        }

        [Fact]
        public void Soft_Delete_Immediate_Test()
        {
            var blog = GetBlog();
            InsertData(blog);
            Assert.Null(blog.DeleteBy);
            Assert.Null(blog.DeleteOn);
            Assert.False(blog.IsDeleted);
            DateTime now = Clock.Now;

            using (var uow = _uowManager.Begin())
            {
                _blogRepository.Delete(blog);
                uow.Commit();
            }

            Assert.Null(_uowManager.Current);

            UsingDbContext(context =>
            {
                var blog1 = context.Blogs.FirstOrDefault(a => a.PId == blog.PId);
                Assert.NotNull(blog1);

                Assert.True(blog1.IsDeleted);
                Assert.Equal("admin", blog1.DeleteBy);
                Assert.True(blog1.DeleteOn >= now);

            });

            using (var uow = _uowManager.Begin())
            {
                var blog2 = _blogRepository.FirstOrDefault(a => a.PId == blog.PId);
                Assert.Null(blog2);
            }

            RemoveBlog(blog);
        }

        [Fact]
        public async Task Soft_Delete_ChangeFlag_Test()
        {
            var blog = GetBlog();
            await InsertDataAsync(blog); 
            Assert.Null(blog.DeleteBy);
            Assert.Null(blog.DeleteOn);
            Assert.False(blog.IsDeleted);
            DateTime now = Clock.Now;

            using (var uow = _uowManager.Begin())
            {
                //这种方式也可以
                //blog.IsDeleted = true;
                //_blogRepository.Update(blog);
                var blog1 = await _blogRepository.FirstOrDefaultAsync(a => a.PId == blog.PId);
                blog1.IsDeleted = true;
                await uow.CommitAsync();
            }

            await UsingDbContextAsync(async context =>
            {
                var blog1 = await context.Blogs.FirstOrDefaultAsync(a => a.PId == blog.PId);
                Assert.NotNull(blog1);

                Assert.True(blog1.IsDeleted);
                Assert.Equal("admin", blog1.DeleteBy);
                Assert.True(blog1.DeleteOn >= now);

            });

            using (var uow = _uowManager.Begin())
            {
                var blog2 = await _blogRepository.FirstOrDefaultAsync(a => a.PId == blog.PId);
                Assert.Null(blog2);
            }

            RemoveBlog(blog);
        }

        [Fact]
        public async Task Real_Delete_Test()
        {
            var blog = GetBlog();
            await InsertDataAsync(blog);
            Post post2;
            using (var uow = _uowManager.Begin())
            {
                var post1 = blog.Posts.First();
                await _postRepository.DeleteAsync(post1);
                post2 = new Post("title02", "empty");
                blog.Posts.Add(post2);

                await uow.CommitAsync();
            }

            await UsingDbContextAsync(async context =>
             {
                 var blog1 = await context.Blogs.Include(a => a.Posts).FirstOrDefaultAsync(a => a.PId == blog.PId);
                 Assert.NotNull(blog1);

                 Assert.True(blog1.Posts.Count == 1);

                 var post = blog1.Posts[0];

                 Assert.Equal(post.PId, post2.PId);

                 Assert.Equal(post.Title, post2.Title);

             });
        }

    }
}