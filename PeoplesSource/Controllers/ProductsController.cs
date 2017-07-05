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
        [Route("GetProducts/{realSKU:string,pSellerId:string,PName:string}")]
        public ActionResult GetProducts(string realSKU, string pSellerId, string PName, string profit1Operand, string profit1Value1, string profit1Value2, string ShippingCost)
        {
            realSKU= realSKU.Trim('\"');
            PName= PName.Trim('\"');
            pSellerId = pSellerId.Trim('\"');
            profit1Operand = profit1Operand.Trim('\"');
            profit1Value1 = profit1Value1.Trim('\"');
            profit1Value2 = profit1Value2.Trim('\"');
            ShippingCost = ShippingCost.Trim('\"');

            double shippingCostValue = 0;
            double.TryParse(ShippingCost, out shippingCostValue); 

            double profit1Value1Double = 0;
            double.TryParse(profit1Value1, out profit1Value1Double);

            double profit1Value2Double = 0;
            double.TryParse(profit1Value2, out profit1Value2Double); 

            if (string.IsNullOrEmpty(realSKU) && string.IsNullOrEmpty(PName))
            {
                return new JsonCamelCaseResult(new { status = "error", discription = "" }, JsonRequestBehavior.AllowGet);

            }
            ProductEntities entities = new ProductEntities();
            var items = new List<ProductModel>();

            if (string.IsNullOrEmpty(profit1Value1))
            {
                items = (from p in entities.Products
                             join r in entities.ReorderProducts on p.RealSKU equals r.SKU
                             join s in entities.SellerInfoes on p.SellerId equals s.Name
                             where (p.RealSKU.StartsWith(realSKU) || realSKU == null) && (p.Name.Contains(PName) || PName == null) && (p.SellerId.Contains(pSellerId) || pSellerId == null)
                             orderby p.SellerId
                             select new ProductModel
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
                                 stockDate = r.StockDate

                             }).ToList();
            }

            else if (!string.IsNullOrEmpty(profit1Operand) && !string.IsNullOrEmpty(profit1Value1))
            {
                if (profit1Operand == "=")
                {
                    items = (from p in entities.Products
                                 let firstProfit = p.PriceDefault - p.Cost - shippingCostValue - (p.PriceDefault * 0.07166666666) - 0.3 - (p.PriceDefault * 0.029)
                                 
                                 join r in entities.ReorderProducts on p.RealSKU equals r.SKU
                                 join s in entities.SellerInfoes on p.SellerId equals s.Name
                                 where (p.RealSKU.StartsWith(realSKU) || realSKU == null) && (p.Name.Contains(PName) || PName == null) && (p.SellerId.Contains(pSellerId) || pSellerId == null)
                                   && Math.Round(firstProfit.Value, 2) == profit1Value1Double
                                 orderby p.SellerId
                                 select new ProductModel
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
                                     firstProfitPrice = firstProfit

                                 }).ToList();

                }
                else if (profit1Operand == "<=")
                {
                    items = (from p in entities.Products
                             let firstProfit = p.PriceDefault - p.Cost - shippingCostValue - (p.PriceDefault * 0.07166666666) - 0.3 - (p.PriceDefault * 0.029)

                             join r in entities.ReorderProducts on p.RealSKU equals r.SKU
                             join s in entities.SellerInfoes on p.SellerId equals s.Name
                             where (p.RealSKU.StartsWith(realSKU) || realSKU == null) && (p.Name.Contains(PName) || PName == null) && (p.SellerId.Contains(pSellerId) || pSellerId == null)
                               && Math.Round(firstProfit.Value, 2) <= profit1Value1Double
                             orderby p.SellerId
                             select new ProductModel
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
                                 firstProfitPrice = firstProfit

                             }).ToList();
                }
                else if (profit1Operand == ">=")
                {
                    items = (from p in entities.Products
                             let firstProfit = p.PriceDefault - p.Cost - shippingCostValue - (p.PriceDefault * 0.07166666666) - 0.3 - (p.PriceDefault * 0.029)

                             join r in entities.ReorderProducts on p.RealSKU equals r.SKU
                             join s in entities.SellerInfoes on p.SellerId equals s.Name
                             where (p.RealSKU.StartsWith(realSKU) || realSKU == null) && (p.Name.Contains(PName) || PName == null) && (p.SellerId.Contains(pSellerId) || pSellerId == null)
                               && Math.Round(firstProfit.Value, 2) >= profit1Value1Double
                             orderby p.SellerId
                             select new ProductModel
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
                                 firstProfitPrice = firstProfit

                             }).ToList();
                }
                else if (profit1Operand == "<")
                {
                    items = (from p in entities.Products
                             let firstProfit = p.PriceDefault - p.Cost - shippingCostValue - (p.PriceDefault * 0.07166666666) - 0.3 - (p.PriceDefault * 0.029)

                             join r in entities.ReorderProducts on p.RealSKU equals r.SKU
                             join s in entities.SellerInfoes on p.SellerId equals s.Name
                             where (p.RealSKU.StartsWith(realSKU) || realSKU == null) && (p.Name.Contains(PName) || PName == null) && (p.SellerId.Contains(pSellerId) || pSellerId == null)
                               && Math.Round(firstProfit.Value, 2) < profit1Value1Double
                             orderby p.SellerId
                             select new ProductModel
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
                                 firstProfitPrice = firstProfit

                             }).ToList();
                }
                else if (profit1Operand == ">")
                {
                    items = (from p in entities.Products
                             let firstProfit = p.PriceDefault - p.Cost - shippingCostValue - (p.PriceDefault * 0.07166666666) - 0.3 - (p.PriceDefault * 0.029)

                             join r in entities.ReorderProducts on p.RealSKU equals r.SKU
                             join s in entities.SellerInfoes on p.SellerId equals s.Name
                             where (p.RealSKU.StartsWith(realSKU) || realSKU == null) && (p.Name.Contains(PName) || PName == null) && (p.SellerId.Contains(pSellerId) || pSellerId == null)
                               && Math.Round(firstProfit.Value, 2) >= profit1Value1Double
                             orderby p.SellerId
                             select new ProductModel
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
                                 firstProfitPrice = firstProfit

                             }).ToList();
                }
                else if (profit1Operand == "<>")
                {
                    items = (from p in entities.Products
                             let firstProfit = p.PriceDefault - p.Cost - shippingCostValue - (p.PriceDefault * 0.07166666666) - 0.3 - (p.PriceDefault * 0.029)

                             join r in entities.ReorderProducts on p.RealSKU equals r.SKU
                             join s in entities.SellerInfoes on p.SellerId equals s.Name
                             where (p.RealSKU.StartsWith(realSKU) || realSKU == null) && (p.Name.Contains(PName) || PName == null) && (p.SellerId.Contains(pSellerId) || pSellerId == null)
                               && Math.Round(firstProfit.Value, 2) != profit1Value1Double
                             orderby p.SellerId
                             select new ProductModel
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
                                 firstProfitPrice = firstProfit

                             }).ToList();
                }
                else if (profit1Operand == "between")
                {
                    items = (from p in entities.Products
                             let firstProfit = p.PriceDefault - p.Cost - shippingCostValue - (p.PriceDefault * 0.07166666666) - 0.3 - (p.PriceDefault * 0.029)

                             join r in entities.ReorderProducts on p.RealSKU equals r.SKU
                             join s in entities.SellerInfoes on p.SellerId equals s.Name
                             where (p.RealSKU.StartsWith(realSKU) || realSKU == null) && (p.Name.Contains(PName) || PName == null) && (p.SellerId.Contains(pSellerId) || pSellerId == null)
                               && (Math.Round(firstProfit.Value, 2) >= profit1Value1Double  && Math.Round(firstProfit.Value, 2) <= profit1Value2Double)
                             orderby p.SellerId
                             select new ProductModel
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
                                 firstProfitPrice = firstProfit

                             }).ToList();
                }
            }


            //if (!string.IsNullOrEmpty(profit1Operand) && !string.IsNullOrEmpty(profit1Value1))
            //{
            //    if (profit1Operand == "=")
            //    {
            //        items = items.Where(p => Math.Round(Convert.ToDouble(p.firstProfitPrice.Value), 2) == profit1Value1Double).ToList();
            //    }
            //    else if (profit1Operand == "<=")
            //    {
            //        items = items.Where(p => Math.Round(Convert.ToDouble(p.firstProfitPrice.Value), 2) <= profit1Value1Double).ToList();
            //    }
            //    else if (profit1Operand == ">=")
            //    {
            //        items = items.Where(p => Math.Round(Convert.ToDouble(p.firstProfitPrice.Value), 2) >= profit1Value1Double).ToList();
            //    }
            //    else if (profit1Operand == "<")
            //    {
            //        items = items.Where(p => Math.Round(Convert.ToDouble(p.firstProfitPrice.Value), 2) < profit1Value1Double).ToList();
            //    }
            //    else if (profit1Operand == ">")
            //    {
            //        items = items.Where(p => Math.Round(Convert.ToDouble(p.firstProfitPrice.Value), 2) > profit1Value1Double).ToList();
            //    }
            //    else if (profit1Operand == "<>")
            //    {
            //        items = items.Where(p => Math.Round(Convert.ToDouble(p.firstProfitPrice.Value), 2) != profit1Value1Double).ToList();
            //    }
            //    else if (profit1Operand == "between")
            //    {
            //        items = items.Where(p => Math.Round(Convert.ToDouble(p.firstProfitPrice.Value), 2) >= profit1Value1Double && Math.Round(Convert.ToDouble(p.firstProfitPrice.Value), 2) <= profit1Value2Double).ToList();
            //    }
            //}

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