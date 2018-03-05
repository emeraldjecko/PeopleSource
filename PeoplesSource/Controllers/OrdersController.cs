using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using PeoplesSource.Data.Models;
using PeoplesSource.Providers;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Xml.Linq;
using System.Xml;
using PeoplesSource.Models;
using System.Text.RegularExpressions;
using System.Web.Configuration;

namespace PeoplesSource.Controllers
{
    [Authorize]
    [RoutePrefix("Orders")]
    public class OrdersController : Controller
    {
        // GET: Orders
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult TestView()
        {

            return View();
        }

        [HttpGet]
        [Route("GetOrders")]
        public ActionResult GetOrders()
        {
            List<OrderModel> orders = new List<OrderModel>();
            using (var db = new ProductEntities())
            {

                var Orders = db.Orders.Where(o => o.txn_id.Contains("RET") && string.IsNullOrEmpty(o.tracking) == false && o.item_sku.Contains("AAA") && o.TrackingClosedStatus == false).ToList();
                foreach (var o in Orders)
                {

                    if (!string.IsNullOrEmpty(o.tracking))
                    {
                        TrackResponse t = new TrackResponse();
                        if (!string.IsNullOrEmpty(o.TrackerXML))
                        {
                            XmlDocument xd = new XmlDocument();
                            xd.LoadXml(o.TrackerXML);
                          
                            foreach (XmlNode node in xd.LastChild)
                            {
                                t.Tracking = node.Attributes["ID"].Value.ToString();
                                t.TrackSummary = node.ChildNodes[0].InnerText.ToString();
                                for (int i = 1; i < node.ChildNodes.Count; i++)
                                {
                                    string trackingInfo = node.ChildNodes[i].InnerText.ToString();

                                    if (t.TrackDetail == null)
                                        t.TrackDetail = new List<TrackerDetailsResponseModel>() { };

                                    t.TrackDetail.Add(SplitStringArray(trackingInfo));
                                }
                            }

                            if (!string.IsNullOrWhiteSpace(t.TrackSummary) &&
                                t.TrackSummary.ToLower().Trim().Contains("your item has been delivered"))
                            {

                                try
                                {
                                    string UserEmail = WebConfigurationManager.AppSettings["ClientEmail"];
                                    string Password = WebConfigurationManager.AppSettings["Password"];
                                    MailMessage mail = new MailMessage();
                                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                                    mail.From = new MailAddress(UserEmail);
                                    mail.To.Add("thizeness@gmail.com");
                                    mail.Subject = "SyncTu Order Status Update";
                                    mail.Body = $"Date:{DateTime.Now} <br /> " +
                                                $"Tracking Number: {o.tracking} <br /> " +
                                                $"Buyer Email: {o.payer_email} <br /> " +
                                                $"Seller Id: {o.account} <br /> " +
                                                $"Buyer Name: {o.name} <br /> " +
                                                $"Buyer Address: {o.address_state} {o.address_street2} {o.address_city} {o.address_state} {o.address_country} {o.address_zip} <br /> " +
                                                $"Status: {o.status} <br /> ";

                                    SmtpServer.Port = 587;
                                    SmtpServer.Credentials = new System.Net.NetworkCredential(UserEmail, Password);
                                    SmtpServer.EnableSsl = true;
                                    SmtpServer.Send(mail);
                                }
                                catch (Exception)
                                {
                                    
                                }
                               
                            }
                        }

                        OrderModel _OrderModel = new OrderModel()
                        {
                            account = o.account,
                            txn_id = o.txn_id,
                            status = o.status,
                            date = Convert.ToDateTime(o.date).Date,
                            order_source = o.order_source,
                            Datetest = o.Datetest,
                            Id = o.Id,
                            address_city = o.address_city,
                            address_country = o.address_country,
                            address_state = o.address_state,
                            address_street = o.address_street,
                            address_street2 = o.address_street2,
                            address_zip = o.address_zip,
                            item_description = o.item_description,
                            item_name = o.item_name,
                            item_sku = o.item_sku,
                            name = o.name,
                            payer_email = o.payer_email,
                            TrackerXML = o.TrackerXML,
                            tracking = o.tracking,
                            trackResponse = t

                        };
                        orders.Add(_OrderModel);
                    }


                }


                return new JsonCamelCaseResult(new { orders = orders, status = "Success", discription = "" }, JsonRequestBehavior.AllowGet);
            }

        }
        public TrackerDetailsResponseModel SplitStringArray(string str)
        {
            string pattern = @"(?<=\D)[\,]";
            string substitution = @"-";
            List<string> FinalTrackingDetails = new List<string>();

            Regex regex = new Regex(pattern);
            string result = regex.Replace(str, substitution);
            List<string> Splitstring = result.Split('-').ToList();
            int i = 0;
            string currentValue = string.Empty;
            foreach (string value in Splitstring)
            {
                try
                {


                    DateTime date = Convert.ToDateTime(value);
                    FinalTrackingDetails.Add(currentValue);
                    FinalTrackingDetails.Add(value);
                    currentValue = string.Empty;


                }
                catch (Exception e)
                {
                    currentValue += value;
                }
            }
            if (!string.IsNullOrEmpty(currentValue))
                FinalTrackingDetails.Add(currentValue);

            TrackerDetailsResponseModel trackerDetailsResponseModel = new TrackerDetailsResponseModel() { };
            for (int s = 0; s < FinalTrackingDetails.Count; s++)
            {
                switch (s)
                {
                    case 0: trackerDetailsResponseModel.OrderStatus = FinalTrackingDetails[s]; break;
                    case 1: trackerDetailsResponseModel.OrderDate = FinalTrackingDetails[s]; break;
                    case 2: trackerDetailsResponseModel.OrderLocation = FinalTrackingDetails[2]; break;
                }

            }
            return trackerDetailsResponseModel;
        }
        [HttpPost]
        [Route("GetReturnTracking/{ordersId:string}")]
        public ActionResult GetReturnTracking(string ordersId)
        {
         
            if ( string.IsNullOrEmpty(ordersId))
                return new JsonCamelCaseResult(new { orders = ordersId, status = "Error", discription = "" }, JsonRequestBehavior.AllowGet);
            List<OrderModel> orders = new List<OrderModel>();
            List<string> ordersID = ordersId.Split(',').ToList<string>();
            using (var db = new ProductEntities())
            {
                var Orders = db.Orders.Where(o => o.txn_id.Contains("RET") && string.IsNullOrEmpty(o.tracking) == false && o.item_sku.Contains("AAA") && o.TrackingClosedStatus == false).ToList();
                foreach (var o in Orders)
                {

                    //&& string.IsNullOrEmpty(o.TrackerXML)
                    if (!string.IsNullOrEmpty(o.tracking))
                    {
                        TrackResponse t = new TrackResponse();
                        if (ordersID.Where(s =>Convert.ToInt32(s) == o.Id).ToList().Count > 0)
                            o.TrackerXML = GetTrackingDetails(o.tracking);
                        if (!string.IsNullOrEmpty(o.TrackerXML)) { 

                        XmlDocument xd = new XmlDocument();
                        xd.LoadXml(o.TrackerXML);

                        foreach (XmlNode node in xd.LastChild)
                        {
                            t.Tracking = node.Attributes["ID"].Value.ToString();
                            t.TrackSummary = node.ChildNodes[0].InnerText.ToString();
                            for (int i = 1; i < node.ChildNodes.Count; i++)
                            {
                                string trackingInfo = node.ChildNodes[i].InnerText.ToString();

                                if (t.TrackDetail == null)
                                    t.TrackDetail = new List<TrackerDetailsResponseModel>() { };

                                t.TrackDetail.Add(SplitStringArray(trackingInfo));
                            }
                        }

                    }
                        OrderModel _OrderModel = new OrderModel()
                        {
                            account = o.account,
                            txn_id = o.txn_id,
                            status = o.status,
                            date = Convert.ToDateTime(o.date).Date,
                            order_source = o.order_source,
                            Datetest = o.Datetest,
                            Id = o.Id,
                            address_city = o.address_city,
                            address_country = o.address_country,
                            address_state = o.address_state,
                            address_street = o.address_street,
                            address_street2 = o.address_street2,
                            address_zip = o.address_zip,
                            item_description = o.item_description,
                            item_name = o.item_name,
                            item_sku = o.item_sku,
                            name = o.name,
                            payer_email = o.payer_email,
                            TrackerXML = o.TrackerXML,
                            tracking = o.tracking,
                            trackResponse = t

                        };
                        orders.Add(_OrderModel);

                    }

                }

                db.SaveChanges();
                return new JsonCamelCaseResult(new { orders = orders, status = "Success", discription = "" }, JsonRequestBehavior.AllowGet);

            }
        }

