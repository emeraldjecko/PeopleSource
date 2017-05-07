using PeoplesSource.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PeoplesSource.Ebay.Models;
using PeoplesSource.Helpers;
using PeoplesSource.EWReturn;
using PeoplesSource.Models;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Globalization;

namespace MessageScheduler
{
    class Program
    {
     

        static void Main(string[] args)
        {
            GetSchedule();
        }

        private static void GetSchedule()
        {
            try {
                PeopleSourceEntities db = new PeopleSourceEntities();
                var ScheduleList = db.MessageSchedules.Where(m => m.DeletedDate == null && m.Status == "Completed" && m.Send_At <= DateTime.Now).ToList();
                if (ScheduleList != null)
                {
                    var ContactList = db.Contacts.Where(m => m.DeleteDate == null && m.EmailAddress != null).Select(k => new { EmailAddress = k.EmailAddress, ContactIds  = k.ContactId}).ToList();
                   // string[] EmailAddress = new string[ContactList.Count];
                   // decimal[] ContactIds = new decimal[ContactList.Count];
                    foreach(var Schdule in ScheduleList)
                    {
                        int i = 0;
                        //foreach (var Contact in ContactList)
                        //{
                        //    EmailAddress[i] = Contact.EmailAddress.Trim();
                        //    ContactIds[i] = Contact.ContactId;
                        //    i = i + 1;
                        //}
                         MailMessage mail = new MailMessage();
                         SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                         mail.From = new MailAddress(Schdule.Seller.Email);
                         var EmailAddresses = string.Join(",", ContactList.Select(k=>k.EmailAddress));
                         mail.To.Add(EmailAddresses);
                         mail.Subject = Schdule.Subject.Trim();
                         mail.Body = Schdule.Body.Trim(); ;

                         SmtpServer.Port = 587;
                         SmtpServer.Credentials = new System.Net.NetworkCredential(Schdule.Seller.Email, "techinfoplace2@15");
                         SmtpServer.EnableSsl = true;
                         List<ManageScheduleMessage> Confirmation = new List<ManageScheduleMessage>();
                         try
                         {
                             SmtpServer.Send(mail);
                             foreach (decimal Id in ContactList.Select(k=>k.ContactIds))
                             {
                                 Confirmation.Add(new ManageScheduleMessage
                                 {
                                     Status = "Sent",
                                     MessageScheduleid = Schdule.MessageScheduleid,
                                     Send_At= Schdule.Send_At,
                                     ContactId = Id
                                 });
                            }
                         }catch(Exception e){
                             foreach (decimal Id in ContactList.Select(k => k.ContactIds))
                             {
                                 Confirmation.Add(new ManageScheduleMessage
                                 {
                                     Status = "Error",
                                     MessageScheduleid = Schdule.MessageScheduleid,
                                     Send_At = Schdule.Send_At,
                                     ContactId = Id
                                 });
                             }
                         }
                         db.ManageScheduleMessages.AddRange(Confirmation);
                        // ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Email sent.');", true);
                        Schdule.Status = "Completed";
                    }
                  db.SaveChanges();
                }
            }catch(Exception e){
            }
        }
    }
}
