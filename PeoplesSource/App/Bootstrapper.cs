using  PeoplesSource.Domain.Repositories;
using StructureMap;

namespace PeoplesSource.App
{
    public class Bootstrapper
    {
        public static void ConfigureIoC()
        {
// ReSharper disable CSharpWarnings::CS0618
            ObjectFactory.Initialize(x =>
//ReSharper restore CSharpWarnings::CS0618
                {
                    x.AddRegistry<WebRegistry>();
                    x.AddRegistry<RepositoryRegistry>();
                });
        }
    }
}