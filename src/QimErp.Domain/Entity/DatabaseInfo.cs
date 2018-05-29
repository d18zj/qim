using System.Collections.Generic;
using Qim.Domain.Entity;
using Qim.MultiTenancy;

namespace QimErp.Domain.Entity
{
    /// <summary>
    ///     数据库信息
    /// </summary>
    [MultiTenancySide(MultiTenancySides.Host)]
    public class DatabaseInfo : CreationAndModificationLogEntity<int>
    {
        /// <summary>
        ///     数据库编号
        /// </summary>
        public string DbNo { get; set; }

        /// <summary>
        ///     数据库名称
        /// </summary>
        public string DbName { get; set; }
        
        /// <summary>
        ///     数据库大小(Kb)
        /// </summary>
        public int DbSize { get; set; }

        /// <summary>
        ///     IP地址
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        ///     用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///     密码
        /// </summary>
        public string Password { get; set; }


        public string GetConnectionStrings()
        {
            return
                $"Data Source={IpAddress};Initial Catalog={DbName};User ID={UserName} ;Password={Password};Connect Timeout=30";
        }

        #region 导航属性

        private List<Tenant> _tenants;
        /// <summary>
        ///     租户集合
        /// </summary>
        public virtual List<Tenant> Tenants
        {
            get { return _tenants ?? (_tenants = new List<Tenant>()); }
            set { _tenants = value; }
        }

        #endregion
    }
}