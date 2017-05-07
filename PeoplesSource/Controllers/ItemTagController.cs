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

    public class ItemTagController : Controller
    {
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
        [HttpPost]
        public ActionResult SaveTags(string TagList, int Sellerid,string Message)
        {
            try
            {
                if (!string.IsNullOrEmpty(TagList) && Sellerid != 0)
                {
                    PeopleSourceEntities db = new PeopleSourceEntities();
                    List<ItemTag> Data = new List<ItemTag>();
                    var sellers = db.Sellers.ToList();
                    Data.Add(new ItemTag
                    {
                        Sellerid =  Sellerid,
                        ItemTags =  TagList.ToString(),
                        CreateDate = DateTime.Now,
                        Message = Message.Trim()
                    });
                    db.ItemTags.AddRange(Data);
                    db.SaveChanges();
                    foreach (var s in Data)
                    {
                        s.Seller = new Seller() { SellarName = sellers.FirstOrDefault(m => m.Sellerid == s.Sellerid).SellarName };
                    }
                    return Json(new { success = true, Data = Data }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception e)
            {
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        [Session]
        public ActionResult GetItemTagDetails()
        {
            try{
                PeopleSourceEntities db = new PeopleSourceEntities();
                var ItemTagList = (from Data in db.ItemTags
                            where Data.DeleteDate == null
                            select new
                            {
                                id =Data.Itemtagid,
                                SellerName = Data.Seller.SellarName,
                                ItemTagID = Data.Itemtagid,
                                ItemTagsList = Data.ItemTags,
                                Sellerid= Data.Sellerid,
                                Message = Data.Message
                            }).ToList();
                return Json(ItemTagList, JsonRequestBehavior.AllowGet);
            } catch(Exception e)
            {
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
           
        [HttpPost]
        public ActionResult DeleteThankyouMessage(int ItemTagID)
        {

            PeopleSourceEntities db = new PeopleSourceEntities();
            if (ItemTagID != 0)
            {
                var ItemTag = db.ItemTags.Find(ItemTagID);
                if (ItemTag != null)
                {
                    ItemTag.DeleteDate = DateTime.Now;
                    db.SaveChanges();
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);

        }

        [Session]
        public ActionResult EditThankYouMessage(decimal ItemTagID)
        {
            if (ItemTagID != 0)
            {
                PeopleSourceEntities db = new PeopleSourceEntities();
                var ItemtagData = db.ItemTags.Select(k => new { Itemtagid = k.Itemtagid, ItemTags = k.ItemTags, Sellerid = k.Sellerid, Message  = k.Message}).FirstOrDefault(m => m.Itemtagid == ItemTagID);
                return Json(new { success = true, ItemtagData = ItemtagData }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false}, JsonRequestBehavior.AllowGet);
        }
        
        [Session]
        [HttpPost]
        public ActionResult SaveEditedTags(string TagList, int Sellerid, int ItemTagID, string Message)
        {
            try
            {
                if (!string.IsNullOrEmpty(TagList) && Sellerid != 0 && ItemTagID !=0)
                {
                    PeopleSourceEntities db = new PeopleSourceEntities();
                    ItemTag Data = db.ItemTags.Find(ItemTagID);
                    Data.ItemTags = TagList.Trim();
                    Data.Sellerid = Sellerid;
                    Data.Message = Message.Trim();
                    db.SaveChanges();
                    var sellers = db.Sellers.ToList();
                    Data.Seller = new Seller() { SellarName = sellers.FirstOrDefault(m => m.Sellerid == Data.Sellerid).SellarName };
                    return Json(new { success = true, Data = Data }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception e)
            {
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

    }
}