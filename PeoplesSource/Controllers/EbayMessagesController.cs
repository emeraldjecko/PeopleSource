using Newtonsoft.Json;
using PeoplesSource.Attribute;
using PeoplesSource.Common;
using PeoplesSource.Domain.Services;
using PeoplesSource.Ebay.Models;
using PeoplesSource.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using PeoplesSource.Data;
using System.Data.Entity.SqlServer;


namespace PeoplesSource.Controllers
{
    public class EbayMessagesController : Controller
    {
        private const int PageSize = 10;
        private readonly ISellerServices _SellerService;

        public EbayMessagesController(ISellerServices SellerService)
        {
            _SellerService = SellerService.ThrowIfNull("SellerService");

        }

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
        public ActionResult GetMessages(int sellerId)
        {
            string error = "";
            PeopleSourceEntities db = new PeopleSourceEntities();
            List<Message> Newview = new List<Message>();
            var MessageList = new List<EbayMessage>();
            MessageList = db.EbayMessages.OrderByDescending(m => m.MessageRecieveDate).OrderBy(m => m.IsRead ? 1 : 0).Where(m => m.Sellerid == sellerId && m.Type == true && m.DeleteDate == null).ToList();
            if (MessageList != null)
            {
                foreach (var MessageData in MessageList)
                {
                    Newview.Add(new Message
                    {
                        ExternalMessageID = MessageData.EbayMessageid.ToString(),
                        MessageID = MessageData.Messageid.ToString(),
                        Subject = MessageData.MessageSubject,
                        SendingUserID = MessageData.Sellerid.ToString(),
                        ReceiveDate = MessageData.MessageRecieveDate.Value != null ? MessageData.MessageRecieveDate.Value : DateTime.Now,
                        ExpirationDate = MessageData.MessageExpiryDate.Value != null ? MessageData.MessageExpiryDate.Value : DateTime.Now,
                        ItemID = MessageData.MessageItemid,
                        RecipientUserID = MessageData.Seller.SellarName,
                        Sender = MessageData.Sender,
                        MessageType = MessageData.MessageType,
                        Read = MessageData.IsRead,
                        TagDetail = (from Data in db.MessageTags
                                     where Data.DeleteDate == null && Data.MasterMessageid == MessageData.Messageid
                                     select new TagList
                                     {
                                         TagsID = Data.Tagid,
                                         TagName = Data.Tag.TagName
                                     }).ToList()
                    });
                }
            }
            return Json(Newview, JsonRequestBehavior.AllowGet);
        }

        [Session]
        public ActionResult GetMessagesById(int sellerId, string messageId)
        {
            PeopleSourceEntities db = new PeopleSourceEntities();
            string error = "";
            GetMyMessagesResponse view = null;
            var seller = db.Sellers.Find(sellerId);
                //_SellerService.GetSeller().Where(x => x.Sellerid == sellerId).FirstOrDefault();
            if (!string.IsNullOrEmpty(seller.UserToken))
            {
                string strReq = @"<?xml version=""1.0"" encoding=""utf-8""?>" +
                                "<GetMyMessagesRequest xmlns=\"urn:ebay:apis:eBLBaseComponents\">" +
                                  "<DetailLevel>ReturnMessages</DetailLevel>" +
                                  "<RequesterCredentials>" +
                                    "<eBayAuthToken>" + seller.UserToken + "</eBayAuthToken>" +
                                  "</RequesterCredentials>" +
                                  "<MessageIDs>" +
                                    "<MessageID>" + messageId + "</MessageID>" +//Can Send upto 10 message ids at a time 
                                  "</MessageIDs>" +
                                "</GetMyMessagesRequest> ";

                XmlDocument xmlDoc = APICall.MakeAPIRequest(strReq, "GetMyMessages", "POST", error, seller.IsProxyRequired,
                                    seller.ProxyIP, seller.ProxyPort, seller.ProxyUsername, seller.ProxyPassword);

                if (error == "")
                {
                    //get the root node, for ease of use
                    XmlNode root = xmlDoc["GetMyMessagesResponse"];
                    if (root != null)
                    {
                        if (root["Errors"] != null)
                        {
                            error += root["Errors"]["ErrorCode"].InnerText + " ";
                            error += root["Errors"]["ShortMessage"].InnerText + " ";
                            error += root["Errors"]["LongMessage"].InnerText;
                        }
                        else
                        {
                            if (root["Ack"].InnerText == "Success")
                            {
                                string jsonText = JsonConvert.SerializeXmlNode(root);
                                string xmls = xmlDoc.InnerXml.Replace(" xmlns=\"urn:ebay:apis:eBLBaseComponents\"", "");
                                using (TextReader sr = new StringReader(xmls))
                                {
                                    XmlSerializer serializer = new XmlSerializer(typeof(GetMyMessagesResponse));
                                    view = serializer.Deserialize(sr) as GetMyMessagesResponse;
                                }
                            }
                        }
                    }
                }
            }
            decimal MID = Convert.ToDecimal(messageId);
            EbayMessage Ebay = db.EbayMessages.FirstOrDefault(m => m.EbayMessageid == MID && m.Type == true && m.Sellerid == sellerId);
            Ebay.IsRead = true;
            db.SaveChanges();
            if (view == null)
                return View("GetMessagesById", view);
            return View("GetMessagesById", view.Messages);

            //if (view == null)
            //    return Json(view, JsonRequestBehavior.AllowGet);
            //return Json(view.Messages, JsonRequestBehavior.AllowGet);

        }

