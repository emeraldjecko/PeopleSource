using Castle.DynamicProxy;
using PeoplesSource.Attribute;
using PeoplesSource.Common;
using PeoplesSource.Domain;
using PeoplesSource.Helpers;
using NHibernate;
using NHibernate.Context;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using StructureMap.Web;

namespace PeoplesSource.App
{
    public class WebRegistry : Registry
    {
        public WebRegistry()
        {
            // Custom dependencies //

            For<ProxyGenerator>().Singleton().Use(new ProxyGenerator());
            For<ISessionFactory>().Singleton().Use(DomainRegistry.GetNHibernateConfig<WebSessionContext>().BuildSessionFactory());
            For<IPersistence>().HybridHttpOrThreadLocalScoped().Use<Persistence>();
            For(typeof(IRepository<>)).Use(typeof(Repository<>));
            ForConcreteType<SessionAttribute>().Configure.Setter<IPersistence>().IsTheDefault();
            //ForConcreteType<AccessHelper>().Configure.Setter<IRoleModulesService>().IsTheDefault().Setter<IReferenceService>().IsTheDefault();
            //ForConcreteType<AccessHelper>().Configure.Setter<IReferenceService>().IsTheDefault().Setter<IRoleModulesService>().IsTheDefault();
            //ForConcreteType<ClientIDFilter>().Configure.Setter<IClientUsersService>().IsTheDefault();
            //ForConcreteType<ValidExpressionAttribute>().Configure.Setter<ICalculator>().IsTheDefault();

            //ForConcreteType<Construction>().Configure.Setter<ICalculator>().IsTheDefault().Setter<ICalculatorContext>().IsTheDefault();
            //ForConcreteType<OperationalEnergySource>().Configure.Setter<ICalculator>().IsTheDefault().Setter<ICalculatorContext>().IsTheDefault();
            //ForConcreteType<Assembly>().Configure.Setter<ICalculator>().IsTheDefault().Setter<ICalculatorContext>().IsTheDefault();

            // Auto-configured dependencies //
            Scan(s =>
            {
                // Assemblies to be auto-configured //
                s.TheCallingAssembly();
                s.Assembly("PeoplesSource.Domain");
                s.Assembly("PeoplesSource.Common");

                // Conventions to be used to auto-configure dependencies //
                s.WithDefaultConventions();
                s.ConnectImplementationsToTypesClosing(typeof(IRepository<>));
                s.ConnectImplementationsToTypesClosing(typeof(IMapper<,>));
                s.ConnectImplementationsToTypesClosing(typeof(IDomainMapper<,>));
            });

            //For<ICalculationService>().HybridHttpOrThreadLocalScoped().Use<CalculationService>();
        }
    }
}