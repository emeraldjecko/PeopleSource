using System.Web;
using PeoplesSource.Domain;

namespace PeoplesSource.App
{
    public class CurrentSessionWebModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.EndRequest += (sender, e) => SessionManager.CloseSession();
        }

        public void Dispose()
        {

        }
    }
}