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

    public class MessageScheduleController : Controller
    {

        public class MessageScheduleFields
        {
            public decimal MessageScheduleID { get; set; }

            public decimal id { get; set; }

            public string Seller { get; set; }

            public int Sellerid { get; set; }

            public string Subject { get; set; }

            public string Body { get; set; }

            public DateTime SendAt { get; set; }

            public DateTime CreatedDate { get; set; }

            public DateTime DeletedDate { get; set; }

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
        [HttpGet]
        public ActionResult GetMessageSchedule()
        {
            PeopleSourceEntities db = new PeopleSourceEntities();
            List<MessageScheduleFields> Schedule = new List<MessageScheduleFields>();

            var ScheduleList = db.MessageSchedules.Where(p => p.DeletedDate == null).ToList();
            if (ScheduleList.Count > 0)
            {
                foreach (var Data in ScheduleList)
                {
                    Schedule.Add(new MessageScheduleFields
                    {
                        id = Data.MessageScheduleid,
                        MessageScheduleID = Data.MessageScheduleid,
                        Seller = Data.Seller.SellarName,
                        Sellerid = Data.Sellerid,
                        Subject = Data.Subject,
                        Body = Data.Body,
                        SendAt = Data.Send_At
                    });
                }
            }
            return Json(Schedule, JsonRequestBehavior.AllowGet);
        }

        [Session]
        public ActionResult GetSchedulesList(decimal MessageScheduleID)
        {
            var model = new MessageSchedule();
            PeopleSourceEntities db = new PeopleSourceEntities();
            model = db.MessageSchedules.Find(MessageScheduleID);
            return PartialView("GetSchedulesList", model);
            //return Json( model.templateList, JsonRequestBehavior.AllowGet);
        }


        [Session]
        public ActionResult GetAllSeller()
        {
            PeopleSourceEntities db = new PeopleSourceEntities();
            var SellerList = db.Sellers.Select(m => new { SellerName = m.SellarName, SellerID = m.Sellerid }).ToList();
            return Json(new { success = true, SellerList = SellerList }, JsonRequestBehavior.AllowGet);
        }

        [Session]
        public ActionResult EditEbayMessageSchedule(decimal ScheduleID)
        {
            if (ScheduleID != 0)
            {
                PeopleSourceEntities db = new PeopleSourceEntities();
             //   var SellerList = db.Sellers.Select(m => new { SellerName = m.SellarName, SellerID = m.Sellerid }).ToList();
                var ScheduleData = db.MessageSchedules.Select(k => new { Subject = k.Subject, Send_At = k.Send_At, Body = k.Body, MessageScheduleid = k.MessageScheduleid, Sellerid = k.Sellerid, SellerName = k.Seller.SellarName}).FirstOrDefault(m => m.MessageScheduleid == ScheduleID);
                //return Json(new { success = true, SellerList = SellerList, ScheduleData = ScheduleData }, JsonRequestBehavior.AllowGet);
                return Json(new { success = true, ScheduleData = ScheduleData }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false}, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public ActionResult DeleteExistingEbayMsgSchedule(int ScheduleID)
        {

            PeopleSourceEntities db = new PeopleSourceEntities();
            if (ScheduleID != 0)
            {
                var MessagesScheduleList = db.MessageSchedules.Find(ScheduleID);

                if (MessagesScheduleList != null)
                {
                    MessagesScheduleList.DeletedDate = DateTime.Now;
                    db.SaveChanges();
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);

        }

        [Session]
        [HttpPost]
        public ActionResult SaveSchedule(string SellerLists = null, string Subject = null, string Body = null,string SendAt = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(SellerLists) && !string.IsNullOrEmpty(Subject) && !string.IsNullOrEmpty(Body) && !string.IsNullOrEmpty(SendAt))
                {
                    string[] SellerIDs = SellerLists.Split(',');
                    PeopleSourceEntities db = new PeopleSourceEntities();
                    List<MessageSchedule> MessageShceduleList = new List<MessageSchedule>();
                    var sellers = db.Sellers.ToList();
                    foreach (string Sellerid in SellerIDs)
                    {
                        int Id = Convert.ToInt32(Sellerid);
                        DateTime SendDate = DateTime.ParseExact(SendAt, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
                        MessageShceduleList.Add(new MessageSchedule()
                        {
                            Sellerid = Id,
                            Subject = Subject.Trim(),
                            Body = Body.Trim(),
                            Send_At =SendDate,
                            CreatedDate = DateTime.Now,
                            Status = "InCompleted"
                        });
                    }
                    db.MessageSchedules.AddRange(MessageShceduleList);
                    db.SaveChanges();
                    foreach(var s in MessageShceduleList){
                        s.Seller = new Seller() { SellarName = sellers.FirstOrDefault(m => m.Sellerid == s.Sellerid).SellarName };
                    }
                    return Json(new { success = true, MessageShcedule = MessageShceduleList }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
        
        [Session]
        [HttpPost]
        public ActionResult SaveEditedSchedule(decimal ScheduleID, string Subject, string Body, string SendAt = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(Subject) && !string.IsNullOrEmpty(Body) && !string.IsNullOrEmpty(SendAt))
                {
                   
                    PeopleSourceEntities db = new PeopleSourceEntities();
                    MessageSchedule MessageShceduleList = db.MessageSchedules.Find(ScheduleID);
                    var sellers = db.Sellers.ToList();
                    MessageShceduleList.Subject = Subject;
                    MessageShceduleList.Body = Body;
                    MessageShceduleList.Send_At = DateTime.ParseExact(SendAt, "dd-MM-yyyy HH:mm tt", CultureInfo.InvariantCulture);
                    MessageShceduleList.DeletedDate = null;
                    db.SaveChanges();
                    MessageShceduleList.Seller = new Seller() { SellarName = sellers.FirstOrDefault(m => m.Sellerid == MessageShceduleList.Sellerid).SellarName };
                    return Json(new { success = true, UpDatedValue = MessageShceduleList }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public ActionResult SaveEditedSchedule(decimal ScheduleID, string SellerLists, string Subject, string Body, string SendAt)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(SellerLists) && !string.IsNullOrEmpty(Subject) && !string.IsNullOrEmpty(Body) && !string.IsNullOrEmpty(SendAt.ToString()))
        //        {
        //            string[] SellerIDs = SellerLists.Split(',');
        //            PeopleSourceEntities db = new PeopleSourceEntities();
        //            //   MessageSchedule MessageShceduleList = new <MessageSchedule()>;
        //            var sellers = db.Sellers.ToList();
        //            List<MessageSchedule> NewValue = new List<MessageSchedule>();
        //            var MessageShceduleList = new MessageSchedule();
        //            foreach (string Sellerid in SellerIDs)
        //            {
        //                int SID = Convert.ToInt32(Sellerid);
        //                MessageShceduleList = db.MessageSchedules.FirstOrDefault(m => m.Sellerid == SID && m.MessageScheduleid == ScheduleID);
        //                if (MessageShceduleList != null)
        //                {
        //                    MessageShceduleList.Subject = Subject;
        //                    MessageShceduleList.Body = Body;
        //                    MessageShceduleList.Send_At = SendAt;
        //                    MessageShceduleList.DeletedDate = null;

        //                }
        //                else
        //                {
        //                    NewValue.Add(new MessageSchedule
        //                    {
        //                        Sellerid = SID,
        //                        Subject = Subject.Trim(),
        //                        Body = Body.Trim(),
        //                        Send_At = SendAt.Trim(),
        //                        CreatedDate = DateTime.Now
        //                    });
        //                }
        //            }
        //            db.MessageSchedules.AddRange(NewValue);
        //            db.SaveChanges();
        //            foreach (var s in NewValue)
        //            {
        //                s.Seller = new Seller() { SellarName = sellers.FirstOrDefault(m => m.Sellerid == s.Sellerid).SellarName };
        //            }
        //            return Json(new { success = true, NewAddedValue = NewValue, OldValue = MessageShceduleList }, JsonRequestBehavior.AllowGet);
        //        }
        //        return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        //}
     
    }
}