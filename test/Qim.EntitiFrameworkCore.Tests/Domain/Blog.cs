using System.Collections.Generic;
using Qim.Domain.Entity;

namespace Qim.EntitiFrameworkCore.Tests.Domain
{
    public class Blog : FullLogEntity, IAggregateRoot, IMustHaveTenant
    {
        public string Name { get; set; }

        public string Url { get; protected set; }

        public int TenantId { get; set; }


        public void ChangeUrl(string url)
        {
            Ensure.NotNullOrWhiteSpace(url, nameof(url));

            var oldUrl = Url;
            Url = url;

            // DomainEvents.Add(new BlogUrlChangedEventData(this, oldUrl));
        }

        #region Ctor

        protected Blog()
        {
        }

        public Blog(string name, string url)
        {
            Ensure.NotNullOrWhiteSpace(name, nameof(name));
            Ensure.NotNullOrWhiteSpace(url, nameof(url));

            Name = name;
            Url = url;
        }

        #endregion

        #region 导航属性

        private List<Post> _posts;

        public virtual List<Post> Posts
        {
            get { return _posts ?? (_posts = new List<Post>()); }
            set { _posts = value; }
        }

        #endregion
    }
}