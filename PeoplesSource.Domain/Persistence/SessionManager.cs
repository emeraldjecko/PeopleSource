using System;
using System.Web;
using NHibernate;
using NHibernate.Context;

namespace PeoplesSource.Domain
{
    /// <summary>
    /// Class SessionManager.
    /// </summary>
    public class SessionManager
    {
        /// <summary>
        /// Gets or sets the factory.
        /// </summary>
        /// <value>The factory.</value>
        private static ISessionFactory Factory { get; set; }


        /// <summary>
        /// Gets the factory.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>ISessionFactory.</returns>
        private static ISessionFactory GetFactory<T>() where T : ICurrentSessionContext
        {
            return DomainRegistry.GetNHibernateConfig<T>().BuildSessionFactory();
        }


        /// <summary>
        /// Gets the current session.
        /// </summary>
        /// <value>The current session.</value>
        public static ISession CurrentSession
        {
            get
            {
                if (Factory == null)
                    Factory = HttpContext.Current != null
                                    ? GetFactory<WebSessionContext>()
                                    : GetFactory<ThreadStaticSessionContext>();
                if (CurrentSessionContext.HasBind(Factory))
                    return Factory.GetCurrentSession();
                ISession session = Factory.OpenSession();
                //session.EnableFilter("NonDeleted");
                CurrentSessionContext.Bind(session);
                return session;
            }
        }

        /// <summary>
        /// Closes the session.
        /// </summary>
        public static void CloseSession()
        {
            if (Factory == null)
                return;
            if (CurrentSessionContext.HasBind(Factory))
            {
                ISession session = CurrentSessionContext.Unbind(Factory);
                session.Close();
            }
        }

        /// <summary>
        /// Commits the session.
        /// </summary>
        /// <param name="session">The session.</param>
        public static void CommitSession(ISession session)
        {
            try
            {
                session.Transaction.Commit();
            }
            catch (Exception)
            {
                session.Transaction.Rollback();
                throw;
            }
        }
    }
}



