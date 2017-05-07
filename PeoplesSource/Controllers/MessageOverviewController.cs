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
using System.Globalization;

namespace PeoplesSource.Controllers
{

    public class MessageOverviewController : Controller
    {
        public class TagNames
        {
            public string TagName { get; set; }
            public int TagID { get; set; }
            public int Total { get; set; }
            public bool Checked { get; set; }
            public decimal TagsID { get; set; }
            public decimal MessageID { get; set; }

        }
        private const int PageSize = 10;
        private readonly ISellerServices _SellerService;

        public MessageOverviewController(ISellerServices SellerService)
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

        //public ActionResult Angular()
        //{
        //    return View();
        //}

        [Session]
        public ActionResult GetMessageOverview()
        {
            string error = "";//Central Standard Time
            GetMyMessagesResponse view = null;
            var sellerList = _SellerService.GetSeller().ToList();
            PeopleSourceEntities db = new PeopleSourceEntities();
            List<Message> Newview = new List<Message>();
            var MessageList = (from Datas in db.MasterMessages
                               where Datas.DeleteDate == null
                               orderby Datas.MessageRecieveDate descending
                               select new Message                                                                                                                                 
                               {                                                                                                                                                  
                                   MessageID = Datas.MasterMessageid.ToString(),                                                                                                        
                                   Subject = Datas.MessageSubject,                                                                                                                
                                   SendingUserID = Datas.Sellerid.ToString(),
                                   ReceiveDate = Datas.MessageRecieveDate != null ? (DateTime)Datas.MessageRecieveDate : Datas.MessageRecieveDate.Value,                          
                                   ItemID = Datas.MessageItemid,                                                                                                                  
                                   RecipientUserID = Datas.Seller.SellarName,                                                                                                     
                                   Sender = Datas.Sender,                                                                                                                         
                                   MessageType = Datas.MessageType,                                                                                                               
                                   Read = Datas.IsRead,                                                                                                                             
                                   Type = Datas.Type,                                                                                                                             
                                   TagDetail = (from Data in db.MessageTags
                                                where Data.DeleteDate == null && Data.MasterMessageid == Datas.MasterMessageid                                                                
                                                select new TagList                                                                                                                
                                                {                                                                                                                                 
                                                    TagsID = Data.Tagid,                                                                                                          
                                                    TagName = Data.Tag.TagName                                                                                                    
                                            }).ToList()
                               }).ToList();

            MessageList = MessageList.OrderBy(m => m.Read).ToList();
            
            if(MessageList != null)
                return Json(MessageList, JsonRequestBehavior.AllowGet);
            else
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
         }

        [Session]
        public ActionResult GetMessageOverviewById(decimal TagID)
        {
            PeopleSourceEntities db = new PeopleSourceEntities();
            List<Message> Newview = new List<Message>();

            var MessageList = db.MessageTags.Where(p=>p.Tagid == TagID && p.DeleteDate==null).Select(m=>m.MasterMessage).ToList();

            if (MessageList != null)
            {
                foreach (var MessageData in MessageList)
                {

                    if (MessageData.Type == true)
                    {
                        Newview.Add(new Message
                        {
                            MessageID = MessageData.MasterMessageid.ToString(),
                            Subject = MessageData.MessageSubject,
                            SendingUserID = MessageData.Sellerid.ToString(),
                            ReceiveDate = MessageData.MessageRecieveDate.Value != null ? MessageData.MessageRecieveDate.Value : DateTime.Now,
                            ExpirationDate = MessageData.MessageExpiryDate.Value != null ? MessageData.MessageExpiryDate.Value : DateTime.Now,
                            ItemID = MessageData.MessageItemid,
                            RecipientUserID = MessageData.Seller.SellarName,
                            Sender = MessageData.Sender,
                            MessageType = MessageData.MessageType,
                            Type =MessageData.Type,
                            Read = MessageData.IsRead,
                            TagDetail = (from Data in db.MessageTags
                                         where Data.DeleteDate == null && Data.MasterMessageid == MessageData.MasterMessageid
                                         select new TagList
                                         {
                                             TagsID = Data.Tagid,
                                             TagName = Data.Tag.TagName
                                         }).ToList()
                        });
                    }
                    else
                    {
                        Newview.Add(new Message
                        {
                            MessageID = MessageData.MasterMessageid.ToString(),
                            SendingUserID = MessageData.Sellerid.ToString(),
                            Subject = MessageData.MessageSubject,
                            ItemID = MessageData.MessageItemid,
                            Sender = MessageData.Sender,
                            MessageType = MessageData.MessageType,
                            RecipientUserID = MessageData.Seller.SellarName,
                            Read = MessageData.IsRead,
                            Type = MessageData.Type,
                            TagDetail = (from Data in db.MessageTags
                                         where Data.DeleteDate == null && Data.MasterMessageid == MessageData.MasterMessageid
                                         select new TagList
                                         {
                                             TagsID = Data.Tagid,
                                             TagName = Data.Tag.TagName
                                         }).ToList()
                        });
                    }
                }
            }
            return Json(Newview, JsonRequestBehavior.AllowGet);
        }