        //[HttpPost]
        //[Route("GetReturnTracking")]
        //public ActionResult GetReturnTracking(List<OrderModel> _Orders)
        //{
        //    if (_Orders == null || _Orders.Count == 0)
        //        return new JsonCamelCaseResult(new { orders = _Orders, status = "Error", discription = "" }, JsonRequestBehavior.AllowGet);
        //    List<OrderModel> orders = new List<OrderModel>();
        //    using (var db = new ProductEntities())
        //    {
        //        var Orders = db.Orders.Where(o => o.txn_id.Contains("RET") && string.IsNullOrEmpty(o.tracking) == false && o.item_sku.Contains("AAA") && o.TrackingClosedStatus == false).ToList().OrderByDescending(s => s.date);
        //        foreach (var o in Orders)
        //        {

        //            //&& string.IsNullOrEmpty(o.TrackerXML)
        //            if (!string.IsNullOrEmpty(o.tracking))
        //            {
        //                TrackResponse t = new TrackResponse();
        //                if (_Orders.Where(s => s.Id == o.Id).ToList().Count > 0)
        //                    o.TrackerXML = GetTrackingDetails(o.tracking);
        //                if (!string.IsNullOrEmpty(o.TrackerXML))
        //                {

        //                    XmlDocument xd = new XmlDocument();
        //                    xd.LoadXml(o.TrackerXML);

