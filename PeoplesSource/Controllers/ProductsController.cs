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


        public ActionResult AutoPrice()
        {
            return View();

        }

        public ActionResult NetoProduct()
        {
            return View();
        }

        private int ConvertNullableInt(vw_ReorderProducts tmpR)
        {
            
            return 1;
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
        [Route("GetProducts/{realSKU:string,PName:string,SellerId:string, OperandProfit1:string, ValueProfit1:string, ShippingCost:string, OperandSRP30:string, ValueSRP30:string, OperandTUS30:string, ValueTUS30:string, OperandSRRS:string, ValueSRRS:string, Profit1Percent:string, Profit2Percent:string, OperandTURS:string, ValueTURS:string}")]
        public ActionResult GetProducts(string realSKU = null, string PName = null, string SellerId = null, string OperandProfit1 = null, string ValueProfit1 = null, string ShippingCost = null, string OperandSRP30 = null, string ValueSRP30 = null, string OperandTUS30 = null, string ValueTUS30 = null, string OperandSRRS = null, string ValueSRRS = null, string Profit1Percent = null, string Profit2Percent = null, string OperandTURS = null, string ValueTURS = null)
        {
            realSKU= realSKU.Trim('\"');
            PName= PName.Trim('\"');
            SellerId = SellerId.Trim('\"');
            OperandProfit1 = OperandProfit1.Trim('\"');
            ValueProfit1 = ValueProfit1.Trim('\"');
            ShippingCost = ShippingCost.Trim('\"');
            OperandSRP30 = OperandSRP30.Trim('\"');
            ValueSRP30 = ValueSRP30.Trim('\"');
            OperandTUS30 = OperandTUS30.Trim('\"');
            ValueTUS30 = ValueTUS30.Trim('\"');
            OperandSRRS = OperandSRRS.Trim('\"');
            ValueSRRS = ValueSRRS.Trim('\"');
            OperandTURS = OperandTURS.Trim('\"');
            ValueTURS = ValueTURS.Trim('\"');
            Profit1Percent = Profit1Percent.Trim('\"');
            Profit2Percent = Profit2Percent.Trim('\"');

            double shippingCostValue = 0;
            double.TryParse(ShippingCost, out shippingCostValue);

            double valueProfit1Double = 0;
            double.TryParse(ValueProfit1, out valueProfit1Double);

            double valueSRP30Double = 0;
            double.TryParse(ValueSRP30, out valueSRP30Double);

            double valueTUS30Double = 0;
            double.TryParse(ValueTUS30, out valueTUS30Double);

            double valueSRRSDouble = 0;
            double.TryParse(ValueSRRS, out valueSRRSDouble);

            int valueTURSInt = 0;
            int.TryParse(ValueTURS, out valueTURSInt);

            float valueProfit1Percent = 0;
            float.TryParse(Profit1Percent, out valueProfit1Percent);


            if (string.IsNullOrEmpty(realSKU) && string.IsNullOrEmpty(PName) && string.IsNullOrEmpty(SellerId) && string.IsNullOrEmpty(OperandProfit1) && string.IsNullOrEmpty(ValueProfit1) && string.IsNullOrEmpty(OperandSRP30) && string.IsNullOrEmpty(ValueSRP30) && string.IsNullOrEmpty(OperandTUS30) && string.IsNullOrEmpty(ValueTUS30) && string.IsNullOrEmpty(OperandSRRS) && string.IsNullOrEmpty(ValueSRRS) && string.IsNullOrEmpty(OperandTURS) && string.IsNullOrEmpty(ValueTURS))
            {
                return new JsonCamelCaseResult(new { status = "error", discription = "" }, JsonRequestBehavior.AllowGet);

            }
           
            ProductEntities entities = new ProductEntities();
            var items = (from p in entities.Products
                         from r in entities.ReorderProducts
                         .Where(rp => p.RealSKU == rp.SKU)
                         .DefaultIfEmpty()
                         join s in entities.SellerInfoes on p.SellerId equals s.Name
                         where (!string.IsNullOrEmpty(p.RealSKU)) && (p.Cost != null) && (p.RealSKU.Contains(realSKU) || realSKU == null) && (p.Name.Contains(PName) || PName == null) && (p.SellerId.Contains(SellerId) || SellerId == null)
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
                             eBayItemID = p.eBayItemID,
                             daily30 = r.DailyUnitsSoldRateForPast30Days != null ? r.DailyUnitsSoldRateForPast30Days : 0,
                             total30 = r.TotalNumberOfUnitsSoldInPast30Days != null ? r.TotalNumberOfUnitsSoldInPast30Days : 0,
                             dailyRestock = r.DailyUnitsSoldRateFromLastRestockToLastSaleDate != null ? r.DailyUnitsSoldRateFromLastRestockToLastSaleDate : 0,
                             totalRestock = r.TotalNumberOfUnitsSoldBetweenLastReStockAndLastSaleDate != null ? r.TotalNumberOfUnitsSoldBetweenLastReStockAndLastSaleDate : 0,
                             stockDate = r.StockDate,
                             qty = r.Quantity != null ? r.Quantity : 0,
                             firstProfitPrice = valueProfit1Percent > 0 ? (p.PriceDefault - p.Cost - shippingCostValue - (p.PriceDefault * 0.0915) - 0.3 - (p.PriceDefault * 0.029) - (p.PriceDefault * (valueProfit1Percent / 100))) : (p.PriceDefault - p.Cost - shippingCostValue - (p.PriceDefault * 0.0915) - 0.3 - (p.PriceDefault * 0.029))


                         }).ToList();

            if (!string.IsNullOrEmpty(OperandProfit1) && !string.IsNullOrEmpty(ValueProfit1))
            {
                if (OperandProfit1 == "=")
                {
                    items = items.Where(p => Math.Round(Convert.ToDouble(p.firstProfitPrice.Value), 2) == valueProfit1Double).ToList();
                }
                else if (OperandProfit1 == "<=")
                {
                    items = items.Where(p => Math.Round(Convert.ToDouble(p.firstProfitPrice.Value), 2) <= valueProfit1Double).ToList();
                }
                else if (OperandProfit1 == ">=")
                {
                    items = items.Where(p => Math.Round(Convert.ToDouble(p.firstProfitPrice.Value), 2) >= valueProfit1Double).ToList();
                }
                else if (OperandProfit1 == "<")
                {
                    items = items.Where(p => Math.Round(Convert.ToDouble(p.firstProfitPrice.Value), 2) < valueProfit1Double).ToList();
                }
                else if (OperandProfit1 == ">")
                {
                    items = items.Where(p => Math.Round(Convert.ToDouble(p.firstProfitPrice.Value), 2) > valueProfit1Double).ToList();
                }
                else if (OperandProfit1 == "<>")
                {
                    items = items.Where(p => Math.Round(Convert.ToDouble(p.firstProfitPrice.Value), 2) != valueProfit1Double).ToList();
                }
            }

            if (!string.IsNullOrEmpty(OperandSRP30) && !string.IsNullOrEmpty(ValueSRP30))
            {
                if (OperandSRP30 == "=")
                {
                    items = items.Where(p => Math.Round(Convert.ToDouble(p.daily30), 2) == valueSRP30Double).ToList();
                }
                else if (OperandSRP30 == "<=")
                {
                    items = items.Where(p => Math.Round(Convert.ToDouble(p.daily30), 2) <= valueSRP30Double).ToList();
                }
                else if (OperandSRP30 == ">=")
                {
                    items = items.Where(p => Math.Round(Convert.ToDouble(p.daily30), 2) >= valueSRP30Double).ToList();
                }
                else if (OperandSRP30 == "<")
                {
                    items = items.Where(p => Math.Round(Convert.ToDouble(p.daily30), 2) < valueSRP30Double).ToList();
                }
                else if (OperandSRP30 == ">")
                {
                    items = items.Where(p => Math.Round(Convert.ToDouble(p.daily30), 2) > valueSRP30Double).ToList();
                }
                else if (OperandSRP30 == "<>")
                {
                    items = items.Where(p => Math.Round(Convert.ToDouble(p.daily30), 2) != valueSRP30Double).ToList();
                }
            }

            if (!string.IsNullOrEmpty(OperandTUS30) && !string.IsNullOrEmpty(ValueTUS30))
            {
                if (OperandTUS30 == "=")
                {
                    items = items.Where(p => Math.Round(Convert.ToDouble(p.total30), 2) == valueTUS30Double).ToList();
                }
                else if (OperandTUS30 == "<=")
                {
                    items = items.Where(p => Math.Round(Convert.ToDouble(p.total30), 2) <= valueTUS30Double).ToList();
                }
                else if (OperandTUS30 == ">=")
                {
                    items = items.Where(p => Math.Round(Convert.ToDouble(p.total30), 2) >= valueTUS30Double).ToList();
                }
                else if (OperandTUS30 == "<")
                {
                    items = items.Where(p => Math.Round(Convert.ToDouble(p.total30), 2) < valueTUS30Double).ToList();
                }
                else if (OperandTUS30 == ">")
                {
                    items = items.Where(p => Math.Round(Convert.ToDouble(p.total30), 2) > valueTUS30Double).ToList();
                }
                else if (OperandTUS30 == "<>")
                {
                    items = items.Where(p => Math.Round(Convert.ToDouble(p.total30), 2) != valueTUS30Double).ToList();
                }
            }

            if (!string.IsNullOrEmpty(OperandSRRS) && !string.IsNullOrEmpty(ValueSRRS))
            {
                if (OperandSRRS == "=")
                {
                    items = items.Where(p => Math.Round(Convert.ToDouble(p.dailyRestock.Value), 2) == valueSRRSDouble).ToList();
                }
                else if (OperandSRRS == "<=")
                {
                    items = items.Where(p => Math.Round(Convert.ToDouble(p.dailyRestock.Value), 2) <= valueSRRSDouble).ToList();
                }
                else if (OperandSRRS == ">=")
                {
                    items = items.Where(p => Math.Round(Convert.ToDouble(p.dailyRestock.Value), 2) >= valueSRRSDouble).ToList();
                }
                else if (OperandSRRS == "<")
                {
                    items = items.Where(p => Math.Round(Convert.ToDouble(p.dailyRestock.Value), 2) < valueSRRSDouble).ToList();
                }
                else if (OperandSRRS == ">")
                {
                    items = items.Where(p => Math.Round(Convert.ToDouble(p.dailyRestock.Value), 2) > valueSRRSDouble).ToList();
                }
                else if (OperandSRRS == "<>")
                {
                    items = items.Where(p => Math.Round(Convert.ToDouble(p.dailyRestock.Value), 2) != valueSRRSDouble).ToList();
                }
            }

            if (!string.IsNullOrEmpty(OperandTURS) && !string.IsNullOrEmpty(ValueTURS))
            {
                if (OperandTURS == "=")
                {
                    items = items.Where(p => Convert.ToInt32(p.total30) == valueTURSInt).ToList();
                }
                else if (OperandTURS == "<=")
                {
                    items = items.Where(p => Convert.ToInt32(p.total30) <= valueTURSInt).ToList();
                }
                else if (OperandTURS == ">=")
                {
                    items = items.Where(p => Convert.ToInt32(p.total30) >= valueTURSInt).ToList();
                }
                else if (OperandTURS == "<")
                {
                    items = items.Where(p => Convert.ToInt32(p.total30) < valueTURSInt).ToList();
                }
                else if (OperandTURS == ">")
                {
                    items = items.Where(p => Convert.ToInt32(p.total30) > valueTURSInt).ToList();
                }
                else if (OperandTURS == "<>")
                {
                    items = items.Where(p => Convert.ToInt32(p.total30) != valueTURSInt).ToList();
                }
            }
           
            return new JsonCamelCaseResult(new { products = items, status = "Success", discription = "" }, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        [Route("UpdateProducts")]
        public ActionResult UpdateProducts(List<ItemViewModel> items)
        {

            string destinationUrl = "https://toast.mylistandsend.com/do/WS/NetoAPI";

            Uri myUri = new Uri(destinationUrl, UriKind.Absolute);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
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
            //request.Headers.Add("NETOAPI_KEY", "iKtwjsGmZX0x1ULKAXv1BFZbGBrAQK2D");
            request.Headers.Add("NETOAPI_KEY", "g0tTIaEWLVLNm8rzAa2b62TUX3c1A9MO");
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

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
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
            //request.Headers.Add("NETOAPI_KEY", "iKtwjsGmZX0x1ULKAXv1BFZbGBrAQK2D");
            request.Headers.Add("NETOAPI_KEY", "g0tTIaEWLVLNm8rzAa2b62TUX3c1A9MO");
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
                         where (p.Date == date || date == null) && (p.RealSKU.Contains(realSKU.Substring(0, 32)) || string.IsNullOrEmpty(realSKU))
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
                            where (p.Date == date || date == null) && (p.RealSKU.Contains(realSKU.Substring(0, 32)) || string.IsNullOrEmpty(realSKU))                 
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