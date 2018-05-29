using System;
using Qim.Domain.Entity;

namespace Qim.EntitiFrameworkCore.Tests.Domain
{
    public class Post : CreationAndModificationLogEntity<Guid>
    {
        public string Title { get; set; }

        public string Body { get; set; }

        public string BlogId { get; set; }

        #region 导航属性

        public virtual Blog Blog { get; set; }

        #endregion

        protected Post()
        {

        }

        public Post(string title, string body)
        {
            Ensure.NotNullOrWhiteSpace(title, nameof(title));

            Title = title;
            Body = body ?? string.Empty;
        }
    }
}