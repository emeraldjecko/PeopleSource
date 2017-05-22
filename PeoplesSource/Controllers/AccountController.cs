using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PeoplesSource.Attribute;
using PeoplesSource.Common;
using PeoplesSource.Domain.Services;
using PeoplesSource.Extensions;
using PeoplesSource.Models;
using PeoplesSource.Helpers;
using System.Web.Configuration;
using System.Net.Mail;
using System.Net;
using Microsoft.SqlServer.Dts.Runtime;

namespace PeoplesSource.Controllers
{
    public class AccountController : Controller
    {

        #region Properties

        private readonly IMapper _mapper;
        private readonly IPersistence _persistence;
        private readonly IReferenceService _referenceService;

        #endregion

        #region Constructor

        public AccountController
            (
                IMapper mapper,
                IPersistence persistence,
                IReferenceService referenceService

            )
        {
            _mapper = mapper.ThrowIfNull("mapper");
            _persistence = persistence.ThrowIfNull("persistence");
            _referenceService = referenceService.ThrowIfNull("referenceService");

        }

        #endregion

        #region Login
       
        [Session]
        public ActionResult Login()
        {
            var notSupported = true;

            var browser = ControllerContext.HttpContext.Request.Browser;
            switch (browser.Browser)
            {
                case "IE":
                    if (browser.MajorVersion >= 9)
                        notSupported = false;
                    break;
                case "Opera":
                    if (browser.MajorVersion >= 10)
                        notSupported = false;
                    break;
                case "Firefox":
                    if (browser.MajorVersion >= 10)
                        notSupported = false;
                    break;
                case "Chrome":
                    if (browser.MajorVersion >= 16)
                        notSupported = false;
                    break;
                case "Safari":
                    if (browser.MajorVersion >= 5)
                        notSupported = false;
                    break;
            }

            ViewResult view = View();
            view.ViewData.Add("notSupported", notSupported);

            var login = new LoginDetail ();
            return View(login);
        }

        [HttpPost]
        [Session]
        public ActionResult Login(LoginDetail model, string returnurl)
        {
            //var country = _referenceService.GetCountry();
             if (ModelState.IsValid)
            {
                DateTime lastLoginDate = new DateTime();
                try
                {
                    MembershipUser userLast = Membership.GetUser(model.Username);
                    lastLoginDate = userLast.LastLoginDate;
                }
                catch { }

                if (MembershipManager.ValidateUser(model.Username, model.Password))
                {
                    MembershipUser user = Membership.GetUser(model.Username);
                    var userId = user.ProviderUserKey.ToString();
                    if (String.IsNullOrEmpty(model.Username))
                        throw new ArgumentException("Value cannot be null or empty.", "userName");

                    FormsAuthentication.SetAuthCookie(model.Username, model.RememberMe);
                    var userNameCookie = new HttpCookie(model.Username)
                    {
                        Expires = DateTime.Now.AddHours(FormsAuthentication.Timeout.TotalHours)
                    };
                    Response.Cookies.Add(userNameCookie);

                    //Check if user has logged in today

                    if (lastLoginDate.ToShortDateString() != DateTime.Now.ToShortDateString()) //first login today
                    {
                        using (var client = new WebClient())
                        {
                            client.DownloadFile(ConfigurationHelper.GetTeapplixProductURL(), ConfigurationHelper.GetTeapplixDownloadFolderPath() + "products.csv");
                            client.DownloadFile(ConfigurationHelper.GetTeapplixQuantityURL(), ConfigurationHelper.GetTeapplixDownloadFolderPath() + "quantity_report.csv");
                            client.DownloadFile(ConfigurationHelper.GetTeapplixOrderURL(), ConfigurationHelper.GetTeapplixDownloadFolderPath() + "order_report.csv");
                        }

                        //Execute Packages
                        string packageLocation = "";
                        Package ssisPackage;
                        Application app;
                        DTSExecResult result;

                        try
                        {
                            //Run ssis products
                            app = new Application();
                            packageLocation = ConfigurationHelper.GetSSISProductPath();
                            ssisPackage = app.LoadPackage(packageLocation, null);
                            result = ssisPackage.Execute();

                            //Run ssis quantity
                            app = new Application();
                            packageLocation = ConfigurationHelper.GetSSISQuantityPath();
                            ssisPackage = app.LoadPackage(packageLocation, null);
                            result = ssisPackage.Execute();


                            //Run ssis order
                            app = new Application();
                            packageLocation = ConfigurationHelper.GetSSISOrderPath();
                            ssisPackage = app.LoadPackage(packageLocation, null);
                            result = ssisPackage.Execute();

                            if (result == DTSExecResult.Failure)
                            {
                                string err = "";
                                foreach (Microsoft.SqlServer.Dts.Runtime.DtsError local_DtsError in ssisPackage.Errors)
                                {
                                    string error = local_DtsError.Description.ToString();
                                    err = err + error;
                                }

                                ViewBag.Message = err;
                            }
                            if (result == DTSExecResult.Success)
                            {
                                string message = "Package Executed Successfully....";
                                ViewBag.Message = message;
                            }
                            
                        }
                        catch(Exception ex)
                        {
                            ViewBag.Message = ex.Message;
                        }

                    }

                    if (!string.IsNullOrEmpty(returnurl))
                    {
                        return Redirect(returnurl);
                    }
                    if (Roles.IsUserInRole(model.Username, "Administrator"))
                    {
                        //    return RedirectToAction("Index", "Home");
                        return RedirectToAction("Index", "Seller");
                    }
                }
                else if (!string.IsNullOrEmpty(model.Username) && !string.IsNullOrEmpty(model.Password))
                {
                    ModelState.AddModelError("", @"Username or password is invalid");
                }
                
            }
            return View(model);
        }

