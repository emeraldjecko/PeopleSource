using NHibernate;

namespace PeoplesSource.Common
{
    /// <summary>
    /// Persistence.
    /// </summary>
    public interface IPersistence
    {
        /// <summary>
        /// Create a new session.
        /// </summary>
        /// <returns>Returns a session.</returns>
        ISession OpenSession();

        ISession OpenSession(bool autoFlushSession);

        //void FlushSession();

        /// <summary>
        /// Gets the current session.
        /// </summary>
        ISession Session { get; }

    }
}
