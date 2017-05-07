using System.Web.Mvc;

namespace PeoplesSource.Controllers
{
    public class BaseController : Controller
    {
        //
        // GET: /Base/

        protected override void ExecuteCore()
        {
            //// Modify current thread’s cultures
            //const string strCulture = "en-US";
            //Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(strCulture);
            //Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
           

            base.ExecuteCore();
        }


     
    }
}
