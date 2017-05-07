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

    public class ContactController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        
        [Session]
        [HttpPost]
        public ActionResult AddNewContactDetail(string ImageUrl,string FName,string MName,string LName,string EmailAddress,string PhoneNo,string Address,string Note)
        {
            if (!string.IsNullOrEmpty(FName) && !string.IsNullOrEmpty(MName) && !string.IsNullOrEmpty(LName) && !string.IsNullOrEmpty(Address))
            {
                if (!string.IsNullOrEmpty(EmailAddress) || !string.IsNullOrEmpty(PhoneNo))
                {
                    PeopleSourceEntities db = new PeopleSourceEntities();
                    Contact Data = new Contact();
                    Data.CreateDate = DateTime.Now;
                    Data.FirstName = FName.Trim();
                    Data.MiddleName = MName.Trim();
                    Data.LastName = LName.Trim();
                    Data.EmailAddress = !string.IsNullOrEmpty(EmailAddress) ? EmailAddress.Trim() : null; 
                    Data.Phone = !string.IsNullOrEmpty(PhoneNo) ? PhoneNo.Trim() : null; 
                    Data.Address = Address.Trim();
                    Data.Note = !string.IsNullOrEmpty(Note) ? Note.Trim() : null;
                    Data.Image = !string.IsNullOrEmpty(ImageUrl) ? ImageUrl.Trim() : null;
                    db.Contacts.Add(Data);
                    db.SaveChanges();
                    return Json(new { success = true, Error = "",Data = Data}, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = true, Error ="Error"}, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
     
         [Session]
        [HttpPost]
        public ActionResult SaveEditedContactDetail(decimal ContactId, string ImageUrl, string FName, string MName, string LName, string EmailAddress, string PhoneNo, string Address, string Note)
        {
            if (!string.IsNullOrEmpty(FName) && !string.IsNullOrEmpty(MName) && !string.IsNullOrEmpty(LName) && !string.IsNullOrEmpty(Address))
            {
                if (!string.IsNullOrEmpty(EmailAddress) || !string.IsNullOrEmpty(PhoneNo))
                {
                    PeopleSourceEntities db = new PeopleSourceEntities();
                    Contact Data = db.Contacts.Find(ContactId);
                    Data.ModifyDate = DateTime.Now;
                    Data.FirstName = FName.Trim();
                    Data.MiddleName = MName.Trim();
                    Data.LastName = LName.Trim();
                    Data.EmailAddress = !string.IsNullOrEmpty(EmailAddress) ? EmailAddress.Trim() : null; 
                    Data.Phone = !string.IsNullOrEmpty(PhoneNo) ? PhoneNo.Trim() : null; 
                    Data.Address = Address.Trim();
                    Data.Note = !string.IsNullOrEmpty(Note) ? Note.Trim() : null;
                    Data.Image = !string.IsNullOrEmpty(ImageUrl) ? ImageUrl.Trim() : null;
                    db.SaveChanges();
                    return Json(new { success = true, Data = Data }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = true, Error ="Error"}, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
     
        
        [Session]
        [HttpPost]
        public ActionResult GetEditedContactDetail(decimal ContactId)
        {
            if (ContactId != 0)
            {
                    PeopleSourceEntities db = new PeopleSourceEntities();
                    var Data = db.Contacts.Find(ContactId);
                   if (Data != null)
                       return Json(new { success = true, Data = Data }, JsonRequestBehavior.AllowGet);
                   else
                       return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        
        [Session]
        public ActionResult GetContactDetails()
        {
            PeopleSourceEntities db = new PeopleSourceEntities();
            var ContactList = db.Contacts.Where(m => m.DeleteDate == null).ToList().OrderBy(m=>m.FirstName);
            if(ContactList != null)
                return Json(new {success = true,data = ContactList ,count = ContactList.Count()}, JsonRequestBehavior.AllowGet);
            else
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }


        [Session]
        [HttpPost]
        public ActionResult DeleteContact(decimal ContactId)
        {
            if (ContactId != 0)
            {
                    PeopleSourceEntities db = new PeopleSourceEntities();
                    var Data = db.Contacts.Find(ContactId);
                    if (Data != null)
                    {
                        Data.DeleteDate = DateTime.Now;
                        db.SaveChanges();
                        return Json(new { success = true, ContactId = Data.ContactId }, JsonRequestBehavior.AllowGet);
                    }
                    else
                        return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
    } 
}