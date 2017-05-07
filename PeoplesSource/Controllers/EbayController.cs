using System;
using System.IO;
using System.Net;
using System.Web.Mvc;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using PeoplesSource.Attribute;
using PeoplesSource.Common;
using PeoplesSource.Domain.Services;
using PeoplesSource.EWReturn;
using PeoplesSource.Models;
using System.Configuration;
using PeoplesSource.Ebay.Models;
using System.Collections.Generic;
using PeoplesSource.Helpers;


namespace PeoplesSource.Controllers
{
    public class EbayController : Controller
    {
        private readonly ReturnManagementService _c = new ReturnManagementService();
        private readonly ISellerServices _SellerService;
        private readonly ITemplateService _templateService;
        public EbayController
        (
         ISellerServices SellerService,
            ITemplateService templateService
        )
        {
            _SellerService = SellerService.ThrowIfNull("SellerService");
            _templateService = templateService.ThrowIfNull("templateService");
        }
        public string ReturnId;

        #region Return Details
        [Session]
        public ActionResult GetReturnDetails(string retId, int selId)
        {
            PeoplesSource.Data.PeopleSourceEntities db = new Data.PeopleSourceEntities();
            decimal RID = Convert.ToDecimal(retId);
            PeoplesSource.Data.EbayMessage Ebay = db.EbayMessages.FirstOrDefault(m => m.MasterMessageid == RID && m.Type == false && m.Sellerid == selId);
            var sellerdetail = _SellerService.GetSellerDetail(selId);
            ReturnId = Ebay.EbayMessageid.ToString();
            _c.Token = sellerdetail.UserToken;
            _c.IpAddress = sellerdetail.ProxyIP;
            _c.Port = sellerdetail.ProxyPort;
            _c.UserName = sellerdetail.ProxyUserName;
            _c.Password = sellerdetail.ProxyPassword;
            _c.IsCredentialRequired = sellerdetail.IsCredentialsRequired;
            _c.IsProxyRequired = sellerdetail.IsProxyRequired;
            ReturnDetailModel returnDetailItem;
            try
            {
                _c.OperationName = "getReturnDetail";
                var returnDetailReq = GetReturnDetailRequest();
                var returnDetail = _c.getReturnDetail(returnDetailReq); //returns null if returnid is not found
                returnDetailItem = GetReturnDetailItemResponse(returnDetail);
            }
            catch (Exception ex)
            {
                returnDetailItem = null;
                var error = ex.Message;
            }
            Ebay.IsRead = true;
            db.SaveChanges();
            return View(returnDetailItem);
        }
        #endregion
       
       
        #region IssueRefund
        [Session]
        public ActionResult IssueRefund(string retId, int selId)
        {
            EbayIssueRefundModel refundItem = new EbayIssueRefundModel();
            refundItem.SellerId = selId;
            refundItem.ReturnId = retId;

            var sellerdetail = _SellerService.GetSellerDetail(selId);
            ReturnId = retId;
            _c.Token = sellerdetail.UserToken;
            _c.IpAddress = sellerdetail.ProxyIP;
            _c.Port = sellerdetail.ProxyPort;
            _c.UserName = sellerdetail.ProxyUserName;
            _c.Password = sellerdetail.ProxyPassword;
            _c.IsCredentialRequired = sellerdetail.IsCredentialsRequired;
            _c.IsProxyRequired = sellerdetail.IsProxyRequired;
            ReturnDetailModel returnDetailItem;
            try
            {
                _c.OperationName = "getReturnDetail";
                var returnDetailReq = GetReturnDetailRequest();
                var returnDetail = _c.getReturnDetail(returnDetailReq); //returns null if returnid is not found
                returnDetailItem = GetReturnDetailItemResponse(returnDetail);
            }
            catch (Exception ex)
            {
                returnDetailItem = null;
                var error = ex.Message;
            }
            refundItem.RefundDetail = new RefundDetail();
            refundItem.RefundDetail.TotalAmount = returnDetailItem.ActualRefundTotalAmount;

            refundItem.RefundDetail.ItemizedRefund = new List<PeoplesSource.Ebay.Models.ItemizedRefund>();
            if (returnDetailItem.ActualItemizedRefund.Count() > 0)
            {
                foreach (var item in returnDetailItem.ActualItemizedRefund)
                {
                    PeoplesSource.Ebay.Models.ItemizedRefund iR = new PeoplesSource.Ebay.Models.ItemizedRefund();
                    iR.Amount = item.Amount;
                    iR.RefundFeeType = ParseEnum.Parse<PeoplesSource.Ebay.Models.RefundFeeType>(item.FeeType);
                    refundItem.RefundDetail.ItemizedRefund.Add(iR);
                }
            }
            else if (returnDetailItem.EstimateItemizedRefund.Count() > 0)
            {
                foreach (var item in returnDetailItem.EstimateItemizedRefund)
                {
                    PeoplesSource.Ebay.Models.ItemizedRefund iR = new PeoplesSource.Ebay.Models.ItemizedRefund();
                    iR.Amount = item.Amount;
                    iR.RefundFeeType = ParseEnum.Parse<PeoplesSource.Ebay.Models.RefundFeeType>(item.FeeType);
                    refundItem.RefundDetail.ItemizedRefund.Add(iR);
                }
            }
            return View(refundItem);
        }