        [Session]
        public ActionResult GetEbayMessagesById(int sellerId, string messageId, string Disable)
        {
            PeopleSourceEntities db = new PeopleSourceEntities();
            string error = "";
            GetMyMessagesResponse view = null;
            var seller = db.Sellers.Find(sellerId);
            var NewView = new List<Message>();
            //_SellerService.GetSeller().Where(x => x.Sellerid == sellerId).FirstOrDefault();
            decimal MID = Convert.ToDecimal(messageId);
            List<EbayMessage> Ebay = db.EbayMessages.Where(m => m.MasterMessageid == MID && m.Type == true && m.Sellerid == sellerId).OrderBy(m=>m.MessageRecieveDate).ToList();
            foreach(var Datas in Ebay)
            {
                Datas.IsRead = true;
                        if (!string.IsNullOrEmpty(seller.UserToken))
                        {
                            string strReq = @"<?xml version=""1.0"" encoding=""utf-8""?>" +
                                            "<GetMyMessagesRequest xmlns=\"urn:ebay:apis:eBLBaseComponents\">" +
                                              "<DetailLevel>ReturnMessages</DetailLevel>" +
                                              "<RequesterCredentials>" +
                                                "<eBayAuthToken>" + seller.UserToken + "</eBayAuthToken>" +
                                              "</RequesterCredentials>" +
                                              "<MessageIDs>" +
                                                "<MessageID>" + Datas.EbayMessageid + "</MessageID>" +//Can Send upto 10 message ids at a time 
                                              "</MessageIDs>" +
                                            "</GetMyMessagesRequest> ";

                            XmlDocument xmlDoc = APICall.MakeAPIRequest(strReq, "GetMyMessages", "POST", error, seller.IsProxyRequired,
                                                seller.ProxyIP, seller.ProxyPort, seller.ProxyUsername, seller.ProxyPassword);

                            if (error == "")
                            {
                                //get the root node, for ease of use
                                XmlNode root = xmlDoc["GetMyMessagesResponse"];
                                if (root != null)
                                {
                                    if (root["Errors"] != null)
                                    {
                                        error += root["Errors"]["ErrorCode"].InnerText + " ";
                                        error += root["Errors"]["ShortMessage"].InnerText + " ";
                                        error += root["Errors"]["LongMessage"].InnerText;
                                    }
                                    else
                                    {
                                        if (root["Ack"].InnerText == "Success")
                                        {
                                            string jsonText = JsonConvert.SerializeXmlNode(root);
                                            string xmls = xmlDoc.InnerXml.Replace(" xmlns=\"urn:ebay:apis:eBLBaseComponents\"", "");
                                            using (TextReader sr = new StringReader(xmls))
                                            {
                                                XmlSerializer serializer = new XmlSerializer(typeof(GetMyMessagesResponse));
                                                view = serializer.Deserialize(sr) as GetMyMessagesResponse;
                                                NewView.AddRange(view.Messages);
                                            }
                                        }
                                    }
                                }
                            }
                        }
           }
            //decimal MID = Convert.ToDecimal(messageId);
            //EbayMessage Ebay = db.EbayMessages.FirstOrDefault(m => m.EbayMessageid == MID && m.Type == true && m.Sellerid == sellerId);
            //Ebay.IsRead = true;
            db.SaveChanges();
            if (NewView == null)
                return View("GetEbayMessagesById", view);
            return View("GetEbayMessagesById", NewView);
        }

