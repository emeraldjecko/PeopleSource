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

    public class FilterController : Controller
    {
        private const int PageSize = 10;
        public class NewMasterMessage
        {
            public decimal SellerID { get; set; }
            public decimal MessageID { get; set; }
            public bool Type { get; set; }
          
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
        public ActionResult GetFilterList()
        {
            try
            {
                PeopleSourceEntities db = new PeopleSourceEntities();//['FilterID', 'From', 'To', 'Subject', 'HasWord', 'HasNote','HasTagName','FromDate','ToDate','Action'],
                var FilterList = db.Filterations.Where(m => m.DeleteDate == null).Select(k => new { FilterID = k.FilterID, Filter_From = k.Filter_From, Filter_To = k.Filter_To, Filter_Subject = k.Filter_Subject, Filter_HasWord = k.Filter_HasWord, Filter_HasNote = k.Filter_HasNote, Filter_TagName = k.Filter_TagName, Filter_FromDate = k.Filter_FromDate, Filter_ToDate = k.Filter_ToDate }).ToList();
                if (FilterList != null)
                {
                    return Json(FilterList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception e) {
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Session]
        public ActionResult DeleteFilter(decimal FilterID)
        {
            try {
                    if (FilterID != 0)
                    {
                        PeopleSourceEntities db = new PeopleSourceEntities();
                        Filteration FilterData = new Filteration();
                        FilterData = db.Filterations.Find(FilterID);
                        FilterData.DeleteDate = DateTime.Now;
                        db.SaveChanges();
                        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                    }
            }
            catch(Exception e) {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
        
         [HttpPost]
        [Session]
        public ActionResult GetFilterAction(decimal FilterID)
        {
            try {
                    if (FilterID != 0)
                    {
                        PeopleSourceEntities db = new PeopleSourceEntities();
                        var FilterData = db.Filterations.Where(m => m.FilterID == FilterID).Select(k => new {TagName = !string.IsNullOrEmpty(k.Tagid.ToString()) ?  k.Tag.TagName :  "Tag Not Assign", Delete = k.Action_Delete, Read = k.Action_Type == "Read" ? true : false, UnRead = k.Action_Type == "UnRead" ? true : false, Note = k.Action_Note != null ? k.Action_Note : "Note Not Assign" });
                        return Json(new { success = true, ActionList = FilterData }, JsonRequestBehavior.AllowGet);
                    }
            }
            catch(Exception e) {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

         [HttpPost]
         [Session]
         public ActionResult GetLinkData(decimal MsgId,string Type)
         {
             try
             {
                 if (MsgId != 0)
                 {
                     var MessageData = new List<NewMasterMessage>();
                     var SingleData = new NewMasterMessage();
                     int count;
                     PeopleSourceEntities db = new PeopleSourceEntities();
                     if (Type == "Prev")
                     {
                         MessageData = (from Data in db.MasterMessages
                                        where Data.DeleteDate == null && Data.MasterMessageid < MsgId
                                        orderby Data.MasterMessageid descending
                                        select new NewMasterMessage
                                        {
                                            SellerID = Data.Sellerid,
                                            MessageID = Data.MasterMessageid,
                                            Type = Data.Type
                                        }).ToList();
                         count = MessageData.Count();
                         SingleData = MessageData.FirstOrDefault();
                     }
                     else
                     {
                         MessageData = (from Data in db.MasterMessages
                                        where Data.DeleteDate == null && Data.MasterMessageid > MsgId
                                        select new NewMasterMessage
                                        {
                                            SellerID = Data.Sellerid,
                                            MessageID = Data.MasterMessageid,
                                            Type = Data.Type
                                        }).ToList();
                         count = MessageData.Count();
                         SingleData = MessageData.FirstOrDefault();
                     }
                     return Json(new { success = true,count = count, Data = SingleData }, JsonRequestBehavior.AllowGet);
                 }
             }
             catch (Exception e)
             {
                 return Json(new { success = false }, JsonRequestBehavior.AllowGet);
             }
             return Json(new { success = false }, JsonRequestBehavior.AllowGet);
         }
    }
}