        [Session]
        [HttpPost]
        public ActionResult IssueRefund(EbayIssueRefundModel IssueRefund)
        {
            var sellerdetail = _SellerService.GetSellerDetail(IssueRefund.SellerId);
            ReturnId = IssueRefund.ReturnId;
            _c.Token = sellerdetail.UserToken;
            _c.IpAddress = sellerdetail.ProxyIP;
            _c.Port = sellerdetail.ProxyPort;
            _c.UserName = sellerdetail.ProxyUserName;
            _c.Password = sellerdetail.ProxyPassword;
            _c.IsCredentialRequired = sellerdetail.IsCredentialsRequired;
            _c.IsProxyRequired = sellerdetail.IsCredentialsRequired;
            RefundModel refundItem = new RefundModel();
            try
            {
                _c.OperationName = "issueRefund";

                var issueRefundrequest = new IssueRefundRequest();
                issueRefundrequest.ReturnId = new ReturnIdType { id = IssueRefund.ReturnId };
                issueRefundrequest.comments = IssueRefund.Comments;
                RefundDetailType rdt = new RefundDetailType();
                rdt.totalAmount = new Amount { Value = IssueRefund.RefundDetail.TotalAmount };
                List<ItemizedRefundDetailType> lstItemizedRefundDetailType = new List<ItemizedRefundDetailType>();
                foreach (var item in IssueRefund.RefundDetail.ItemizedRefund)
                {
                    ItemizedRefundDetailType obj = new ItemizedRefundDetailType();
                    obj.amount = new Amount { Value = item.Amount };
                    obj.refundFeeType = (PeoplesSource.EWReturn.RefundFeeType)item.RefundFeeType;
                    lstItemizedRefundDetailType.Add(obj);
                }
                rdt.itemizedRefund = lstItemizedRefundDetailType.ToArray();
                issueRefundrequest.refundDetail = rdt;

                var issueRefundResponse = _c.issueRefund(issueRefundrequest);
                refundItem = GetIssueRefundResponse(issueRefundResponse);

            }
            catch (Exception ex)
            {
                refundItem = null;
                var error = ex.Message;
            }
            return Json(new { data = refundItem, success = true }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Provide Seller Info
        [Session]
        public ActionResult ProvideSellerInfo(string retId, int selId)
        {
            var sellerdetail = _SellerService.GetSellerDetail(selId);
            ReturnId = retId;
            _c.Token = sellerdetail.UserToken;
            _c.IpAddress = sellerdetail.ProxyIP;
            _c.Port = sellerdetail.ProxyPort;
            _c.UserName = sellerdetail.ProxyUserName;
            _c.Password = sellerdetail.ProxyPassword;
            _c.IsCredentialRequired = sellerdetail.IsCredentialsRequired;
            _c.IsProxyRequired = sellerdetail.IsProxyRequired;
            SellerInfoModel sellerItem;
            try
            {
                _c.OperationName = "provideSellerInfo";
                var iprovideSellerInforequest = GetProvideSellerInfoRequest();
                var provideSellerInfoResponse = _c.provideSellerInfo(iprovideSellerInforequest);
                sellerItem = GetSellerInfoResponse(provideSellerInfoResponse);
            }
            catch (Exception ex)
            {
                sellerItem = null;
                var error = ex.Message;
            }
            return View(sellerItem);

        }

        [Session]
        [HttpPost]
        public ActionResult ProvideSellerInfo(EbayProvideSellerInfoModel EbayProvideSellerInfoModel)
        {
            string retId = "2";
            int selId = 1;
            var sellerdetail = _SellerService.GetSellerDetail(selId);
            ReturnId = retId;
            _c.Token = sellerdetail.UserToken;
            _c.IpAddress = sellerdetail.ProxyIP;
            _c.Port = sellerdetail.ProxyPort;
            _c.UserName = sellerdetail.ProxyUserName;
            _c.Password = sellerdetail.ProxyPassword;
            _c.IsCredentialRequired = sellerdetail.IsCredentialsRequired;
            _c.IsProxyRequired = sellerdetail.IsProxyRequired;
            SellerInfoModel sellerItem;
            try
            {
                _c.OperationName = "provideSellerInfo";
                var iprovideSellerInforequest = GetProvideSellerInfoRequest();
                var provideSellerInfoResponse = _c.provideSellerInfo(iprovideSellerInforequest);
                sellerItem = GetSellerInfoResponse(provideSellerInfoResponse);
            }
            catch (Exception ex)
            {
                sellerItem = null;
                var error = ex.Message;
            }
            return View(sellerItem);

        }
        #endregion

        [Session]
        public ActionResult ProvideTrackingInfo(string retId, int selId)
        {
            var sellerdetail = _SellerService.GetSellerDetail(selId);
            ReturnId = retId;
            _c.Token = sellerdetail.UserToken;
            _c.IpAddress = sellerdetail.ProxyIP;
            _c.Port = sellerdetail.ProxyPort;
            _c.UserName = sellerdetail.ProxyUserName;
            _c.Password = sellerdetail.ProxyPassword;
            _c.IsCredentialRequired = sellerdetail.IsCredentialsRequired;
            _c.IsProxyRequired = sellerdetail.IsProxyRequired;
            TrackingInfoModel trackingInfoItem;
            try
            {
                _c.OperationName = "provideTrackingInfo";
                var provideTrackingInforequest = GetProvideTrackingInfoRequest();
                var provideTrackingInfoResponse = _c.provideTrackingInfo(provideTrackingInforequest);
                trackingInfoItem = GetTrackingInfoResponse(provideTrackingInfoResponse);

            }
            catch (Exception ex)
            {
                trackingInfoItem = null;
                var error = ex.Message;
            }
            return View(trackingInfoItem);
        }

        [Session]
        public ActionResult SetItemAsReceived(string retId, int selId)
        {
            var sellerdetail = _SellerService.GetSellerDetail(selId);
            ReturnId = retId;
            _c.Token = sellerdetail.UserToken;
            _c.IpAddress = sellerdetail.ProxyIP;
            _c.Port = sellerdetail.ProxyPort;
            _c.UserName = sellerdetail.ProxyUserName;
            _c.Password = sellerdetail.ProxyPassword;
            _c.IsCredentialRequired = sellerdetail.IsCredentialsRequired;
            _c.IsProxyRequired = sellerdetail.IsProxyRequired;
            ItemReceivedModel receivedItem;
            try
            {
                _c.OperationName = "setItemAsReceived";
                var setItemAsReceivedrequest = GetSetItemAsReceivedRequest();
                var setItemAsReceivedResponse = _c.setItemAsReceived(setItemAsReceivedrequest);
                receivedItem = GetItemAsReceivedResponse(setItemAsReceivedResponse);
            }
            catch (Exception ex)
            {
                receivedItem = null;
                var error = ex.Message;
            }
            return View(receivedItem);
        }

        [Session]
        [HttpPost]
        public ActionResult Send(Mail model)
        {
            if (ModelState.IsValid)
            {
                var seller = _SellerService.GetSellerDetail(model.SellerId);
                var request = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["ebayApiUrl"]);

                if (seller.IsProxyRequired)
                {
                    WebProxy proxy = new WebProxy();

                    proxy.Address = new Uri("http://" + seller.ProxyIP + ":" + seller.ProxyPort + "");
                    proxy.BypassProxyOnLocal = false;
                    if (seller.IsCredentialsRequired)
                    {
                        proxy.Credentials = new NetworkCredential(seller.ProxyUserName, seller.ProxyPassword);
                    }
                    else
                    {
                        proxy.UseDefaultCredentials = true;
                    }
                    request.Proxy = proxy;
                }
                var doc = XDocument.Load(Server.MapPath("~/XmlTemplate/EbayMessage.xml"));
                var sw = new StringWriter();
                var xw = new XmlTextWriter(sw);
                doc.WriteTo(xw);
                string xmlString = sw.ToString();
                xmlString = xmlString.Replace("{{MailItemId}}", model.ItemId);
                xmlString = xmlString.Replace("{{MailSubject}}", model.MailSubject);
                xmlString = xmlString.Replace("{{MailBody}}", model.MailBody);
                xmlString = xmlString.Replace("{{MailRecipientID}}", model.RecepientId);
                xmlString = xmlString.Replace("{{UserToken}}", seller.UserToken);
                byte[] bytes = System.Text.Encoding.ASCII.GetBytes(xmlString);
                request.Headers.Add("X-EBAY-API-COMPATIBILITY-LEVEL", "717");
                request.Headers.Add("X-EBAY-API-DEV-NAME", seller.DevID);
                request.Headers.Add("X-EBAY-API-APP-NAME", seller.AppID);
                request.Headers.Add("X-EBAY-API-CERT-NAME", seller.CertID);
                request.Headers.Add("X-EBAY-API-SITEID", Convert.ToString(seller.SiteID));
                request.Headers.Add("X-EBAY-API-CALL-NAME", "AddMemberMessageAAQToPartner");
                request.ContentType = "text/xml";
                request.ContentLength = bytes.Length;
                request.Method = "POST";
                var requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                HttpWebResponse response;
                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream responseStream = response.GetResponseStream();
                    if (responseStream != null)
                    {
                        string responseStr = new StreamReader(responseStream).ReadToEnd();
                        var doc1 = new XmlDocument();
                        doc1.LoadXml(responseStr);
                        XmlNodeList nodeList = doc1.GetElementsByTagName("Ack");
                        string Ack = string.Empty;
                        foreach (XmlNode node in nodeList)
                        {
                            Ack = node.InnerText;
                        }
                        if (Ack == "Failure")
                        {
                            return Json(new { success = false, errors = "Message Sending Failed" }, JsonRequestBehavior.AllowGet);
                        }
                        return Json(new { success = true, Message = "Message Send Successfully" }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { success = false, errors = "Message Sending Failed" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                success = false,
                JsonRequestBehavior.AllowGet,
                Message = "ModelError",
                errors = string.Join("<br/>", ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray())
            });

        }

        [Session]
        public ActionResult GetMessageByTemplate(string tempId)
        {
            var template = _templateService.GetTemplate(Convert.ToInt32(tempId));
            return Json(template.TemplateContent, JsonRequestBehavior.AllowGet);
        }

        [Session]
        public ActionResult SendMessage(string itemId, string recId, int sellerId)
        {
            var model = new Mail();
            model.ItemId = itemId;
            model.RecepientId = recId;
            model.SellerId = sellerId;
            AddReference(model);
            return View(model);

        }

        private void AddReference(Mail model)
        {
            model.TemplateList = _templateService.GetTemplatesBySellerId(model.SellerId)
                .Select(x => new LookupItem
                {
                    Id = x.Id,
                    Description = x.TemplateName
                }).OrderBy(x => x.Description)
                .ToList();
        }

        public getUserReturnsRequest GetUserReturns()
        {
            var req = new getUserReturnsRequest(); // no input parameters will retrieve all returns in which the user is (or was) involved as a buyer or seller in the last 30 days
            //req.orderId = "1234"; //  orderId filter can be used to retrieve returns filed against an order (can be more than one for multiple line item orders).
            //req.itemFilter  // the itemFilter container can be used to retrieve returns filed against an item listing (can be more than one for multiple-quantity, fixed-price listings) or to retrieve a return filed against a specific order line item.
            //req.ReturnStatusFilter //ReturnStatusFilter container can be used to retrieve returns in specific state(s), such as ITEM_SHIPPED, CLOSED, MY_RESPONSE_DUE, and others.
            //req.creationDateRangeFilter // creationDateRangeFilter container is used to set a date range. All returns created within this date range are retrieved. The maximum date range period is 90 days.
            //req.otherUserFilter   // Other User Filter: the otherUserFilter container is used to retrieve returns for another eBay user acting in a buyer or seller role. If this filter type is used
            return req;
        }

        public getReturnDetailRequest GetReturnDetailRequest()
        {
            var req = new getReturnDetailRequest(); // one input parameter available ReturnId
            if (!string.IsNullOrEmpty(ReturnId)) { req.ReturnId = new ReturnIdType { id = ReturnId }; }
            // else { req.ReturnId = new ReturnIdType { id = "5010176355" }; }
            return req;
        }

        public GetActivityOptionsRequest GetActivityOptions()
        {
            var req = new GetActivityOptionsRequest(); //
            if (!string.IsNullOrEmpty(ReturnId)) { req.ReturnId = new ReturnIdType { id = ReturnId }; }
            //   else { req.ReturnId = new ReturnIdType { id = "5010176355" }; }
            return req;
        }

        public IssueRefundRequest GetIssueRefundRequest()
        {
            var req = new IssueRefundRequest(); //
            if (!string.IsNullOrEmpty(ReturnId)) { req.ReturnId = new ReturnIdType { id = ReturnId }; }
            //   else { req.ReturnId = new ReturnIdType { id = "5010176355" }; }
            //req.refundDetail //The refundDetail container is used to provide the refund type and amount.
            return req;
        }

        public ProvideSellerInfoRequest GetProvideSellerInfoRequest()
        {
            var req = new ProvideSellerInfoRequest(); //  the seller must provide the return ID for the return for which a Return Merchandise Authorization number or alternative return shipping address will be provided.
            if (!string.IsNullOrEmpty(ReturnId)) { req.ReturnId = new ReturnIdType { id = ReturnId }; }
            // else { req.ReturnId = new ReturnIdType { id = "5010176355" }; }
            //req.returnAddress
            return req;
        }

        public ProvideTrackingInfoRequest GetProvideTrackingInfoRequest()
        {
            var req = new ProvideTrackingInfoRequest(); // 
            if (!string.IsNullOrEmpty(ReturnId)) { req.ReturnId = new ReturnIdType { id = ReturnId }; }
            //  else { req.ReturnId = new ReturnIdType { id = "5010176355" }; }
            //req.carrierUsed
            //req.comments
            //req.trackingNumber
            return req;
        }

        public SetItemAsReceivedRequest GetSetItemAsReceivedRequest()
        {
            var req = new SetItemAsReceivedRequest(); // 
            if (!string.IsNullOrEmpty(ReturnId)) { req.ReturnId = new ReturnIdType { id = ReturnId }; }
            //  else { req.ReturnId = new ReturnIdType { id = "5010176355" }; }
            return req;
        }

        public ReturnDetailModel GetReturnDetailItemResponse(getReturnDetailResponse returnDetail)
        {
            var returnDetailItem = new ReturnDetailModel();
            if (returnDetail != null)
            {
                if (returnDetail.ReturnDetail != null)
                {
                    if (returnDetail.ReturnDetail.buyerReturnShipment != null)
                    {
                        returnDetailItem.BuyerReturnShipmentCarrierUsed =
                            returnDetail.ReturnDetail.buyerReturnShipment.carrierUsed;
                        returnDetailItem.BuyerReturnShipmentDeliveryDate =
                            returnDetail.ReturnDetail.buyerReturnShipment.deliveryDate;
                        returnDetailItem.BuyerReturnShipmentMaxEstDeliveryDate =
                            returnDetail.ReturnDetail.buyerReturnShipment.maxEstDeliveryDate;
                        returnDetailItem.BuyerReturnShipmentMinEstDeliveryDate =
                            returnDetail.ReturnDetail.buyerReturnShipment.minEstDeliveryDate;
                        returnDetailItem.BuyerReturnShipmentMerchandiseAuthorization =
                            returnDetail.ReturnDetail.buyerReturnShipment.returnMerchandiseAuthorization;
                        returnDetailItem.BuyerReturnShipmentStatus =
                            Convert.ToString(returnDetail.ReturnDetail.buyerReturnShipment.shipmentStatus);
                        if (returnDetail.ReturnDetail.buyerReturnShipment.shippingAddress != null)
                        {
                            returnDetailItem.BuyerReturnShipmentCity =
                                returnDetail.ReturnDetail.buyerReturnShipment.shippingAddress.city;
                            returnDetailItem.BuyerReturnShipmentCountry =
                                returnDetail.ReturnDetail.buyerReturnShipment.shippingAddress.country;
                            returnDetailItem.BuyerReturnShipmentCounty =
                                returnDetail.ReturnDetail.buyerReturnShipment.shippingAddress.county;
                            returnDetailItem.BuyerReturnShipmentName =
                                returnDetail.ReturnDetail.buyerReturnShipment.shippingAddress.name;
                            returnDetailItem.BuyerReturnShipmentPostalCode =
                                returnDetail.ReturnDetail.buyerReturnShipment.shippingAddress.postalCode;
                            returnDetailItem.BuyerReturnShipmentStatus =
                                returnDetail.ReturnDetail.buyerReturnShipment.shippingAddress.stateOrProvince;
                            returnDetailItem.BuyerReturnShipmentStreet1 =
                                returnDetail.ReturnDetail.buyerReturnShipment.shippingAddress.street1;
                            returnDetailItem.BuyerReturnShipmentStreet2 =
                                returnDetail.ReturnDetail.buyerReturnShipment.shippingAddress.street2;
                        }
                    }
                    if (returnDetail.ReturnDetail.caseId != null)
                    {
                        returnDetailItem.CaseId = returnDetail.ReturnDetail.caseId.id;
                        returnDetailItem.CaseType = Convert.ToString(returnDetail.ReturnDetail.caseId.type);
                    }
                    returnDetailItem.GlobalId = Convert.ToString(returnDetail.ReturnDetail.globalId);
                    if (returnDetail.ReturnDetail.refundInfo != null)
                    {
                        if (returnDetail.ReturnDetail.refundInfo.actualRefundDetail != null)
                        {
                            returnDetailItem.ActualRefundDate =
                                returnDetail.ReturnDetail.refundInfo.actualRefundDetail.refundDate;
                            returnDetailItem.ActualRefundStatus =
                                Convert.ToString(returnDetail.ReturnDetail.refundInfo.actualRefundDetail.refundStatus);
                            if (returnDetail.ReturnDetail.refundInfo.actualRefundDetail.actualRefund.itemizedRefund != null)
                            {
                                foreach (var refund in returnDetail.ReturnDetail.refundInfo.actualRefundDetail.actualRefund.itemizedRefund)
                                {
                                    var ir = new PeoplesSource.Models.ItemizedRefund
                                    {
                                        Amount = refund.amount.Value,
                                        CurrencyId = refund.amount.currencyId,
                                        FeeType = Convert.ToString(refund.refundFeeType)
                                    };
                                    returnDetailItem.ActualItemizedRefund.Add(ir);
                                }
                                returnDetailItem.ActualRefundTotalAmount =
                                    returnDetail.ReturnDetail.refundInfo.actualRefundDetail.actualRefund.itemizedRefund.Sum(x => x.amount.Value);
                            }
                        }
                        if (returnDetail.ReturnDetail.refundInfo.estimatedRefundDetail != null)
                        {
                            if (returnDetail.ReturnDetail.refundInfo.estimatedRefundDetail.estimatedRefund.itemizedRefund != null)
                            {
                                foreach (var refund in returnDetail.ReturnDetail.refundInfo.estimatedRefundDetail.estimatedRefund.itemizedRefund)
                                {
                                    var ir = new PeoplesSource.Models.ItemizedRefund
                                    {
                                        Amount = refund.amount.Value,
                                        CurrencyId = refund.amount.currencyId,
                                        FeeType = Convert.ToString(refund.refundFeeType)
                                    };
                                    returnDetailItem.EstimateItemizedRefund.Add(ir);
                                }
                                returnDetailItem.ActualRefundTotalAmount =
                                    returnDetail.ReturnDetail.refundInfo.estimatedRefundDetail.estimatedRefund.itemizedRefund
                                        .Sum(x => x.amount.Value);
                                if (returnDetail.ReturnDetail.refundInfo.estimatedRefundDetail != null && returnDetail.ReturnDetail.refundInfo.estimatedRefundDetail.itemizedOptionalRefund != null)
                                {
                                    foreach (var refund in returnDetail.ReturnDetail.refundInfo.estimatedRefundDetail.itemizedOptionalRefund)
                                    {
                                        var ir = new PeoplesSource.Models.ItemizedRefund
                                        {
                                            Amount = refund.amount.Value,
                                            CurrencyId = refund.amount.currencyId,
                                            FeeType = Convert.ToString(refund.refundFeeType)
                                        };
                                        returnDetailItem.EstimateItemizedOptionalRefund.Add(ir);
                                    }
                                }
                            }
                        }
                        returnDetailItem.RefundDueDate = returnDetail.ReturnDetail.refundInfo.refundDue;
                    }
                    if (returnDetail.ReturnDetail.returnHistory != null)
                    {
                        foreach (var history in returnDetail.ReturnDetail.returnHistory)
                        {
                            var h = new ReturnHistory { CreationDate = history.creationDate, Note = history.note };
                            if (history.activityDetail != null)
                            {
                                h.ActivityCode = history.activityDetail.code;
                                h.ActivityContent = history.activityDetail.content;
                                h.ActivityDescription = history.activityDetail.description;
                            }
                            if (history.author != null)
                            {
                                h.AuthorRole = Convert.ToString(history.author.role);
                                h.AuthorUserId = history.author.userId;
                            }
                            returnDetailItem.ReturnHistorys.Add(h);
                        }
                    }
                    if (returnDetail.ReturnDetail.returnPolicy != null)
                    {
                        returnDetailItem.ReturnPolicyOptedForMultipleReturnAddress =
                            returnDetail.ReturnDetail.returnPolicy.optedForMultipleReturnAddress;
                        returnDetailItem.ReturnPolicyoptedForRMA =
                            returnDetail.ReturnDetail.returnPolicy.optedForRMA;
                    }
                    if (returnDetail.ReturnDetail.shipmentInfo != null)
                    {
                        foreach (var shipping in returnDetail.ReturnDetail.shipmentInfo)
                        {
                            var sp = new ShippingInfo
                            {
                                CarrierUsed = shipping.carrierUsed,
                                DeliveryDate = shipping.deliveryDate,
                                MaxEstDeliveryDate = shipping.maxEstDeliveryDate,
                                MinEstDeliveryDate = shipping.minEstDeliveryDate,
                                ReturnMerchandiseAuthorization = shipping.returnMerchandiseAuthorization,
                                ShipmentStatus = Convert.ToString(shipping.shipmentStatus),
                                ShippingCost = (shipping.shippingCost != null) ? shipping.shippingCost.Value : 0,
                                CurrencyId = (shipping.shippingCost != null) ? shipping.shippingCost.currencyId : "",
                                TrackingNumber = shipping.trackingNumber
                            };
                            if (shipping.shippingAddress != null)
                            {
                                sp.City = shipping.shippingAddress.city;
                                sp.Country = shipping.shippingAddress.country;
                                sp.County = shipping.shippingAddress.county;
                                sp.Name = shipping.shippingAddress.name;
                                sp.PostalCode = shipping.shippingAddress.postalCode;
                                sp.StateOrProvince = shipping.shippingAddress.stateOrProvince;
                                sp.Street1 = shipping.shippingAddress.street1;
                                sp.Street2 = shipping.shippingAddress.street2;
                            }
                            returnDetailItem.ShippingInfo.Add(sp);
                        }
                    }
                }
                if (returnDetail.ReturnSummary != null)
                {
                    var ret = new ReturnItemModel();
                    ret.CreationDate = returnDetail.ReturnSummary.creationDate;
                    ret.LastModifiedDate = returnDetail.ReturnSummary.lastModifiedDate;
                    if (returnDetail.ReturnSummary.otherParty != null)
                    {
                        ret.OtherPartyRole = returnDetail.ReturnSummary.otherParty.role.ToString(); // BUYER, SELLER, EBAY, SYSTEM, OTHER
                        ret.OtherPartyUserId = returnDetail.ReturnSummary.otherParty.userId;
                    }
                    if (returnDetail.ReturnSummary.responseDue != null)
                    {
                        if (returnDetail.ReturnSummary.responseDue.party != null)
                        {
                            ret.ResponseDuePartyRole = returnDetail.ReturnSummary.responseDue.party.role.ToString();
                            ret.ResponseDuePartyUserId = returnDetail.ReturnSummary.responseDue.party.userId;
                        }
                        ret.ResponseDueRespondByDate = returnDetail.ReturnSummary.responseDue.respondByDate;
                    }
                    ret.ReturnId = returnDetail.ReturnSummary.ReturnId.id;
                    if (returnDetail.ReturnSummary.returnRequest != null)
                    {
                        ret.ReturnRequestComment = returnDetail.ReturnSummary.returnRequest.comments;
                        if (returnDetail.ReturnSummary.returnRequest.returnItem != null && returnDetail.ReturnSummary.returnRequest.returnItem.Any())
                        {
                            foreach (var retItem in returnDetail.ReturnSummary.returnRequest.returnItem)
                            {
                                var returnItemmodel = new ReturnRequestItem
                                {
                                    ReturnRequestItemId = retItem.itemId,
                                    ReturnRequestItemQuantity = retItem.returnQuantity,
                                    ReturnRequestTransactionId = retItem.transactionId
                                };
                                ret.ReturnRequestItems.Add(returnItemmodel);
                            }
                            ret.ReturnRequestReasonCode = returnDetail.ReturnSummary.returnRequest.returnReason.code;
                            ret.ReturnRequestReasonContent = returnDetail.ReturnSummary.returnRequest.returnReason.content;
                            ret.ReturnRequestReasonDescription = returnDetail.ReturnSummary.returnRequest.returnReason.description;
                        }
                    }
                    ret.ReturnType = returnDetail.ReturnSummary.ReturnType.ToString(); //MONEY_BACK, REPLACEMENT, UNKNOWN
                    ret.Status = returnDetail.ReturnSummary.status.ToString(); //CLOSED,WAITING_FOR_SELLER_INFO,READY_FOR_SHIPPING,ITEM_SHIPPED,ITEM_DELIVERED,ESCALATED,UNKNOWN
                    returnDetailItem.ReturnSummery = ret;
                }


                returnDetailItem.Ack = Convert.ToString(returnDetail.ack); //Failure,Success,Warning,PartialFailure
                returnDetailItem.TimeStamp = returnDetail.timestamp;
                returnDetailItem.Version = returnDetail.version;
                if (returnDetail.errorMessage != null)
                {
                    foreach (var item in returnDetail.errorMessage)
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
                        returnDetailItem.ErrorItems.Add(itemError);
                    }
                }
            }
            return returnDetailItem;
        }

        public ReturnModel GetReturnItemResponse(getUserReturnsResponse returns)
        {
            var returnItem = new ReturnModel();

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
                    if (item.returnRequest != null)
                    {
                        ret.ReturnRequestComment = item.returnRequest.comments;
                        if (item.returnRequest.returnItem != null && item.returnRequest.returnItem.Any())
                        {
                            foreach (var retItem in item.returnRequest.returnItem)
                            {
                                var returnItemmodel = new ReturnRequestItem
                                {
                                    ReturnRequestItemId = retItem.itemId,
                                    ReturnRequestItemQuantity = retItem.returnQuantity,
                                    ReturnRequestTransactionId = retItem.transactionId
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
                }
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
                returnItem.Ack = returns.ack.ToString(); //Failure,Success,Warning,PartialFailure
                returnItem.TimeStamp = returns.timestamp;
                returnItem.Version = returns.version;
            }
            return returnItem;
        }

        public ActivityOptionModel GetActivityOptionsResponse(GetActivityOptionsResponse activityOptionsResponse)
        {
            var activityOptionItem = new ActivityOptionModel();
            if (activityOptionsResponse != null)
            {
                foreach (var option in activityOptionsResponse.activityOptions)
                {
                    activityOptionItem.ActivityOptions.Add(Convert.ToString(option));
                }
                activityOptionItem.Ack = activityOptionsResponse.ack.ToString();
                activityOptionItem.TimeStamp = activityOptionsResponse.timestamp;
                activityOptionItem.Version = activityOptionsResponse.version;
                foreach (var item in activityOptionsResponse.errorMessage)
                {
                    var itemError = new ErrorModel();
                    itemError.ErrorCategory = Convert.ToString(item.category);
                    itemError.ErrorDomain = item.domain;
                    itemError.ErrorId = item.errorId;
                    itemError.ErrorExceptionId = item.exceptionId;
                    itemError.Message = item.message;
                    itemError.Severity = Convert.ToString(item.severity);
                    itemError.Subdomain = item.subdomain;
                    foreach (var parameter in item.parameter)
                    {
                        var param = new ErrorParameterModel
                        {
                            Value = parameter.Value,
                            Name = parameter.name
                        };
                        itemError.ErrorParameters.Add(param);
                    }
                    activityOptionItem.ErrorItems.Add(itemError);
                }
            }
            return activityOptionItem;
        }

        public RefundModel GetIssueRefundResponse(IssueRefundResponse issueRefundResponse)
        {
            var refundItem = new RefundModel();
            if (issueRefundResponse != null)
            {
                refundItem.Ack = issueRefundResponse.ack.ToString();
                refundItem.RefundStatus = issueRefundResponse.RefundStatus.ToString();
                refundItem.TimeStamp = issueRefundResponse.timestamp;
                refundItem.Version = issueRefundResponse.version;
                foreach (var item in issueRefundResponse.errorMessage)
                {
                    var itemError = new ErrorModel();
                    itemError.ErrorCategory = Convert.ToString(item.category);
                    itemError.ErrorDomain = item.domain;
                    itemError.ErrorId = item.errorId;
                    itemError.ErrorExceptionId = item.exceptionId;
                    itemError.Message = item.message;
                    itemError.Severity = Convert.ToString(item.severity);
                    itemError.Subdomain = item.subdomain;
                    foreach (var parameter in item.parameter)
                    {
                        var param = new ErrorParameterModel
                        {
                            Value = parameter.Value,
                            Name = parameter.name
                        };
                        itemError.ErrorParameters.Add(param);
                    }
                    refundItem.RefundErrorItems.Add(itemError);
                }

            }
            return refundItem;
        }

        public SellerInfoModel GetSellerInfoResponse(ProvideSellerInfoResponse provideSellerInfoResponse)
        {
            var sellerItem = new SellerInfoModel();
            if (provideSellerInfoResponse != null)
            {
                sellerItem.Ack = provideSellerInfoResponse.ack.ToString();
                sellerItem.TimeStamp = provideSellerInfoResponse.timestamp;
                sellerItem.Version = provideSellerInfoResponse.version;
                foreach (var item in provideSellerInfoResponse.errorMessage)
                {
                    var itemError = new ErrorModel();
                    itemError.ErrorCategory = Convert.ToString(item.category);
                    itemError.ErrorDomain = item.domain;
                    itemError.ErrorId = item.errorId;
                    itemError.ErrorExceptionId = item.exceptionId;
                    itemError.Message = item.message;
                    itemError.Severity = Convert.ToString(item.severity);
                    itemError.Subdomain = item.subdomain;
                    foreach (var parameter in item.parameter)
                    {
                        var param = new ErrorParameterModel
                        {
                            Value = parameter.Value,
                            Name = parameter.name
                        };
                        itemError.ErrorParameters.Add(param);
                    }
                    sellerItem.SellerErrorItems.Add(itemError);
                }
            }
            return sellerItem;
        }

        public TrackingInfoModel GetTrackingInfoResponse(ProvideTrackingInfoResponse provideTrackingInfoResponse)
        {
            var trackingInfoItem = new TrackingInfoModel();
            if (provideTrackingInfoResponse != null)
            {
                trackingInfoItem.DeliveryStatus = provideTrackingInfoResponse.deliveryStatus.ToString();
                trackingInfoItem.Ack = provideTrackingInfoResponse.ack.ToString();
                trackingInfoItem.TimeStamp = provideTrackingInfoResponse.timestamp;
                trackingInfoItem.Version = provideTrackingInfoResponse.version;
                foreach (var item in provideTrackingInfoResponse.errorMessage)
                {
                    var itemError = new ErrorModel();
                    itemError.ErrorCategory = Convert.ToString(item.category);
                    itemError.ErrorDomain = item.domain;
                    itemError.ErrorId = item.errorId;
                    itemError.ErrorExceptionId = item.exceptionId;
                    itemError.Message = item.message;
                    itemError.Severity = Convert.ToString(item.severity);
                    itemError.Subdomain = item.subdomain;
                    foreach (var parameter in item.parameter)
                    {
                        var param = new ErrorParameterModel
                        {
                            Value = parameter.Value,
                            Name = parameter.name
                        };
                        itemError.ErrorParameters.Add(param);
                    }
                    trackingInfoItem.TrackingErrorItems.Add(itemError);
                }
            }
            return trackingInfoItem;
        }

        public ItemReceivedModel GetItemAsReceivedResponse(SetItemAsReceivedResponse setItemAsReceivedResponse)
        {
            var receivedItem = new ItemReceivedModel();
            if (setItemAsReceivedResponse != null)
            {
                receivedItem.Ack = setItemAsReceivedResponse.ack.ToString();
                receivedItem.TimeStamp = setItemAsReceivedResponse.timestamp;
                receivedItem.Version = setItemAsReceivedResponse.version;
                foreach (var item in setItemAsReceivedResponse.errorMessage)
                {
                    var itemError = new ErrorModel();
                    itemError.ErrorCategory = Convert.ToString(item.category);
                    itemError.ErrorDomain = item.domain;
                    itemError.ErrorId = item.errorId;
                    itemError.ErrorExceptionId = item.exceptionId;
                    itemError.Message = item.message;
                    itemError.Severity = Convert.ToString(item.severity);
                    itemError.Subdomain = item.subdomain;
                    foreach (var parameter in item.parameter)
                    {
                        var param = new ErrorParameterModel
                        {
                            Value = parameter.Value,
                            Name = parameter.name
                        };
                        itemError.ErrorParameters.Add(param);
                    }
                    receivedItem.ErrorItems.Add(itemError);
                }
            }
            return receivedItem;
        }

    }
}


namespace PeoplesSource.EWReturn
{
    public partial class ReturnManagementService
    {
        public string Token { get; set; }

        public string IpAddress { get; set; }

        public string Port { get; set; }

        public bool IsCredentialRequired { get; set; }

        public bool IsProxyRequired { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string OperationName { get; set; }

        protected override WebRequest GetWebRequest(Uri uri)
        {
            var httpHeaders = new System.Collections.Specialized.NameValueCollection
            {
                {"X-EBAY-API-COMPATIBILITY-LEVEL", "717"},
                {"X-EBAY-SOA-OPERATION-NAME", OperationName},
                {"X-EBAY-API-SITEID", "0"},
                {"X-EBAY-SOA-SECURITY-TOKEN", Token}
            };
            var request = (HttpWebRequest)base.GetWebRequest(uri);
            if (IsProxyRequired)
            {

                WebProxy proxy = new WebProxy();
                proxy.Address = new Uri("http://" + IpAddress + ":" + Port + "");
                proxy.BypassProxyOnLocal = false;
                if (IsCredentialRequired)
                {
                    proxy.Credentials = new NetworkCredential(UserName, Password);
                }
                else
                {
                    proxy.UseDefaultCredentials = true;
                }
                request.Proxy = proxy;
            }
            request.Headers.Add(httpHeaders);
            return request;
        }
    }
}