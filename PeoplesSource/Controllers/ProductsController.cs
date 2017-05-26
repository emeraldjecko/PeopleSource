using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PeoplesSource.Data;
using PeoplesSource.Data.Models;
using PeoplesSource.Providers;
using PeoplesSource.Models;
using System.Net;
using System.Xml.Linq;
using System.IO;
using System.Net.Http;
using System.Text;

namespace PeoplesSource.Controllers
{
    [Authorize]
    [RoutePrefix("Products")]
    public class ProductsController : Controller
    {
        // GET: Products
        public ActionResult Index()
        {
            return View();

        }
        public ActionResult NetoProduct()
        {


            return View();
        }
        [HttpGet]
        [Route("InDepth")]
        public ActionResult NetoProductInDepth(string realSku, DateTime date)
        {
            ViewBag.realSku = realSku;
            ViewBag.date = date;

            return View();
        }
        [HttpGet]
        [Route("GetProducts/{realSKU:string,PName:string}")]
        public ActionResult GetProducts(string realSKU=null,string PName=null)
        {
            realSKU= realSKU.Trim('\"');
            PName= PName.Trim('\"');
            if (string.IsNullOrEmpty(realSKU) && string.IsNullOrEmpty(PName))
            {
                return new JsonCamelCaseResult(new { status = "error", discription = "" }, JsonRequestBehavior.AllowGet);

            }
            ProductEntities entities = new ProductEntities();
            
            var items = (from p in entities.Products
                         from r in entities.ReorderProducts
                         .Where(rp => p.RealSKU == rp.SKU)
                         .DefaultIfEmpty()
                         join s in entities.SellerInfoes on p.SellerId equals s.Name
                         where (p.RealSKU.StartsWith(realSKU) || realSKU == null) && ( p.Name.Contains(PName) || PName == null)
                         orderby p.SellerId
                         select new 
                         {
                             Name = p.Name,
                             SKU = p.SKU,
                             SellerId = p.SellerId,
                             Increment = s.Increment,
                             IsPercentage = s.IsPercentage,
                             KZ = s.KZ,
                             OHT = s.OHT,
                             PriceDefault = p.PriceDefault,
                             RealSKU = p.RealSKU,
                             Cost = p.Cost,
                             eBayItemID=p.eBayItemID,
                             daily30 = r.DailyUnitsSoldRateForPast30Days != null ? r.DailyUnitsSoldRateForPast30Days : 0,
                             total30 = r.TotalNumberOfUnitsSoldInPast30Days != null ? r.TotalNumberOfUnitsSoldInPast30Days : 0,
                             dailyRestock = r.DailyUnitsSoldRateFromLastRestockToLastSaleDate != null ? r.DailyUnitsSoldRateFromLastRestockToLastSaleDate : 0,
                             totalRestock = r.TotalNumberOfUnitsSoldBetweenLastReStockAndLastSaleDate != null ? r.TotalNumberOfUnitsSoldBetweenLastReStockAndLastSaleDate : 0,
                             stockDate = r.StockDate

                         }).ToList();

            return new JsonCamelCaseResult(new { products = items, status = "Success", discription = "" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("UpdateProducts")]
        public ActionResult UpdateProducts(List<ItemViewModel> items)
        {

            string destinationUrl = "https://toast.mylistandsend.com/do/WS/NetoAPI";

            Uri myUri = new Uri(destinationUrl, UriKind.Absolute);


            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(myUri);

            XDocument doc = new XDocument();

            var ui = new XElement("UpdateItem");

            var xItems = from i in items
                         select new XElement("Item",
                         new XElement("SKU", i.SKU), new XElement("DefaultPrice", i.UpdatedPrice));

            ui.Add(xItems);

            doc.Add(ui);

            var xml = doc.ToString();

            byte[] bytes;
            bytes = System.Text.Encoding.ASCII.GetBytes(xml);
            request.ContentType = "appplication/xml";
            request.Headers.Add("NETOAPI_ACTION", "UpdateItem");
            //request.Headers.Add("NETOAPI_KEY", "9dkPGdD44iwyy0xnnkv6dBHs0yZTNpnn");
            request.Headers.Add("NETOAPI_KEY", "iKtwjsGmZX0x1ULKAXv1BFZbGBrAQK2D");
            request.Headers.Add("NETOAPI_USERNAME", "DevDev02");
            request.ContentLength = bytes.Length;
            request.Method = "POST";

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            HttpWebResponse response;
            response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream responseStream = response.GetResponseStream();
                string responseStr = new StreamReader(responseStream).ReadToEnd();


                XDocument xdoc = null;
                xdoc = XDocument.Parse(responseStr);
                var requestStatus = xdoc.Descendants("UpdateItemResponse").Elements("Ack").FirstOrDefault().Value;
                if (requestStatus != "Success")
                {
                    //var errorMessage = xdoc.Descendants("Message").FirstOrDefault().Value;
                    var errorMessages = xdoc.Descendants("Message").ToList();
                    string allErrors = "";
                    foreach (var item in errorMessages)
                    {
                        allErrors += "\n" + item.Value.ToString();
                    }
                    return new JsonCamelCaseResult(new { status = requestStatus, discription = allErrors }, JsonRequestBehavior.AllowGet);

                }

                return new JsonCamelCaseResult(new { status = requestStatus, discription = "Successfully Updated the prices." }, JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        [HttpPost]
        [Route("UpdateProducts")]
        public ActionResult UpdateNetoProducts(List<ItemViewModel> items)
        {

            string destinationUrl = "https://toast.mylistandsend.com/do/WS/NetoAPI";

            Uri myUri = new Uri(destinationUrl, UriKind.Absolute);


            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(myUri);

            XDocument doc = new XDocument();

            var ui = new XElement("UpdateItem");

            var xItems = from i in items
                         select new XElement("Item",
                         new XElement("SKU", i.netoSKU), new XElement("DefaultPrice", i.Price2));

            ui.Add(xItems);

            doc.Add(ui);

            var xml = doc.ToString();

            byte[] bytes;
            bytes = System.Text.Encoding.ASCII.GetBytes(xml);
            request.ContentType = "appplication/xml";
            request.Headers.Add("NETOAPI_ACTION", "UpdateItem");
            //request.Headers.Add("NETOAPI_KEY", "9dkPGdD44iwyy0xnnkv6dBHs0yZTNpnn");
            request.Headers.Add("NETOAPI_KEY", "iKtwjsGmZX0x1ULKAXv1BFZbGBrAQK2D");
            request.Headers.Add("NETOAPI_USERNAME", "DevDev02");
            request.ContentLength = bytes.Length;
            request.Method = "POST";

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            HttpWebResponse response;
            response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream responseStream = response.GetResponseStream();
                string responseStr = new StreamReader(responseStream).ReadToEnd();


                XDocument xdoc = null;
                xdoc = XDocument.Parse(responseStr);
                var requestStatus = xdoc.Descendants("UpdateItemResponse").Elements("Ack").FirstOrDefault().Value;
                if (requestStatus != "Success")
                {
                    //var errorMessage = xdoc.Descendants("Message").FirstOrDefault().Value;
                    var errorMessages = xdoc.Descendants("Message").ToList();
                    string allErrors = "";
                    foreach (var item in errorMessages)
                    {
                        allErrors += "\n" + item.Value.ToString();
                    }
                    return new JsonCamelCaseResult(new { status = requestStatus, discription = allErrors }, JsonRequestBehavior.AllowGet);

                }

                return new JsonCamelCaseResult(new { status = requestStatus, discription = "Successfully Updated the prices." }, JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        [HttpGet]
        [Route("GetNetoProducts/{date:DateTime?}")]
        public ActionResult GetNetoProducts(DateTime? date = null)
        {
            //if (date=)
            //{
            //    return new JsonCamelCaseResult(new { status = "error", discription = "" }, JsonRequestBehavior.AllowGet);

            //}
            ProductEntities entities = new ProductEntities();
            //    var query = entities.NetoProducts
            //.GroupBy(c => c.ItemNumber)
            //.Select(g => g.OrderByDescending(c => c.Date).First())
            //.Select(p => new
            //{
            //    Title = p.Title,
            //    NetoSKU = p.NetoSKU,
            //    SellerId = p.SellerId,
            //    Position = p.Position,
            //    ItemNumber = p.ItemNumber,
            //    Date = p.Date,
            //    PriceDefault = p.Price,
            //    RealSKU = p.RealSKU,
            //    Cost = p.Cost,
            //    TotalListings = p.TotalListings,
            //    ProductID = p.ProductID



            //}).ToList();
            if (date == null)
                date = (from p in entities.NetoProducts select new { Date = p.Date }).OrderByDescending(t => t.Date).FirstOrDefault().Date;

            var items = (from p in entities.NetoProducts
                         where p.Date == date || date == null
                         orderby p.SellerId
                         select new
                         {
                             Title = p.Title,
                             NetoSKU = p.NetoSKU,
                             SellerId = p.SellerId,
                             Position = p.Position,
                             ItemNumber = p.ItemNumber,
                             Date = p.Date,
                             PriceDefault = p.Price,
                             RealSKU = p.RealSKU,
                             Cost = p.Cost,
                             TotalListings = p.TotalListings,
                             ProductID = p.ProductID



                         }).OrderByDescending(t => t.Date).ToList();

            return new JsonCamelCaseResult(new { netoproducts = items, status = "Success", discription = "" }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        [Route("GetNetoInDepthProducts/{realSKUstring,date:DateTime?,section:string}")]
        public ActionResult GetNetoInDepthProducts(string realSKU = null, DateTime? date = null,string section =null)
        {
            //if (date=)
            //{
            //    return new JsonCamelCaseResult(new { status = "error", discription = "" }, JsonRequestBehavior.AllowGet);

            //}
            ProductEntities entities = new ProductEntities();


            if (section == "Section1")
            {
                var items = (from p in entities.NetoProducts
                         where (p.Date == date || date == null) && (p.RealSKU.Contains(realSKU.Substring(0, 29)) || string.IsNullOrEmpty(realSKU))
                         orderby p.SellerId
                         select new
                         {
                             Title = p.Title,
                             NetoSKU = p.NetoSKU,
                             SellerId = p.SellerId,
                             Position = p.Position,
                             ItemNumber = p.ItemNumber,
                             Date = p.Date,
                             PriceDefault = p.Price,
                             Price2 = p.Price2,
                             RealSKU = p.RealSKU,
                             Cost = p.Cost,
                             TotalListings = p.TotalListings,
                             ProductID = p.ProductID,
                             Shipping = p.shipping,
                             Sold = p.sold



                         }).OrderByDescending(t => t.Date).ToList();
                return new JsonCamelCaseResult(new { netoproducts = items, status = "Success", discription = "" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
              var  items = (from p in entities.BestMatchPositions
                            where (p.Date == date || date == null) && (p.RealSKU.Contains(realSKU.Substring(0, 29)) || string.IsNullOrEmpty(realSKU))                 
                         select new
                         {
                             Title = p.Title,
                             SellerId = p.SellerId,
                             Position = p.Position,
                             ItemNumber = p.ItemNumber,
                             Date = p.Date,
                             PriceDefault = p.Price,
                             Price2 = p.Price2,
                             RealSKU = p.RealSKU,
                             TotalListings = p.TotalListings,
                             Shipping = p.shipping,
                             Sold = p.sold
                         }).ToList();
                return new JsonCamelCaseResult(new { netoproducts = items, status = "Success", discription = "" }, JsonRequestBehavior.AllowGet);
            }
         

        }


    }
}