        [Session]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return Redirect("~/Account/Login");
        }

        #endregion

        #region Forgot Password

        public ActionResult ForgotPassword()
        {
           // var model = new ForgotPassword { UserName = "Username" };
            return View();
        }

        //[HttpPost]
        //public ActionResult ForgotPassword(ForgotPassword model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var loginUser = Membership.GetUser(model.UserName);
        //        if (loginUser != null)
        //        {
        //            var email = loginUser.Email;
        //            MembershipUser mu = loginUser;
        //            var newPassword = mu.ResetPassword();
        //            try
        //            {
        //                //_clientService.SendEmailToUser(email, "Health Tek Password Recovery", string.Format("Your  password for Health Tek  is: {0}", newPassword));
        //                string UserEmail = WebConfigurationManager.AppSettings["ClientEmail"];
        //                string Password = WebConfigurationManager.AppSettings["Password"];
        //                MailMessage mail = new MailMessage();
        //                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                        
        //                mail.From = new MailAddress(UserEmail);
        //                mail.To.Add(email);
        //                mail.Subject = "SyncTu Provide the User Password";
        //                mail.Body = "Hello User with "+email+" , We Providing the Password o your Acctount which is '"+newPassword+"'";

        //                SmtpServer.Port = 587;
        //                SmtpServer.Credentials = new System.Net.NetworkCredential(UserEmail, Password);
        //                SmtpServer.EnableSsl = true;
        //                SmtpServer.Send(mail);
        //                ModelState.AddModelError("", @"Your password is sent to your registered email address");
        //                return View();
        //            }
        //            catch (Exception)
        //            {
        //                ModelState.AddModelError("", @"Email does not sending successfully");
        //                return View();
        //            }
        //        }

        //        ModelState.AddModelError("", @"Username is incorrect.");
        //        return View();
        //    }
        //    return View();
        //}

        [HttpPost]
        public ActionResult ForgotPassword(string EmailAddress)
        {
         
                 var loginUser = Membership.GetUser(EmailAddress);
                if (loginUser != null)
                {
                    var email = loginUser.Email;
                    MembershipUser mu = loginUser;
                    var newPassword = mu.GetPassword();
                    try
                    {
                        //_clientService.SendEmailToUser(email, "Health Tek Password Recovery", string.Format("Your  password for Health Tek  is: {0}", newPassword));
                        string UserEmail = WebConfigurationManager.AppSettings["ClientEmail"];
                        string Password = WebConfigurationManager.AppSettings["Password"];
                        MailMessage mail = new MailMessage();
                        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                        mail.From = new MailAddress(UserEmail);
                        mail.To.Add(email);
                        mail.Subject = "SyncTu Provide the User Password";
                        mail.Body = "Hello User with User Name " + email + " , Your Current Password is '" + newPassword + "'";

                        SmtpServer.Port = 587;
                        SmtpServer.Credentials = new System.Net.NetworkCredential(UserEmail, Password);
                        SmtpServer.EnableSsl = true;
                        SmtpServer.Send(mail);
                        return Json(new { success = true, Message = "Password Successfully Sent on Defined Email" }, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception)
                    {
                        return Json(new { success = false ,Message ="Password Not sent on Defined Email"}, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { success = false, Message = "User Name Is not Valid" }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Change Password

        public ActionResult ChangePassword()
        {
            return View();
        }

        [Session]
        public ActionResult ChangePasswordDone(ChangePassword model, string userName)
        {
            if (ModelState.IsValid)
            {
                using (var session = _persistence.OpenSession())
                {
                    var link = "";
                    using (var transaction = session.BeginTransaction())
                    {
                        return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return this.FailSaveResult();
        }

        #endregion


    }
}

