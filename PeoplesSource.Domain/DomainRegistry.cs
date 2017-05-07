using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;
using NHibernate.Context;
using StructureMap.Configuration.DSL;
using NHibernate.Caches.SysCache2;

namespace PeoplesSource.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public class DomainRegistry : Registry
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Configuration GetNHibernateConfig<T>() where T : ICurrentSessionContext
        {
            // Note: Make sure to not use FluentNHibernate 1.3.0.727
            // There is a problem with it. http://stackoverflow.com/a/9580634/56145

            var config = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008
                    .ConnectionString(c => c.FromConnectionStringWithKey("PeoplesSourceConnectionString")))
                .Mappings(m => m.HbmMappings.AddFromAssemblyOf<Country>())
                .Cache(x => x.UseSecondLevelCache())
                .CurrentSessionContext<T>()
                .BuildConfiguration();

            config.SessionFactory()
                .Caching.Through<SysCacheProvider>().WithDefaultExpiration(60);

            return config;

             //typeof(IdentityUser), 
             //   typeof(IdentityRole), 
             //   typeof(IdentityUserLogin), 
             //   typeof(IdentityUserClaim),


        }
    }
}
