using PeoplesSource.Attribute;
using PeoplesSource.Common;
using PeoplesSource.Domain;
using PeoplesSource.Domain.Services;
using PeoplesSource.Extensions;
using PeoplesSource.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PeoplesSource.Controllers
{
    public class AdminController : Controller
    {

        #region Properties

        private const int PageSize = 10;
        private readonly IPersistence _persistence;
        private readonly IMapper _mapper;
        private readonly IAdminService _adminService;
        private readonly IDomainMapper<AdminDetail, Admin> _AdminDetailDomainMapper;
        #endregion

        #region Constructor
        public AdminController
        (
         IPersistence persistence,
         IMapper mapper,
         IAdminService adminService,
         IDomainMapper<AdminDetail, Admin> AdminDetailDomainMapper
        )
        {
            _persistence = persistence.ThrowIfNull("persistence");
            _mapper = mapper.ThrowIfNull("mapper");
            _adminService = adminService.ThrowIfNull("AdminService");
            _AdminDetailDomainMapper = AdminDetailDomainMapper.ThrowIfNull("SellerDetailDomainMapper");
        }

        #endregion

        #region Index
        [Session]
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return Redirect("~/Account/Login?returnUrl=" + null);
            }
        }

        [Session]
        public JsonResult List()
        {
            var adminList = _adminService.GetAdmin();
            var adminmapper = _mapper.Get<Admin, AdminLine>();
            var model = new AdminList();
            model.AdminItems = adminList.Select(adminmapper.Map).ToList();
            return Json(model.AdminItems, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Add User
        [Session]
        public ActionResult Add()
        {
            if (Request.IsAuthenticated)
            {
                AdminDetail model = new AdminDetail();
                model.IsActive = true;
                return View(model);
            }
            else
            {
                return Redirect("~/Account/Login?returnUrl=" + null);
            }
        }

        [Session]
        [UserIdFilter]
        [HttpPost]
        public ActionResult Add(AdminDetail model, Guid? userId)
        {
            string Message = "";
            if (Request.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    using (var session = _persistence.OpenSession())
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            Guid uid = Guid.NewGuid();
                            MembershipCreateStatus status;
                            Membership.Provider.CreateUser(model.UserName, model.Password, model.Email, "Question", "Answer", true, uid, out status);

                            if (status == MembershipCreateStatus.Success)
                            {
                                Roles.AddUserToRole(model.UserName, RoleEnum.Administrator.ToString());

                                model.CreatedBy = userId;
                                model.CreatedDate = DateTime.Now;
                                model.UserId = uid;
                                var admin_Detail = _mapper.Map<AdminDetail, Admin>(model);
                                _adminService.SaveAdminDetail(admin_Detail);
                                transaction.Commit();
                                this.AddSuccessMessage("Admin Saved Successfully.");
                                string link = Url.Action("Index", "Admin");
                                return this.SuccessSaveResult(link);
                            }
                            else
                            {
                                switch (status)
                                {
                                    case MembershipCreateStatus.DuplicateUserName:
                                        Message = "The user name you provided already exists.";
                                        break;
                                    case MembershipCreateStatus.DuplicateEmail:
                                        Message = "The email address you provided already exists.";
                                        break;
                                    case MembershipCreateStatus.InvalidEmail:
                                        Message = "The email address you provided in invalid.";
                                        break;
                                    case MembershipCreateStatus.InvalidPassword:
                                        Message = "The password you provided must contains @ key word.";
                                        break;
                                    default:
                                        Message = "There is something wrong happened while creating user. Please try again later";
                                        break;
                                }


                            }
                        }
                    }
                }
            }
            else
            {
                return Redirect("~/Account/Login?returnUrl=" + null);
            }
            return this.FailSaveResult(Message);
        }
        #endregion

        #region Edit
        [Session]
        public ActionResult Edit(int id)
        {
            var admindetail = _adminService.GetAdminDetail(id);

            MembershipUser mu = Membership.GetUser(admindetail.UserId);


            var model = _mapper.Map<Admin, AdminDetail>(admindetail);
            model.Password = mu.GetPassword();
            return View(model);
        }

        [Session]
        [UserIdFilter]
        [HttpPost]
        public ActionResult Edit(AdminDetail model, Guid? userId)
        {
            string Message = "";
            if (ModelState.IsValid)
            {
                using (var session = _persistence.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        bool updatedata = true;
                        model.UpdatedBy = userId;
                        model.UpdatedDate = DateTime.Now;

                        MembershipUserCollection allUsers = Membership.GetAllUsers();
                        foreach (MembershipUser user in allUsers)
                        {
                            if (user.Email == model.Email && (Guid)user.ProviderUserKey != model.UserId)
                            {
                                Message = "Email address Already exists";
                                updatedata = false;
                            }
                        }

                        if (updatedata)
                        {
                            MembershipUser mu = Membership.GetUser(model.UserId);
                            string password = mu.GetPassword();

                            if (password != model.Password)
                            {
                                if (model.Password.Length >= 7 && model.Password.IndexOf('@') > 0)
                                {
                                    bool passchange = mu.ChangePassword(password, model.Password);
                                    if (!passchange)
                                    {
                                        Message = "Password did not updated";
                                        updatedata = false;
                                    }
                                    mu.Email = model.Email;
                                    Membership.UpdateUser(mu);

                                    var query = session.CreateSQLQuery("update [aspnet_Users] set [UserName]= '" + model.UserName + "', [LoweredUserName]= '" + model.UserName + "' where [UserId] = '" + model.UserId + "' ");
                                    query.ExecuteUpdate();
                                }
                                else
                                {
                                    Message = "The password you provided must contains @ key word & must be grater or equal to 7 charactor";
                                    updatedata = false;
                                }
                            }
                        }

                        if (updatedata)
                        {
                            var sellerdetail = _adminService.GetAdminDetail(model.AdminId);
                            _AdminDetailDomainMapper.MapToExisting(model, sellerdetail);
                            _adminService.SaveAdminDetail(sellerdetail);
                            transaction.Commit();

                            this.AddSuccessMessage("Admin Updated Successfully");
                            var link = Url.Action("Index", "Admin");
                            return this.SuccessSaveResult(link);
                        }
                    }
                }
            }
            return this.FailSaveResult(Message);
        }
        #endregion

        [Session]
        public ActionResult Delete(int id)
        {
            try
            {
                using (var session = _persistence.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        var admindetail = _adminService.GetAdminDetail(id);
                        MembershipUser mu = Membership.GetUser(admindetail.UserId);

                        _adminService.Delete(id);
                        transaction.Commit();

                        Membership.DeleteUser(mu.UserName);                        
                    }   
                    return this.SuccessSaveResult();
                }
            }
            catch (Exception ex)
            {

                return this.FailSaveResult(ex.Message);
            }

        }
    }
}