        [Session]
        [HttpPost]
        public ActionResult SaveDetailTag(decimal MessageId, string TagList)
        {
            if (MessageId != 0)
            {
                PeopleSourceEntities db = new PeopleSourceEntities();
                var MasterMessages = db.MasterMessages.Find(MessageId);
                MasterMessages.DetailTags = TagList.Trim();
                db.SaveChanges();
                return Json(new { success = true, TagList = MasterMessages.DetailTags }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        [Session]
        [HttpGet]
        public ActionResult GetDetailTag(decimal MessageId)
        {
            if (MessageId != 0)
            {
                PeopleSourceEntities db = new PeopleSourceEntities();
                var MasterMessages = db.MasterMessages.Find(MessageId);
                var NextMsg = db.MasterMessages.Count(m=>m.DeleteDate == null && m.MasterMessageid > MessageId);
                var PreviousMsg = db.MasterMessages.Count(m => m.DeleteDate == null && m.MasterMessageid < MessageId);
                string Disable = "Nothing";
                if (NextMsg == 0)
                {
                    Disable = "Next";
                }
                else if (PreviousMsg == 0)
                {
                    Disable = "Prev";
                }
                return Json(new { success = true, TagList = MasterMessages.DetailTags, Disable = Disable }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        [Session]
        [HttpPost]
        public ActionResult DeleteEbayMessage(decimal MessageId)
        {
            if (MessageId != 0)
            {
                PeopleSourceEntities db = new PeopleSourceEntities();
                var MasterMessages = db.MasterMessages.Find(MessageId);
                List<MasterMessage> NextPagedDetail = db.MasterMessages.Where(m => m.DeleteDate == null && m.MasterMessageid > MessageId).ToList();              
                string Button = "Nothing";
                int ChkNext = NextPagedDetail.Count;
                var PageDetail = new MasterMessageCustome();
                if (ChkNext == 0)
                {
                    Button = "Next";
                    NextPagedDetail = db.MasterMessages.Where(m => m.DeleteDate == null && m.MasterMessageid < MessageId).OrderByDescending(m=>m.MasterMessageid).ToList();
                    int ChkPrev = NextPagedDetail.Count;
                    if (ChkPrev == 0)
                    {
                        Button = "Prev";
                    }
                    else
                    {
                        PageDetail = NextPagedDetail.Select(m => new MasterMessageCustome { Sellerid = m.Sellerid, MasterMessageid = m.MasterMessageid, Type = m.Type }).FirstOrDefault();
                    }
                  
                }
                else
                {
                    var chkprevious = db.MasterMessages.Count(m => m.DeleteDate == null && m.MasterMessageid < MessageId);
                    if (chkprevious == 0)
                    {
                        Button = "Prev";
                    }
                    PageDetail = NextPagedDetail.Select(m => new MasterMessageCustome { Sellerid = m.Sellerid, MasterMessageid = m.MasterMessageid, Type = m.Type }).FirstOrDefault();
                }
                MasterMessages.DeleteDate = DateTime.Now;
                db.SaveChanges();
                return Json(new { success = true, NextPagedDetail = PageDetail , Button = Button }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        [Session]
        [HttpGet]
        public ActionResult GetTagsDetails(decimal MsgID)
        {
            PeopleSourceEntities db = new PeopleSourceEntities();
          //  var TagList = db.Tags.Where(m => m.DeletedDate == null).Select(m => new { TagID = m.Tagid ,TagName = m.TagName}).ToList();
            var TagList = db.Tags.Where(m => m.DeletedDate == null).Select(m => new { TagID = m.Tagid, TagName = m.TagName, Existing = (db.MessageTags.Where(k => k.DeleteDate == null && k.MasterMessageid == MsgID && k.Tagid == m.Tagid).Count() != 0) ? "True" : "False" }).ToList();
            return Json(new { success = true, TagList = TagList}, JsonRequestBehavior.AllowGet);
        }

        [Session]
        [HttpPost]
        public ActionResult AssignSystemTags(decimal MsgID, string CheckedTagList)
        {
            if (MsgID != 0 && !string.IsNullOrEmpty(CheckedTagList))
            {
                PeopleSourceEntities db = new PeopleSourceEntities();
                string[] TagIds = CheckedTagList.Split(',');
                List<MessageTag> MessageTags;
                MessageTag AssignTag = new MessageTag();
                decimal MessageID = Convert.ToDecimal(MsgID);
                MessageTags = db.MessageTags.Where(m => m.MasterMessageid == MessageID).ToList();
                if (MessageTags.Count == 0)
                {

                    foreach (string TID in TagIds)
                    {
                        AssignTag = new MessageTag();
                        AssignTag.MasterMessageid = MessageID;
                        AssignTag.Tagid = Convert.ToDecimal(TID);
                        AssignTag.DeleteDate = null;
                        db.MessageTags.Add(AssignTag);
                    }
                }
                else
                {
                    db.MessageTags.RemoveRange(MessageTags);
                    foreach (string TagID in TagIds)
                    {
                        decimal TID = Convert.ToDecimal(TagID);
                        var NewMessageTags = db.MessageTags.FirstOrDefault(m => m.Tagid == TID && m.MasterMessageid == MessageID);
                        if (NewMessageTags == null)
                        {
                            AssignTag = new MessageTag();
                            AssignTag.MasterMessageid = MessageID;
                            AssignTag.Tagid = Convert.ToDecimal(TID);
                            AssignTag.DeleteDate = null;
                            db.MessageTags.Add(AssignTag);
                        }
                        else
                        {
                            NewMessageTags.DeleteDate = null;
                        }
                    }
                }
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
    }
}