using PeoplesSource.Common;
using NHibernate;
using StructureMap;

namespace PeoplesSource.Mappers
{
    public class BaseMapper
    {
        public ISession Session
        {
            get { return ObjectFactory.GetInstance<IPersistence>().Session; }
        }
    }
}