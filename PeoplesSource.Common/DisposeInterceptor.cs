using Castle.DynamicProxy;
using System;
//using NHibernate.Proxy.DynamicProxy;

namespace PeoplesSource.Common
{
    internal class DisposeInterceptor : IInterceptor
    {
        private bool _disposed;

        private readonly Action _onDispose;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposeInterceptor"/> class.
        /// </summary>
        /// <param name="onDispose">The on dispose.</param>
        public DisposeInterceptor(Action onDispose)
        {
            _onDispose = onDispose.ThrowIfNull("onDispose");
        }

        public void Intercept(IInvocation invocation)
        {
            if (typeof(IDisposable) == invocation.Method.DeclaringType)
            {
                if ("Dispose" == invocation.Method.Name)
                {
                    if (!_disposed)
                    {
                        _disposed = true;
                        _onDispose();
                    }
                }
            }
            invocation.Proceed();
        }
    }
}
