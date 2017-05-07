using StructureMap.Configuration.DSL;

namespace PeoplesSource.Domain.Repositories
{
    public class RepositoryRegistry : Registry
    {
        public RepositoryRegistry()
        {
            For(typeof(IRepository<>)).Use(typeof(GenericRepository<>));
        }
    }
}
