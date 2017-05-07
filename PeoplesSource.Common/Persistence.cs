using System;
using System.Collections.Generic;
using Castle.DynamicProxy;
using NHibernate;

namespace PeoplesSource.Common
{
    /// <summary>
    /// Persistence.
    /// </summary>
    public class Persistence : IPersistence
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly Stack<ISession> _sessionStack;
        private readonly Object _lock;
        private readonly ProxyGenerator _proxyGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="Persistence"/> class.
        /// </summary>
        /// <param name="sessionFactory">The session factory.</param>
        public Persistence(ISessionFactory sessionFactory, ProxyGenerator proxyGenerator)
        {
            _sessionFactory = sessionFactory.ThrowIfNull("sessionFactory");
            _sessionStack = new Stack<ISession>();
            _lock = new Object();
            _proxyGenerator = proxyGenerator.ThrowIfNull("proxyGenerator");
        }

        public ISession OpenSession()
        {
            return OpenSession(true);
        }

        /// <summary>
        /// Create a new session.
        /// </summary>
        /// <returns>Returns a session.</returns>
        public ISession OpenSession(bool autoFlushSession)
        {
            lock (_lock)
            {
                var session = _sessionFactory.OpenSession();
              //  session.EnableFilter("NonDeleted");
                if (!autoFlushSession)
                {
                    session.FlushMode = FlushMode.Never;
                }
                var proxy = _proxyGenerator.CreateInterfaceProxyWithTarget(
                    session,
                    new DisposeInterceptor(() => _sessionStack.Pop())
                );
                _sessionStack.Push(proxy);
                return proxy;
            }
        }

        public void FlushSession()
        {
            Session.Flush();
            Session.Close();
            //OpenSession();
        }

        /// <summary>
        /// Gets the current session.
        /// </summary>
        /// <value></value>
        public ISession Session
        {
            get
            {
                lock (_lock)
                {
                    if (_sessionStack.Count > 0)
                    {
                        return _sessionStack.Peek();
                    }

                    return null;
                }
            }
        }
    }
}