        [Session]
        [HttpPost]
        public ActionResult AddNewTag(string tagname)
        {
            try
            {

                if (!string.IsNullOrEmpty(tagname))
                {
                    PeopleSourceEntities db = new PeopleSourceEntities();
                    Tag Exists = db.Tags.FirstOrDefault(m => m.TagName.ToLower().Trim() == tagname.ToLower().Trim());
                    if (Exists == null)
                    {
                        Tag Tags = new Tag();
                        Tags.TagName = tagname.Trim().First().ToString().ToUpper() + String.Join("", tagname.Skip(1)).Trim();
                        Tags.CreatedDate = DateTime.Now;
                        Tags.DeletedDate = null;
                        db.Tags.Add(Tags);
                        db.SaveChanges();
                        return Json(new { success = true, Data = Tags }, JsonRequestBehavior.AllowGet);
                    }
                    else if (Exists.DeletedDate != null)
                    {
                        Exists.DeletedDate = null;
                        Exists.CreatedDate = DateTime.Now;
                        db.SaveChanges();
                        return Json(new { success = true, message = "Deleted", DataID = Exists.Tagid, DataName = Exists.TagName }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = true, message = "Exists" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception e)
            {
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        [Session]
        public ActionResult GetAllTags()
        {
            PeopleSourceEntities db = new PeopleSourceEntities();
            var TagList = (from Data in db.Tags
                           where Data.DeletedDate == null
                           select new
                           {
                               TagID = Data.Tagid,
                               TagName = Data.TagName,
                               Total = db.MessageTags.Count(m => m.Tagid == Data.Tagid && m.DeleteDate == null)
                           }).ToList();
            return Json(new { success = true, AllTags = TagList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteTag(int ID)
        {
            if (ID != 0)
            {
                PeopleSourceEntities db = new PeopleSourceEntities();
                Tag Tags = db.Tags.Find(ID);
                if (Tags != null)
                {
                    Tags.DeletedDate = DateTime.Now;
                    var ListTagArray = new List<TagNames>();
                    List<MessageTag> MessageTags = new List<MessageTag>();
                    MessageTags = db.MessageTags.Where(m => m.Tagid == ID).ToList();
                    foreach (MessageTag SingleTags in MessageTags)
                    {
                        SingleTags.DeleteDate = DateTime.Now;
                        ListTagArray.Add(new TagNames
                        {
                            TagName = SingleTags.Tag.TagName,
                            TagsID = SingleTags.Tagid,
                            MessageID = SingleTags.MasterMessageid
                        });
                    }
                    db.SaveChanges();
                    return Json(new { success = true, ListTagArray = ListTagArray }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult DeleteExistingTag(int ID, decimal MID)
        {
            if (ID != 0)
            {
                PeopleSourceEntities db = new PeopleSourceEntities();
                if (MID != 0)
                {
                    var MessageTags = db.MessageTags.FirstOrDefault(m => m.Tagid == ID && m.MasterMessageid == MID);
                    if (MessageTags != null)
                    {
                        MessageTags.DeleteDate = DateTime.Now;
                        db.SaveChanges();
                        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public ActionResult DeleteExistingEbayMsg(int MID)
        {
           
                PeopleSourceEntities db = new PeopleSourceEntities();
                if (MID != 0)
                {
                    var MessagesList = db.MasterMessages.Find(MID);

                    if (MessagesList != null)
                    {
                        var ListTagArray = new List<TagNames>();
                        MessagesList.DeleteDate = DateTime.Now;
                        if (MessagesList.MessageTags.Count > 0)
                        {
                            foreach (MessageTag Tags in MessagesList.MessageTags)
                            {
                                Tags.DeleteDate = DateTime.Now;
                                ListTagArray.Add(new TagNames
                                {
                                    TagName = Tags.Tag.TagName,
                                    TagsID = Tags.Tagid,
                                    MessageID = MessagesList.MasterMessageid
                                });
                            }
                        }
                        db.SaveChanges();
                        return Json(new { success = true, ListTagArray = ListTagArray }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult DeleteSelectedEbayMsg(string MID)
        {

                PeopleSourceEntities db = new PeopleSourceEntities();
                if (!string.IsNullOrEmpty(MID))
                {
                    string[] MessageIDs = MID.Split(',');
                    var ListTagArray = new List<TagNames>();
                    MasterMessage MessagesList = new MasterMessage();
                    foreach (string MessageID in MessageIDs)
                    {
                        if (!string.IsNullOrEmpty(MessageID))
                        {
                            Decimal ID = Convert.ToDecimal(MessageID);
                            MessagesList = db.MasterMessages.Find(ID);
                            if (MessagesList != null)
                            {
                                MessagesList.DeleteDate = DateTime.Now;
                                if (MessagesList.MessageTags.Count > 0)
                                {
                                    foreach (MessageTag Tags in MessagesList.MessageTags)
                                    {
                                        Tags.DeleteDate = DateTime.Now;
                                        ListTagArray.Add(new TagNames
                                        {
                                            TagName = Tags.Tag.TagName,
                                            TagsID = Tags.Tagid,
                                            MessageID = MessagesList.MasterMessageid
                                        });
                                    }
                                }
                            }
                        }
                    }
                    db.SaveChanges();
                    return Json(new { success = true, ListTagArray = ListTagArray }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
        
        [Session]
        [HttpPost]
        public ActionResult GetEditedData(int ID)
        {
            if (ID != 0)
            {
                PeopleSourceEntities db = new PeopleSourceEntities();
                Tag Tags = db.Tags.Find(ID);
                if (Tags != null)
                {
                    return Json(new { success = true, Tagid = Tags.Tagid, TagName = Tags.TagName }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);

        }

        [Session]
        [HttpPost]
        public ActionResult EditTag(string tagname, int ID)
        {

            if (!string.IsNullOrEmpty(tagname))
            {
                PeopleSourceEntities db = new PeopleSourceEntities();
                var Exists = db.Tags.FirstOrDefault(m => m.TagName.ToLower().Trim() == tagname.ToLower().Trim() && m.Tagid != ID);
                if (Exists == null)
                {
                    var Tag = db.Tags.Find(ID);
                    Tag.TagName = tagname.Trim().First().ToString().ToUpper() + String.Join("", tagname.Skip(1)).Trim();
                    db.SaveChanges();
                    return Json(new { success = true, Tagid = Tag.Tagid, TagName = Tag.TagName }, JsonRequestBehavior.AllowGet);
                }
                else if (Exists.DeletedDate != null)
                {
                    Exists.DeletedDate = null;
                    Exists.CreatedDate = DateTime.Now;
                    Exists.TagName = tagname.Trim().First().ToString().ToUpper() + String.Join("", tagname.Skip(1)).Trim();
                    db.SaveChanges();
                    return Json(new { success = true, Tagid = Exists.Tagid, TagName = Exists.TagName }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = true, message = "Exists" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        [Session]
        [HttpPost]
        public ActionResult AssignTag(string TagIdList, string CheckedTagList)
        {
            if (!string.IsNullOrEmpty(TagIdList) && !string.IsNullOrEmpty(CheckedTagList))
            {
                PeopleSourceEntities db = new PeopleSourceEntities();
                string[] TagIds = CheckedTagList.Split(',');
                string[] MessageIds = TagIdList.Split(',');
                List<MessageTag> MessageTags;
                MessageTag AssignTag = new MessageTag();
                foreach (string MID in MessageIds)
                {
                    decimal MessageID = Convert.ToDecimal(MID);
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

                }
                db.SaveChanges();
                var TagArray = new List<TagNames>();

                foreach (Tag TagID in db.Tags.ToList())
                {
                    int TID = Convert.ToInt32(TagID.Tagid);
                    TagArray.Add(new TagNames
                    {
                        Total = db.MessageTags.Count(m => m.Tagid == TID && m.DeleteDate == null),
                        TagID = TID
                    });
                }
                var ListTagArray = new List<TagNames>();
                foreach (var MsgID in MessageIds)
                {
                    decimal MID = Convert.ToDecimal(MsgID);
                    var MessageTagArray = db.MessageTags.Where(m => m.DeleteDate == null && m.MasterMessageid == MID).ToList();
                    foreach (var Data in MessageTagArray)
                    {
                        ListTagArray.Add(new TagNames
                        {
                            TagName = Data.Tag.TagName,
                            TagsID = Data.Tagid,
                            MessageID = Data.MasterMessageid
                        });
                    }
                }
                return Json(new { success = true, AllTags = TagArray, MessageTagArray = ListTagArray }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        [Session]
        [HttpPost]
        public ActionResult SetUnReadOrRead(string MessageList, string Type)
        {
            if (!string.IsNullOrEmpty(MessageList))
            {
                string[] Message = MessageList.Split(',');
                PeopleSourceEntities db = new PeopleSourceEntities();
                if (Type.ToLower().Trim() == "unread")
                {
                    foreach (var ID in Message)
                    {
                        decimal MID = Convert.ToDecimal(ID);
                        var MessageTable = db.MasterMessages.Find(MID);
                        if (MessageTable != null)
                        {
                            MessageTable.IsRead = false;
                        }
                    }
                }
                else
                {
                    foreach (var ID in Message)
                    {
                        decimal MID = Convert.ToDecimal(ID);
                        var MessageTable = db.MasterMessages.Find(MID);
                        if (MessageTable != null)
                        {
                            MessageTable.IsRead = true;
                        }
                    }
                }
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
           }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        [Session]
        [HttpPost]
        public ActionResult ShowNote(int MsgID)
        {
            if (MsgID != 0)
            {
                PeopleSourceEntities db = new PeopleSourceEntities();
                var Notes = db.MasterMessages.Find(MsgID);
                return Json(new { success = true, Notes = Notes.Note}, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        [Session]
        [HttpPost]
        public ActionResult SaveNote(int MsgID, string Note)
        {
            if (MsgID != 0)
            {
                PeopleSourceEntities db = new PeopleSourceEntities();
                var Notes = db.MasterMessages.Find(MsgID);
                Notes.Note = Note.Trim();
                db.SaveChanges();
                return Json(new { success = true}, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

      
        [Session]
        public ActionResult GetFilterMessageOverview(string Type = null, string GlobalString = null, string HasWord = null, string CatagoryList = null, bool chkboxattchment = false, bool chkboxnot = false, string Subject = null, string From = "", string To = "" ,string FromDate="",string ToDate= "")
        {
            if (Type == "FilterSearch")
            {
                if (string.IsNullOrEmpty(HasWord) && string.IsNullOrEmpty(CatagoryList) && string.IsNullOrEmpty(Subject) && string.IsNullOrEmpty(From) && string.IsNullOrEmpty(To) && chkboxattchment == false && chkboxnot == false && string.IsNullOrEmpty(FromDate) && string.IsNullOrEmpty(ToDate))
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    GetMyMessagesResponse view = null;
                    var sellerList = _SellerService.GetSeller().ToList();
                    PeopleSourceEntities db = new PeopleSourceEntities();
                    List<Message> Newview = new List<Message>();
                    var MessagesList = (from Datas in db.MasterMessages
                                        where Datas.DeleteDate == null
                                        select new Message
                                        {
                                            MessageID = Datas.MasterMessageid.ToString(),
                                            Subject = Datas.MessageSubject,
                                            SendingUserID = Datas.Sellerid.ToString(),
                                            ReceiveDate = Datas.MessageRecieveDate != null ? (DateTime)Datas.MessageRecieveDate : Datas.MessageRecieveDate.Value,
                                            ItemID = Datas.MessageItemid,
                                            RecipientUserID = Datas.Seller.SellarName,
                                            Sender = Datas.Sender,
                                            MessageType = Datas.MessageType,
                                            Read = Datas.IsRead,
                                            Type = Datas.Type,
                                            Note = Datas.Note,
                                            TagDetail = (from Data in db.MessageTags
                                                         where Data.DeleteDate == null && Data.MasterMessageid == Datas.MasterMessageid
                                                         select new TagList
                                                         {
                                                             TagsID = Data.Tagid,
                                                             TagName = Data.Tag.TagName
                                                         }).ToList()
                                        });
                    if (!string.IsNullOrEmpty(HasWord))
                    {
                        MessagesList = MessagesList.Where(m => m.Note.Contains(HasWord) || m.Subject.Contains(HasWord) || m.Sender.Contains(HasWord) || m.MessageType.Contains(HasWord));
                    }
                    if (!string.IsNullOrEmpty(From))
                    {
                        MessagesList = MessagesList.Where(m => m.Sender.Contains(From));
                    }
                    if (!string.IsNullOrEmpty(To))
                    {
                        MessagesList = MessagesList.Where(m => m.RecipientUserID.Contains(To));
                    }
                    if (!string.IsNullOrEmpty(CatagoryList))
                    {
                        MessagesList = MessagesList.Where(m => (m.TagDetail.Where(k => k.TagName.ToLower().Trim() == CatagoryList.ToLower().Trim())).Count() > 0);
                    }
                    if (!string.IsNullOrEmpty(Subject))
                    {
                        MessagesList = MessagesList.Where(m => m.Subject.Contains(Subject));
                    }
                    if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
                    {
                        DateTime FromDatetime = DateTime.ParseExact(ToDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                        if(FromDate.Trim() == "1 Day"){
                            FromDatetime = FromDatetime.AddDays(-1);
                        }else if(FromDate.Trim() == "3 Day")
                        {
                             FromDatetime = FromDatetime.AddDays(-3);
                        }else  if(FromDate.Trim() == "1 Week")
                        {
                             FromDatetime = FromDatetime.AddDays(-7);
                        }else  if(FromDate.Trim() == "2 Week")
                        {
                             FromDatetime = FromDatetime.AddDays(-14);
                        }else  if(FromDate.Trim() == "1 Month")
                        {
                             FromDatetime = FromDatetime.AddDays(-30);
                        }else  if(FromDate.Trim() == "2 Month")
                        {
                             FromDatetime = FromDatetime.AddDays(-60);
                        }else  if(FromDate.Trim() == "6 Month")
                        {
                             FromDatetime = FromDatetime.AddDays(-180);
                        }
                        else if (FromDate.Trim() == "1 Year")
                        {
                         FromDatetime = FromDatetime.AddDays(-360);
                        }
                        DateTime? ToDatetime = DateTime.ParseExact(ToDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                        MessagesList = MessagesList.Where(m => m.ReceiveDate >= FromDatetime && m.ReceiveDate <= ToDatetime);
                    }
                    if (chkboxnot == true)
                    {
                        MessagesList = MessagesList.Where(m => m.Note != null);
                    }
                    return Json(MessagesList, JsonRequestBehavior.AllowGet);
                }
            }
            else if (Type == "Global")
            {
                if (!string.IsNullOrEmpty(GlobalString))
                {
                    PeopleSourceEntities db = new PeopleSourceEntities();
                    var MessagesList = (from Datas in db.MasterMessages
                                        where Datas.DeleteDate == null && Datas.Note.Contains(GlobalString) || Datas.MessageSubject.Contains(GlobalString) || Datas.Sender.Contains(GlobalString) || Datas.MessageType.Contains(GlobalString)
                                        select new Message
                                        {
                                            MessageID = Datas.MasterMessageid.ToString(),
                                            Subject = Datas.MessageSubject,
                                            SendingUserID = Datas.Sellerid.ToString(),
                                            ReceiveDate = Datas.MessageRecieveDate != null ? (DateTime)Datas.MessageRecieveDate : Datas.MessageRecieveDate.Value,
                                            ItemID = Datas.MessageItemid,
                                            RecipientUserID = Datas.Seller.SellarName,
                                            Sender = Datas.Sender,
                                            MessageType = Datas.MessageType,
                                            Read = Datas.IsRead,
                                            Type = Datas.Type,
                                            Note = Datas.Note,
                                            TagDetail = (from Data in db.MessageTags
                                                         where Data.DeleteDate == null && Data.MasterMessageid == Datas.MasterMessageid
                                                         select new TagList
                                                         {
                                                             TagsID = Data.Tagid,
                                                             TagName = Data.Tag.TagName
                                                         }).ToList()
                                        });
                    return Json(MessagesList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
            }
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        [Session]
        public ActionResult GetCustomeFilterMessageOverview(string GlobalString = null, string HasWord = null, string FilterTagName = null, bool chkboxattchment = false, bool chkboxnot = false, string Subject = null, string From = "", string To = "", string FromDate = "", string ToDate = "", bool AllRead = false, bool AllUnRead = false, bool AllDelete = false, decimal TagID = 0, string Note = null)
        {
            if (string.IsNullOrEmpty(HasWord) && string.IsNullOrEmpty(FilterTagName) && string.IsNullOrEmpty(Subject) && string.IsNullOrEmpty(From) && string.IsNullOrEmpty(To) && chkboxattchment == false && chkboxnot == false && string.IsNullOrEmpty(FromDate) && string.IsNullOrEmpty(ToDate))
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                GetMyMessagesResponse view = null;
                string FilterFromDate = null;
                string FilterToDate = null;
                var sellerList = _SellerService.GetSeller().ToList();
                PeopleSourceEntities db = new PeopleSourceEntities();
                var MessagesList = db.MasterMessages.Where(m=>m.DeleteDate == null);

                if (!string.IsNullOrEmpty(HasWord))
                {
                    MessagesList = MessagesList.Where(m => m.Note.Contains(HasWord) || m.MessageSubject.Contains(HasWord) || m.Sender.Contains(HasWord) || m.MessageType.Contains(HasWord));
                }
                if (!string.IsNullOrEmpty(From))
                {
                    MessagesList = MessagesList.Where(m => m.Sender.Contains(From));
                }
                if (!string.IsNullOrEmpty(To))
                {
                    MessagesList = MessagesList.Where(m => m.Seller.SellarName.Contains(To));
                }
                if (!string.IsNullOrEmpty(FilterTagName))
                {
                    MessagesList = MessagesList.Where(m => (m.MessageTags.Where(k => k.Tag.TagName.Trim() == FilterTagName.ToLower().Trim())).Count() > 0);
                }
                if (!string.IsNullOrEmpty(Subject))
                {
                    MessagesList = MessagesList.Where(m => m.MessageSubject.Contains(Subject));
                }
                if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
                {
                    DateTime FromDatetime = DateTime.ParseExact(ToDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                    if (FromDate.Trim() == "1 Day")
                    {
                        FromDatetime = FromDatetime.AddDays(-1);
                    }
                    else if (FromDate.Trim() == "3 Day")
                    {
                        FromDatetime = FromDatetime.AddDays(-3);
                    }
                    else if (FromDate.Trim() == "1 Week")
                    {
                        FromDatetime = FromDatetime.AddDays(-7);
                    }
                    else if (FromDate.Trim() == "2 Week")
                    {
                        FromDatetime = FromDatetime.AddDays(-14);
                    }
                    else if (FromDate.Trim() == "1 Month")
                    {
                        FromDatetime = FromDatetime.AddDays(-30);
                    }
                    else if (FromDate.Trim() == "2 Month")
                    {
                        FromDatetime = FromDatetime.AddDays(-60);
                    }
                    else if (FromDate.Trim() == "6 Month")
                    {
                        FromDatetime = FromDatetime.AddDays(-180);
                    }
                    else if (FromDate.Trim() == "1 Year")
                    {
                        FromDatetime = FromDatetime.AddDays(-360);
                    }
                    DateTime? ToDatetime = DateTime.ParseExact(ToDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                    FilterFromDate = FromDatetime.ToString();
                    FilterToDate = ToDatetime.ToString();
                    MessagesList = MessagesList.Where(m => m.MessageRecieveDate >= FromDatetime && m.MessageRecieveDate <= ToDatetime);
                }
                if (chkboxnot == true)
                {
                    MessagesList = MessagesList.Where(m => m.Note != null);
                }

                List<MessageTag> AssignTag = new List<MessageTag>();
                foreach (var Data in MessagesList)
                {
                    if (AllRead == true)
                    {
                        Data.IsRead = true;
                    }
                    else if (AllUnRead == true)
                    {
                        Data.IsRead = false;
                    }
                    if (!string.IsNullOrEmpty(Note))
                    {
                        Data.Note = Note;
                    }
                    if (AllDelete == true)
                    {
                        Data.DeleteDate = DateTime.Now;

                        foreach (var TagData in Data.MessageTags)
                        {
                            TagData.DeleteDate = DateTime.Now;
                        }
                    }
                    if (TagID != 0)
                    {
                        if (AllDelete == true)
                        {
                            AssignTag.Add(new MessageTag
                            {
                                MasterMessageid = Convert.ToDecimal(Data.MasterMessageid),
                                Tagid = TagID,
                                DeleteDate = DateTime.Now
                            });
                        }
                        else
                        {
                            decimal MsgID = Convert.ToDecimal(Data.MasterMessageid);
                            int Check = db.MessageTags.Where(m => m.Tagid == TagID && m.MasterMessageid == MsgID && m.DeleteDate == null).Count();
                            if (Check == 0)
                            {
                                AssignTag.Add(new MessageTag
                                {
                                    MasterMessageid = Convert.ToDecimal(Data.MasterMessageid),
                                    Tagid = TagID
                                });
                            }
                        }
                    }
                }
                if (TagID != 0)
                {
                    db.MessageTags.AddRange(AssignTag);
                }

                Filteration Filter = new Filteration();
                Filter.Action_Note = !string.IsNullOrEmpty(Note) ? Note.Trim() : null;
                Filter.Tagid = TagID > 0 ? TagID : default(decimal?);
                Filter.Action_Type = AllRead == true ? "Read" : AllUnRead == true ? "UnRead" : "Nothing";
                Filter.Action_Delete = AllDelete == true ? true : false;
                Filter.Filter_From = !string.IsNullOrEmpty(From) ?  From.Trim() : null;
                Filter.Filter_FromDate =  !string.IsNullOrEmpty(FilterFromDate) ?  FilterFromDate.Trim() : null;
                Filter.Filter_HasNote = chkboxnot;
                Filter.Filter_Subject =  !string.IsNullOrEmpty(Subject) ? Subject.Trim() : null;
                Filter.Filter_ToDate =  !string.IsNullOrEmpty(FilterToDate) ?  FilterToDate.Trim() : null;
                Filter.Filter_TagName = !string.IsNullOrEmpty(FilterTagName) ? FilterTagName.Trim() : null;
                Filter.Filter_To = !string.IsNullOrEmpty(To) ?  To.Trim() : null;
                Filter.Filter_HasWord = !string.IsNullOrEmpty(HasWord) ? HasWord.Trim() : null;
                db.Filterations.Add(Filter);
                db.SaveChanges();
                if (AllDelete == true)
                {

                    var NewMessagesList = (from Datas in db.MasterMessages
                                        where Datas.DeleteDate == null
                                        select new Message
                                        {
                                            MessageID = Datas.MasterMessageid.ToString(),
                                            Subject = Datas.MessageSubject,
                                            SendingUserID = Datas.Sellerid.ToString(),
                                            ReceiveDate = Datas.MessageRecieveDate != null ? (DateTime)Datas.MessageRecieveDate : Datas.MessageRecieveDate.Value,
                                            ItemID = Datas.MessageItemid,
                                            RecipientUserID = Datas.Seller.SellarName,
                                            Sender = Datas.Sender,
                                            MessageType = Datas.MessageType,
                                            Read = Datas.IsRead,
                                            Type = Datas.Type,
                                            Note = Datas.Note,
                                            DeleteDate = Datas.DeleteDate,
                                            TagDetail = (from Data in db.MessageTags
                                                         where Data.DeleteDate == null && Data.MasterMessageid == Datas.MasterMessageid
                                                         select new TagList
                                                         {
                                                             TagsID = Data.Tagid,
                                                             TagName = Data.Tag.TagName
                                                         }).ToList()
                                        });
                    return Json(NewMessagesList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var NewMessagesList = (from Datas in MessagesList
                                           select new Message
                                           {
                                               MessageID = Datas.MasterMessageid.ToString(),
                                               Subject = Datas.MessageSubject,
                                               SendingUserID = Datas.Sellerid.ToString(),
                                               ReceiveDate = Datas.MessageRecieveDate != null ? (DateTime)Datas.MessageRecieveDate : Datas.MessageRecieveDate.Value,
                                               ItemID = Datas.MessageItemid,
                                               RecipientUserID = Datas.Seller.SellarName,
                                               Sender = Datas.Sender,
                                               MessageType = Datas.MessageType,
                                               Read = Datas.IsRead,
                                               Type = Datas.Type,
                                               Note = Datas.Note,
                                               DeleteDate = Datas.DeleteDate,
                                               TagDetail = (from Data in db.MessageTags
                                                            where Data.DeleteDate == null && Data.MasterMessageid == Datas.MasterMessageid
                                                            select new TagList
                                                            {
                                                                TagsID = Data.Tagid,
                                                                TagName = Data.Tag.TagName
                                                            }).ToList()
                                           });
                    return Json(NewMessagesList, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
    }
}