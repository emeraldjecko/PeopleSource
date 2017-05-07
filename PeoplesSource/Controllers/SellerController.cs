using PeoplesSource.Attribute;
using PeoplesSource.Common;
using PeoplesSource.Domain;
using PeoplesSource.Domain.Services;
using PeoplesSource.EWReturn;
using PeoplesSource.Extensions;
using PeoplesSource.Helpers;
using PeoplesSource.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml;


namespace PeoplesSource.Controllers
{
    public class SellerController : Controller
    {
        #region Properties

        private const int PageSize = 10;
        private readonly IPersistence _persistence;
        private readonly IMapper _mapper;
        private readonly IReferenceService _referenceService;
        private readonly ISellerServices _SellerService;
        private readonly ITemplateService _templateServie;
        private readonly IDomainMapper<SellerDetail, Seller> _SellerDetailDomainMapper;
        private readonly IDomainMapper<TemplateDetail, Template> _templateDomainMapper;

        #endregion

        #region Constructor
        public SellerController
        (
         IReferenceService referenceServices,
         IPersistence persistence,
         IMapper mapper,
         ISellerServices SellerService,
         IDomainMapper<SellerDetail, Seller> SellerDetailDomainMapper,
            IDomainMapper<TemplateDetail, Template> TemplateDomainMapper,
            ITemplateService TemplateService
        )
        {
            _persistence = persistence.ThrowIfNull("persistence");
            _mapper = mapper.ThrowIfNull("mapper");
            _referenceService = referenceServices.ThrowIfNull("referenceServices");
            _SellerService = SellerService.ThrowIfNull("SellerService");
            _SellerDetailDomainMapper = SellerDetailDomainMapper.ThrowIfNull("SellerDetailDomainMapper");
            _templateDomainMapper = TemplateDomainMapper.ThrowIfNull("TemplateDomainMapper");
            _templateServie = TemplateService.ThrowIfNull("TemplateService");

        }

        #endregion
        // GET: Seller
        [Session]
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {

                var sellerList = _SellerService.GetSeller();
                var sellermapper = _mapper.Get<Seller, SellerLine>();
                var model = new SellerList();
                model.SellerItems = sellerList.Select(sellermapper.Map).ToList();
                return View("Index", model);
            }
            else
            {
                return Redirect("~/Account/Login?returnUrl=" + null);
            }

        }

        [Session]
        public JsonResult List()
        {
            var sellerList = _SellerService.GetSeller();
            var sellermapper = _mapper.Get<Seller, SellerLine>();
            var model = new SellerList();
            model.SellerItems = sellerList.Select(sellermapper.Map).ToList();
            return Json(model.SellerItems, JsonRequestBehavior.AllowGet);


        }

