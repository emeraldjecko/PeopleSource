using System.Web.Mvc;
using PeoplesSource.Attribute;
using PeoplesSource.Extensions;

namespace PeoplesSource.Controllers
{
    /// <summary>
    /// Class MessageController.
    /// </summary>
    public class MessageController : Controller
    {
        /// <summary>
        /// Message this instance.
        /// </summary>
        /// <returns>JsonResult.</returns>
        [Session]
        [HttpGet]
        public JsonResult Messages()
        {
            return Json(this.GetMessages(), JsonRequestBehavior.AllowGet);
        }

    }
}
