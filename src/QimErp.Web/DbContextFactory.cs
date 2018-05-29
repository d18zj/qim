using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Qim.Configuration;
using Qim.EntitiFrameworkCore.Map;
using Qim.Ioc.Autofac;
using Qim.MultiTenancy;
using Qim.Reflection;

namespace QimErp.Web
{
    public class EntityMapManager
    {
        private static EntityMapManager _instance;
        private static readonly object _obj = new object();
        private readonly IList<IEntityMapConfig> _hostEntityMaps;
        private readonly IList<IEntityMapConfig> _tenantEntityMaps;

        private EntityMapManager(ITypeFinder typeFinder)
        {
            var typesToRegister = typeFinder.FindClassesOfType(typeof(IEntityMapConfig));
            _hostEntityMaps = new List<IEntityMapConfig>();
            _tenantEntityMaps = new List<IEntityMapConfig>();
            foreach (var type in typesToRegister)
            {
                var mapConfig = (IEntityMapConfig) Activator.CreateInstance(type);
                var attr =
                    mapConfig.EntityType.GetTypeInfo().GetCustomAttribute<MultiTenancySideAttribute>(true);
                if (attr != null && attr.Side == MultiTenancySides.Host)
                    _hostEntityMaps.Add(mapConfig);
                else
                    _tenantEntityMaps.Add(mapConfig);
            }
        }

        public static EntityMapManager GetInstance()
        {
            if (_instance == null)
            {
                var resolver = Bootstrapper.Start(app => app.UseAutofac()).Resolver;
                var typeFinder = resolver.GetService<ITypeFinder>();
                _instance = new EntityMapManager(typeFinder);
            }

            return _instance;
        }

        public IEnumerable<IEntityMapConfig> GetEntityMaps(MultiTenancySides? sides = null)
        {
            if (sides == null) return _hostEntityMaps.Concat(_tenantEntityMaps);

            return sides.Value == MultiTenancySides.Host ? _hostEntityMaps : _tenantEntityMaps;
        }
    }

    public class HostDbContext : DbContext
    {
        public HostDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityMap in EntityMapManager.GetInstance().GetEntityMaps(MultiTenancySides.Host))
                entityMap.Map(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
    }

    public class TenantDbContext : DbContext
    {
        public TenantDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityMap in EntityMapManager.GetInstance().GetEntityMaps(MultiTenancySides.Tenant))
                entityMap.Map(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
    }

    public class TenantDbContextFactory : IDesignTimeDbContextFactory<TenantDbContext>
    {
        private const string ConnectingString =
            "Data Source=.;Initial Catalog=QimErp;User ID=sa ;Password=txlzj.;Connect Timeout=30";


        public TenantDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<TenantDbContext>();
            builder.UseSqlServer(ConnectingString, b => b.MigrationsAssembly("QimErp.Web"));

            return new TenantDbContext(builder.Options);
        }
    }

    public class HostDbContextFactory : IDesignTimeDbContextFactory<HostDbContext>
    {
        private const string ConnectingString =
            "Data Source=.;Initial Catalog=QimErpHost;User ID=sa ;Password=txlzj.;Connect Timeout=30";


        public HostDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<HostDbContext>();
            builder.UseSqlServer(ConnectingString, b => b.MigrationsAssembly("QimErp.Web"));
            return new HostDbContext(builder.Options);
        }
    }
}