        [Session]
        public ActionResult Add(string ReturnFlag = "")
        {
            if (Request.IsAuthenticated)
            {
                SellerDetail model = new SellerDetail();
                model.AppID = ConfigurationManager.AppSettings["AppID"];
                model.CertID = ConfigurationManager.AppSettings["CertID"];
                model.DevID = ConfigurationManager.AppSettings["DevID"];
                if (ReturnFlag == "1")
                {
                    HttpCookie responseCookie = CookieHelper.GetCookie("EbayResponse");
                    if (responseCookie != null)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(responseCookie["eBayAuthToken"])))
                        {
                            model.UserToken = Convert.ToString(responseCookie["eBayAuthToken"]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(responseCookie["username"])))
                        {
                            model.SellarName = Convert.ToString(responseCookie["username"]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(responseCookie["ErrorMessage"])) && string.IsNullOrEmpty(model.UserToken))
                        {
                            ViewBag.ErrorMessage = Convert.ToString(responseCookie["ErrorMessage"]);
                        }
                        CookieHelper.DeleteCookie("EbayResponse");
                    }
                }

                model.IsActive = true;
                AddReference(model);
                return View(model);
            }
            else
            {
                return Redirect("~/Account/Login?returnUrl=" + null);
            }
        }

        [Session]
        [HttpPost]
        [UserIdFilter]
        public JsonResult SaveSellerdetail(SellerDetail model, Guid? userId)
        {
            if (ModelState.IsValid)
            {
                using (var session = _persistence.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        model.CreatedBy = userId;
                        model.CreatedDate = DateTime.Now;     
                        var seller_Detail = _mapper.Map<SellerDetail, Seller>(model);
                        _SellerService.SaveSellerDetail(seller_Detail);
                        if (model.SellId != null)
                        {
                            var templateList = _templateServie.GetTemplatesBySellerId(model.SellId.Value);
                            foreach (var template in templateList)
                            {
                                var newTemplate = new TemplateDetail();
                                newTemplate.SellerId = seller_Detail.Sellerid;
                                newTemplate.TemplateName = template.TemplateName;
                                newTemplate.TemplateContent = template.TemplateContent;
                                newTemplate.CreatedDate = DateTime.Now;
                                var templates = _mapper.Map<TemplateDetail, Template>(newTemplate);
                                _templateServie.SaveTemplates(templates);
                            }
                        }
                        transaction.Commit();
                        this.AddSuccessMessage("Seller Saved Successfully.");
                        string link = Url.Action("Index", "Seller");
                        return this.SuccessSaveResult(link);
                    }
                }
            }
            return this.FailSaveResult();
        }

        [Session]
        public ActionResult Edit(int id)
        {
            var sellerdetail = _SellerService.GetSellerDetail(id);
            var model = _mapper.Map<Seller, SellerDetail>(sellerdetail);
            return View(model);
        }

        [Session]
        [UserIdFilter]
        public ActionResult Update(SellerDetail model, Guid? userId)
        {
            if (ModelState.IsValid)
            {
                using (var session = _persistence.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        model.UpdatedBy = userId;
                        model.UpdatedDate = DateTime.Now;      
                        var sellerdetail = _SellerService.GetSellerDetail(model.Sellerid);
                        sellerdetail.Email = model.Email;
                        _SellerDetailDomainMapper.MapToExisting(model, sellerdetail);
                        _SellerService.SaveSellerDetail(sellerdetail);
                        transaction.Commit();
                        this.AddSuccessMessage("Seller Updated Successfully");
                        var link = Url.Action("Index", "Seller");
                        return this.SuccessSaveResult(link);
                    }
                }
            }
            return this.FailSaveResult();
        }

        [Session]
        [UserIdFilter]
        public ActionResult Delete(int id)
        {
            try
            {
                using (var session = _persistence.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        var model = new SellerList();
                        //model.tdId = tdId;
                        var templateList = _templateServie.GetTemplatesBySellerId(id);
                        var tList = new List<TemplateLine>();
                        model.SellerId = id;
                        foreach (Template tag in templateList)
                        {
                            tList.Add(new TemplateLine
                            {
                                Id = tag.Id,
                                Seller = tag.seller.SellarName,
                                CreatedDate = tag.CreatedDate,
                                SellerId = tag.seller.Sellerid,
                                TemplateContent = tag.TemplateContent,
                                TemplateName = tag.TemplateName

                            });
                            _templateServie.Delete(tag.Id);
                        }
                        transaction.Commit();

                    }
                    using (var transaction1 = session.BeginTransaction())
                    {
                        _SellerService.Delete(id);
                        transaction1.Commit();
                    }
                    return this.SuccessSaveResult();

                }
            }
            catch (Exception ex)
            {

                return this.FailSaveResult(ex.Message);
            }

        }

        [Session]
        public ActionResult DeleteTemplate(int id)
        {
            try
            {
                using (var session = _persistence.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        _templateServie.Delete(id);
                        transaction.Commit();
                        return this.SuccessSaveResult();
                    }
                }
            }
            catch (Exception ex)
            {

                return this.FailSaveResult(ex.Message);
            }

        }

        [Session]
        public ActionResult Template(int sellerId)
        {
            var model = new TemplateDetail();
            model.SellerId = sellerId;
            return View();
        }

        [Session]
        [HttpPost]
        public ActionResult Template(TemplateDetail model)
        {
            if (ModelState.IsValid)
            {
                using (var session = _persistence.OpenSession())
                {
                    var template = _mapper.Map<TemplateDetail, Template>(model);
                    template.CreatedDate = DateTime.Now;
                    using (var transaction = session.BeginTransaction())
                    {
                        _templateServie.SaveTemplates(template);
                        transaction.Commit();
                    }
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new
            {
                success = false,
                JsonRequestBehavior.AllowGet,
                errors = string.Join("<br/>", ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray())
            });
        }

        [Session]
        public ActionResult EditTemplate(int id)
        {
            var tempdetail = _templateServie.GetTemplate(id);
            var model = _mapper.Map<Template, TemplateDetail>(tempdetail);
            return View(model);
        }

        [Session]
        [HttpPost]
        public ActionResult EditTemplate(TemplateDetail model)
        {
            if (ModelState.IsValid)
            {
                using (var session = _persistence.OpenSession())
                {
                    var templateDetail = _templateServie.GetTemplate(model.Id);
                    using (var transaction = session.BeginTransaction())
                    {
                        _templateDomainMapper.MapToExisting(model, templateDetail);
                        _templateServie.SaveTemplates(templateDetail);
                        transaction.Commit();
                    }
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new
            {
                success = false,
                JsonRequestBehavior.AllowGet,
                errors = string.Join("<br/>", ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray())
            });

        }

        [Session]
        public ActionResult GetTemplateList(int sellerId, string tdId)
        {
            var model = new SellerList();
            model.tdId = tdId;

            var templateList = _templateServie.GetTemplatesBySellerId(sellerId);
            var tList = new List<TemplateLine>();
            model.SellerId = sellerId;
            foreach (Template tag in templateList)
            {
                tList.Add(new TemplateLine
                {
                    Id = tag.Id,
                    Seller = tag.seller.SellarName,
                    CreatedDate = tag.CreatedDate,
                    SellerId = tag.seller.Sellerid,
                    TemplateContent = tag.TemplateContent,
                    TemplateName = tag.TemplateName
                });
            }
            model.templateList = tList;
            return PartialView("GetTemplateList", model);
            //return Json( model.templateList, JsonRequestBehavior.AllowGet);
        }

        private void AddReference(SellerDetail model)
        {
            model.sellerList = _SellerService.GetSeller()
                .Select(x => new LookupItem
                {
                    Id = x.Sellerid,
                    Description = x.SellarName
                }).OrderBy(x => x.Description)
                .ToList();
        }

        [Session]
        public ActionResult SellerReturn(int id)
        {
            var returnItems = new ReturnModel();
            ViewBag.SellID = id;
            var seller = _SellerService.GetSellerDetail(id);
            ViewBag.SellName = seller.SellarName;
            return View("SellerReturn", returnItems.ReturnItems);
        }

        [Session]
        public JsonResult SellerList(int sellerId)
        {
            var request = new getUserReturnsRequest();
            var returnItems = GetReturnItemResponse(request, sellerId);
            //var returnItems = new ReturnModel();
            //returnItems.ReturnItems.Add(new ReturnItemModel()
            //{
            //    ReturnId = "111",
            //    OtherPartyUserId = "111",
            //    ReturnType = "111",
            //    Status = "111",
            //    CreationDate = DateTime.Now,
            //    selId = 5,
            //    ItemId = "1324"
            //});
            return Json(returnItems.ReturnItems, JsonRequestBehavior.AllowGet);
        }

        [Session]
        public ReturnModel GetReturnItemResponse(getUserReturnsRequest request, int id)
        {
            var sellerdetail = _SellerService.GetSellerDetail(id);
            var c = new ReturnManagementService();
            c.IpAddress = sellerdetail.ProxyIP;
            c.Port = sellerdetail.ProxyPort;
            c.UserName = sellerdetail.ProxyUserName;
            c.Password = sellerdetail.ProxyPassword;
            c.IsCredentialRequired = sellerdetail.IsCredentialsRequired;
            c.IsProxyRequired = sellerdetail.IsProxyRequired;
            c.OperationName = "getUserReturns";
            //c.Token = "AgAAAA**AQAAAA**aAAAAA**aL52VQ**nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6AGkYChC5iHog+dj6x9nY+seQ**ZuICAA**AAMAAA**tDLOPafSIYVbRh13kVRnH8Xj5NVQ9zQa4Jlo4q5DtzuXAZot6ZU3mcwMGffAHl5Gei83rdFImVegPKTZzyrdG7cKgpkRBbZnwrnrcHyI7CEFqrLVPcqGx2LG2kbzEMQi4kNk78Y22vKNT1edvPFREcNYrG1nsybQ/UQbvGD7wbOa2y87Xy+2dCoDhH8ybsTZ5PdjYb6WOyTNIomwBYHql9+CiaRbSx3s8h9Uv4dh4I+A0O1xKt0upgutStb2xoeiDfcWB/olxuXGNKxeuJZ5ZiTmLoI6NVr4FN3W5ddOBhkJqmaGMzdPP7rWQF52okykVKbhsrsKIEPQWYI+JHpqh+6AyrbIrTaNyqyPWW2Lh7gZbOJ9mA/vOn3eJl/2o/wae8e19AxQ6u6OH3hZh0XCQfdApCCDnk0SshqFABsnLYX2Bs/9TQpNUZM71WjA5ExKd/RWrWLTDTqCsrcNKSnyqX6BYkJp15heWODVVyNXmKzDStwfxTY1y49euRfxIZTeP4ufO735+z2U6MV+g9rwDcHg+WtpOF1d2EIW2IehfcOwrZyH7fug29eUV3n1vCVQp4maqkCBUYz71XqPycfbBrnlqt7Xk1qwwsjMUMfBvBFVb3fbbG2CNTn6sznyhzRS7T5tjiaMuWIMjl/lyVoxdVWB6i/CBEgql8XtvxWsUxIW25QsWVhu03KYkSBUsG+n2bIPeGipad7NvWZjqOpnK78nEVKcFynHVTkWNspRHII9XtQ1qdpO7i5gm4imUxw5";
            c.Token = sellerdetail.UserToken;
            var returnItem = new ReturnModel();
            try
            {
                var returns = c.getUserReturns(request);


                if (returns != null)
                {
                    foreach (var item in returns.returns)
                    {
                        var ret = new ReturnItemModel();
                        ret.CreationDate = item.creationDate;
                        ret.LastModifiedDate = item.lastModifiedDate;
                        if (item.otherParty != null)
                        {
                            ret.OtherPartyRole = item.otherParty.role.ToString(); // BUYER, SELLER, EBAY, SYSTEM, OTHER
                            ret.OtherPartyUserId = item.otherParty.userId;
                        }
                        if (item.responseDue != null)
                        {
                            if (item.responseDue.party != null)
                            {
                                ret.ResponseDuePartyRole = item.responseDue.party.role.ToString();
                                ret.ResponseDuePartyUserId = item.responseDue.party.userId;
                            }
                            ret.ResponseDueRespondByDate = item.responseDue.respondByDate;
                        }
                        ret.ReturnId = item.ReturnId.id;
                        ret.selId = id;
                        if (item.returnRequest != null)
                        {
                            ret.ReturnRequestComment = item.returnRequest.comments;
                            if (item.returnRequest.returnItem != null && item.returnRequest.returnItem.Any())
                            {
                                ret.ItemId = item.returnRequest.returnItem[0].itemId;
                                foreach (var retItem in item.returnRequest.returnItem)
                                {
                                    var returnItemmodel = new ReturnRequestItem
                                    {
                                        ReturnRequestItemId = retItem.itemId,
                                        ReturnRequestItemQuantity = retItem.returnQuantity,
                                        ReturnRequestTransactionId = retItem.transactionId,

                                    };
                                    ret.ReturnRequestItems.Add(returnItemmodel);
                                }
                                ret.ReturnRequestReasonCode = item.returnRequest.returnReason.code;
                                ret.ReturnRequestReasonContent = item.returnRequest.returnReason.content;
                                ret.ReturnRequestReasonDescription = item.returnRequest.returnReason.description;
                            }
                        }
                        ret.ReturnType = item.ReturnType.ToString(); //MONEY_BACK, REPLACEMENT, UNKNOWN
                        ret.Status = item.status.ToString(); //CLOSED,WAITING_FOR_SELLER_INFO,READY_FOR_SHIPPING,ITEM_SHIPPED,ITEM_DELIVERED,ESCALATED,UNKNOWN
                        returnItem.ReturnItems.Add(ret);

                        // Insert into our message tables
                    }
                    if (returns.errorMessage != null)
                    {
                        foreach (var item in returns.errorMessage)
                        {
                            var itemError = new ErrorModel();
                            itemError.ErrorCategory = item.category.ToString(); //Application,System,Request
                            itemError.ErrorDomain = item.domain;
                            itemError.ErrorId = item.errorId;
                            itemError.ErrorExceptionId = item.exceptionId;
                            itemError.Message = item.message;
                            foreach (var parameter in item.parameter)
                            {
                                var param = new ErrorParameterModel
                                {
                                    Value = parameter.Value,
                                    Name = parameter.name
                                };
                                itemError.ErrorParameters.Add(param);
                            }
                            returnItem.ReturnErrorItems.Add(itemError);
                        }
                    }
                    returnItem.Ack = returns.ack.ToString(); //Failure,Success,Warning,PartialFailure
                    returnItem.TimeStamp = returns.timestamp;
                    returnItem.Version = returns.version;
                }
            }
            catch (Exception)
            {

            }
            return returnItem;
        }

        [Session]
        public ActionResult ValidateProxy(int id)
        {
            var seller = _SellerService.GetSellerDetail(id);
            //var isProxy = CanPing(seller.ProxyIP);

            var isProxy = SoketConnect(seller.ProxyIP, Convert.ToInt32(seller.ProxyPort));
            if (isProxy)
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        private static bool CanPing(string address)
        {
            Ping ping = new Ping();

            try
            {
                PingReply reply = ping.Send(address, 2000);
                if (reply == null) return false;

                return (reply.Status == IPStatus.Success);
            }
            catch (PingException e)
            {
                return false;
            }
        }

        public static bool SoketConnect(string host, int port)
        {
            var isSuccess = false;
            try
            {
                var connsock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                connsock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 200);
                //System.Threading.Thread.Sleep(500);
                var hip = IPAddress.Parse(host);
                var ipep = new IPEndPoint(hip, port);
                connsock.Connect(ipep);
                if (connsock.Connected)
                {
                    isSuccess = true;
                }
                connsock.Close();
            }
            catch (Exception)
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        [Session]
        public ActionResult Accept(string ebaytkn = "", string tknexp = "", string username = "")
        {
            string error = "";
            HttpCookie sessionCookie = CookieHelper.GetCookie("Ebay");
            string sessionID = sessionCookie.Values["SessionID"];
            CookieHelper.DeleteCookie("Ebay");

            string strReq = @"<?xml version=""1.0"" encoding=""utf-8""?>
                        <FetchTokenRequest xmlns=""urn:ebay:apis:eBLBaseComponents"">
                          <SessionID>" + sessionID + @"</SessionID>
                        </FetchTokenRequest>";


            XmlDocument xmlDoc = APICall.MakeAPIRequest(strReq, "FetchToken", "POST", error);

            if (error == "")
            {
                XmlNode root = xmlDoc["FetchTokenResponse"];
                Dictionary<string, string> dict = new Dictionary<string, string>();
                if (root["Errors"] != null)
                {
                    string errorCode = root["Errors"]["ErrorCode"].InnerText;
                    errorCode += " " + root["Errors"]["ShortMessage"].InnerText;
                    errorCode += " " + root["Errors"]["LongMessage"].InnerText;
                    dict.Add("ErrorMessage", errorCode);
                }
                else
                {
                    dict.Add("eBayAuthToken", root["eBayAuthToken"].InnerText);
                    dict.Add("username", username);
                    dict.Add("HardExpirationTime", root["HardExpirationTime"].InnerText);
                }

                CookieHelper.CreateCookie("EbayResponse", dict);
            }
            return Redirect("~/Seller/Add?ReturnFlag=1");
        }

        [Session]
        public ActionResult Reject()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("ErrorMessage", "Oops! User did'nt accept the app permission. Please try again.");
            CookieHelper.CreateCookie("EbayResponse", dict);
            return Redirect("~/Seller/Add?ReturnFlag=1");
        }

        [Session]
        public ActionResult GetUserToken()
        {
            string error = "";
            try
            {
                string signInURL = ConfigurationManager.AppSettings["SignInURL"];
                string runame = ConfigurationManager.AppSettings["RuName"];
                GetSessionID(runame, error);
                HttpCookie sessionCookie = CookieHelper.GetCookie("Ebay");
                if (sessionCookie != null)
                {
                    string sessionID = sessionCookie.Values["SessionID"];
                    if (sessionID != "")
                    {
                        return Redirect(signInURL + runame + "&SessID=" + Server.UrlEncode(sessionID));
                    }
                    else
                    {
                        return Redirect("~/Seller/Add");
                    }
                }
                else
                {
                    return Redirect("~/Seller/Add");
                }

            }
            catch (Exception ex)
            {
                error += ex.Message;
                return Redirect("~/Seller/Add");
            }

        }

        private void GetSessionID(string runame, string error)
        {
            try
            {
                XmlDocument xmlDoc;
                CookieHelper.DeleteCookie("Ebay");
                error = "";

                string strReq = @"<?xml version=""1.0"" encoding=""utf-8""?>
                        <GetSessionIDRequest xmlns=""urn:ebay:apis:eBLBaseComponents"">
                          <RuName>" + ConfigurationManager.AppSettings["RuName"] + @"</RuName>
                        </GetSessionIDRequest>";


                xmlDoc = new XmlDocument();
                xmlDoc = APICall.MakeAPIRequest(strReq, "GetSessionID", "POST", error);

                if (error == "")
                {
                    //get the root node, for ease of use
                    XmlNode root = xmlDoc["GetSessionIDResponse"];
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    if (root["Errors"] != null)
                    {
                        string errorCode = root["Errors"]["ErrorCode"].InnerText + " ";
                        errorCode += root["Errors"]["ShortMessage"].InnerText + " ";
                        errorCode += root["Errors"]["LongMessage"].InnerText;
                        dict.Add("ErrorMessage", errorCode);
                    }
                    else
                    {
                        dict.Add("SessionID", root["SessionID"].InnerText);
                    }
                    CookieHelper.CreateCookie("Ebay", dict);
                }
            }
            catch (Exception ex)
            {
                error += ex.Message + "\n" + ex.StackTrace;
            }
        }
    }
}