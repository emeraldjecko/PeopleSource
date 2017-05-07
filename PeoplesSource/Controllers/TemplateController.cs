using System.Web.Mvc;
using PeoplesSource.Attribute;

namespace PeoplesSource.Controllers
{
    public class TemplateController : Controller
    {
        // GET: Template
        public ActionResult Index()
        {
            return View();
        }

        [Session]
        public ActionResult List()
        {
            return View();
        }
    }
}