        //                    foreach (XmlNode node in xd.LastChild)
        //                    {
        //                        t.Tracking = node.Attributes["ID"].Value.ToString();
        //                        t.TrackSummary = node.ChildNodes[0].InnerText.ToString();
        //                        for (int i = 1; i < node.ChildNodes.Count; i++)
        //                        {
        //                            string trackingInfo = node.ChildNodes[i].InnerText.ToString();

        //                            if (t.TrackDetail == null)
        //                                t.TrackDetail = new List<TrackerDetailsResponseModel>() { };

        //                            t.TrackDetail.Add(SplitStringArray(trackingInfo));
        //                        }
        //                    }

        //                }
        //                OrderModel _OrderModel = new OrderModel()
        //                {
        //                    account = o.account,
        //                    txn_id = o.txn_id,
        //                    status = o.status,
        //                    date = Convert.ToDateTime(o.date),
        //                    order_source = o.order_source,
        //                    Datetest = o.Datetest,
        //                    Id = o.Id,
        //                    address_city = o.address_city,
        //                    address_country = o.address_country,
        //                    address_state = o.address_state,
        //                    address_street = o.address_street,
        //                    address_street2 = o.address_street2,
        //                    address_zip = o.address_zip,
        //                    item_description = o.item_description,
        //                    item_name = o.item_name,
        //                    item_sku = o.item_sku,
        //                    name = o.name,
        //                    payer_email = o.payer_email,
        //                    TrackerXML = o.TrackerXML,
        //                    tracking = o.tracking,
        //                    trackResponse = t

        //                };
        //                orders.Add(_OrderModel);

        //            }

        //        }

        //        db.SaveChanges();
        //        return new JsonCamelCaseResult(new { orders = orders, status = "Success", discription = "" }, JsonRequestBehavior.AllowGet);

        //    }
        //}
        public string GetTrackingDetails(string TrackingId)
        {
            string UserId = ConfigurationManager.AppSettings["USPSUserName"].ToString();
            string Password = ConfigurationManager.AppSettings["USPSPassword"].ToString();
            string ApiUrl = ConfigurationManager.AppSettings["USPSApiUrl"].ToString();
            string CompleteUrl = string.Concat(ApiUrl, string.Format("&XML=<TrackRequest USERID=\"{0}\"><TrackID ID=\"{1}\"></TrackID></TrackRequest>", UserId, TrackingId));
            WebRequest req = WebRequest.Create(CompleteUrl);
            req.Method = "GET";
            req.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(string.Concat(UserId, ":", Password)));
            //req.Credentials = new NetworkCredential("username", "password");
            HttpWebResponse resp = req.GetResponse() as HttpWebResponse;
            Encoding enc = System.Text.Encoding.GetEncoding(1252);
            StreamReader loResponseStream = new
              StreamReader(resp.GetResponseStream(), enc);

            string Response = loResponseStream.ReadToEnd();

            loResponseStream.Close();
            resp.Close();
            return Response.ToString();


        }

        [HttpPost]
        [Route("ClosedTracking")]
        public ActionResult ClosedTracking(OrderModel _Order)
        {
            using (var db = new ProductEntities())
            {
                var Orders = db.Orders.Where(o => o.Id == _Order.Id).FirstOrDefault();
                if (Orders != null)
                {
                    Orders.TrackingClosedStatus = true;
                    db.SaveChanges();
                }

            }
            
            return new JsonCamelCaseResult(new {   status = "Success", discription = "" }, JsonRequestBehavior.AllowGet);
        }     
    }
}