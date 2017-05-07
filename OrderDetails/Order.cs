﻿using PeoplesSource.Data;
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
using System.Globalization;
using System.Net.Mail;

namespace OrderDetails
{
    class Order
    {
        static void Main(string[] args)
        {
            GetData();
        }

        private static void GetData()
        {
            var Msg = new EbayMessage();
            string error = "";
            GetOrdersResponse view = null;
            PeopleSourceEntities db = new PeopleSourceEntities();
            var sellerList = db.Sellers.ToList();
            foreach (var seller in sellerList)
            {

                //string FromDate = DateTime.UtcNow.AddDays(-30).ToString("o");
                //string ToDate = DateTime.UtcNow.ToString("o");
                //string strReq = @"<?xml version=""1.0"" encoding=""utf-8""?>"+
                //                    "<GetOrdersRequest xmlns=\"urn:ebay:apis:eBLBaseComponents\">"+
                //                        "<RequesterCredentials>"+
                //                            "<eBayAuthToken>"+seller.UserToken+"</eBayAuthToken>"+
                //                        "</RequesterCredentials>"+
                //                                "<CreateTimeFrom>"+FromDate+"</CreateTimeFrom>"+
                //                                "<CreateTimeTo>"+ToDate+"</CreateTimeTo>"+
                //                        "<OrderRole>Seller</OrderRole>"+
                //                        "<OrderStatus>Completed</OrderStatus>"+
                //                        "<DetailLevel>ReturnAll</DetailLevel>"+
                //                     "</GetOrdersRequest>";

                

                String rawXml = @"<?xml version=""1.0"" encoding=""UTF-8""?>"+
                "<GetOrdersResponse xmlns=\"urn:ebay:apis:eBLBaseComponents\">"+
                "<Timestamp>2015-08-31T04:48:38.811Z</Timestamp><Ack>Success</Ack><Version>937</Version><Build>E937_CORE_APIXO_17655486_R1</Build><PaginationResult><TotalNumberOfPages>1</TotalNumberOfPages><TotalNumberOfEntries>5</TotalNumberOfEntries></PaginationResult><HasMoreOrders>false</HasMoreOrders><OrderArray><Order><OrderID>198024019019</OrderID><OrderStatus>Completed</OrderStatus><AdjustmentAmount currencyID='USD'>0.0</AdjustmentAmount><AmountPaid currencyID='USD'>30.89</AmountPaid><AmountSaved currencyID='USD'>0.0</AmountSaved><CheckoutStatus><eBayPaymentStatus>NoPaymentFailure</eBayPaymentStatus><LastModifiedTime>2015-08-13T01:11:58.000Z</LastModifiedTime><PaymentMethod>PayPal</PaymentMethod><Status>Complete</Status><IntegratedMerchantCreditCardEnabled>false</IntegratedMerchantCreditCardEnabled></CheckoutStatus><ShippingDetails><InsuranceFee currencyID='USD'>0.0</InsuranceFee><InsuranceOption>NotOffered</InsuranceOption><InsuranceWanted>false</InsuranceWanted><SalesTax><SalesTaxPercent>0.0</SalesTaxPercent><SalesTaxState /><ShippingIncludedInTax>false</ShippingIncludedInTax><SalesTaxAmount currencyID='USD'>0.0</SalesTaxAmount></SalesTax><ShippingServiceOptions><ShippingService>ShippingMethodStandard</ShippingService><ShippingServiceCost currencyID='USD'>0.0</ShippingServiceCost><ShippingServicePriority>1</ShippingServicePriority><ExpeditedService>false</ExpeditedService><ShippingTimeMin>1</ShippingTimeMin><ShippingTimeMax>6</ShippingTimeMax></ShippingServiceOptions><SellingManagerSalesRecordNumber>2016</SellingManagerSalesRecordNumber><GetItFast>false</GetItFast></ShippingDetails><CreatingUserRole>Buyer</CreatingUserRole><CreatedTime>2015-08-08T02:00:28.000Z</CreatedTime><PaymentMethods>PayPal</PaymentMethods><SellerEmail>stetsonfowler72@gmail.com</SellerEmail><ShippingAddress><Name>Tuan Pham</Name><Street1>325 English Oak Ct</Street1><Street2 /><CityName>Orlando</CityName><StateOrProvince>FL</StateOrProvince><Country>US</Country><CountryName>United States</CountryName><Phone>407 925 5671</Phone><PostalCode>32807-5079</PostalCode><AddressID>1403038667021</AddressID><AddressOwner>eBay</AddressOwner><ExternalAddressID /></ShippingAddress><ShippingServiceSelected><ShippingInsuranceCost currencyID='USD'>0.0</ShippingInsuranceCost><ShippingService>ShippingMethodStandard</ShippingService><ShippingServiceCost currencyID='USD'>0.0</ShippingServiceCost></ShippingServiceSelected><Subtotal currencyID='USD'>30.89</Subtotal><Total currencyID='USD'>30.89</Total><ExternalTransaction><ExternalTransactionID>503062804Y3847803</ExternalTransactionID><ExternalTransactionTime>2015-08-08T02:00:24.000Z</ExternalTransactionTime><FeeOrCreditAmount currencyID='USD'>1.2</FeeOrCreditAmount><PaymentOrRefundAmount currencyID='USD'>30.89</PaymentOrRefundAmount></ExternalTransaction><DigitalDelivery>false</DigitalDelivery><TransactionArray><Transaction><Buyer><Email>Invalid Request</Email><UserFirstName /><UserLastName /></Buyer><ShippingDetails><SellingManagerSalesRecordNumber>2015</SellingManagerSalesRecordNumber><ShipmentTrackingDetails><ShippingCarrierUsed>USPS</ShippingCarrierUsed><ShipmentTrackingNumber>9400110200793659412210</ShipmentTrackingNumber></ShipmentTrackingDetails></ShippingDetails><CreatedDate>2015-08-08T02:00:28.000Z</CreatedDate><Item><ApplicationData>288-67161</ApplicationData><ItemID>261838572710</ItemID><SKU>55599865898</SKU></Item><QuantityPurchased>1</QuantityPurchased><Status><PaymentHoldStatus>None</PaymentHoldStatus></Status><TransactionID>1668081313016</TransactionID><TransactionPrice currencyID='USD'>15.27</TransactionPrice><TransactionSiteID>US</TransactionSiteID><ActualShippingCost currencyID='USD'>0.0</ActualShippingCost><ActualHandlingCost currencyID='USD'>0.0</ActualHandlingCost></Transaction><Transaction><Buyer><Email>Invalid Request</Email><UserFirstName /><UserLastName /></Buyer><ShippingDetails><SellingManagerSalesRecordNumber>2014</SellingManagerSalesRecordNumber><ShipmentTrackingDetails><ShippingCarrierUsed>USPS</ShippingCarrierUsed><ShipmentTrackingNumber>9400110200793659412210</ShipmentTrackingNumber></ShipmentTrackingDetails></ShippingDetails><CreatedDate>2015-08-08T02:00:27.000Z</CreatedDate><Item><ApplicationData>286-67162</ApplicationData><ItemID>261838572932</ItemID><SKU>58965742685</SKU></Item><QuantityPurchased>1</QuantityPurchased><Status><PaymentHoldStatus>None</PaymentHoldStatus></Status><TransactionID>1668081311016</TransactionID><TransactionPrice currencyID='USD'>15.62</TransactionPrice><TransactionSiteID>US</TransactionSiteID><ActualShippingCost currencyID='USD'>0.0</ActualShippingCost><ActualHandlingCost currencyID='USD'>0.0</ActualHandlingCost></Transaction></TransactionArray><BuyerUserID>tpelectronicscom</BuyerUserID><PaidTime>2015-08-08T02:00:28.000Z</PaidTime><ShippedTime>2015-08-10T07:00:00.000Z</ShippedTime><IntegratedMerchantCreditCardEnabled>false</IntegratedMerchantCreditCardEnabled><EIASToken>nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6wDk4qkC5eAqQ+dj6x9nY+seQ==</EIASToken><MonetaryDetails><Payments><Payment><PaymentStatus>Succeeded</PaymentStatus><Payer type='eBayUser'>tpelectronicscom</Payer><Payee type='eBayUser'>blastermechi</Payee><PaymentTime>2015-08-08T02:00:24.000Z</PaymentTime><PaymentAmount currencyID='USD'>30.89</PaymentAmount><ReferenceID type='ExternalTransactionID'>503062804Y3847803</ReferenceID><FeeOrCreditAmount currencyID='USD'>1.2</FeeOrCreditAmount></Payment></Payments></MonetaryDetails></Order><Order><OrderID>198295317019</OrderID><OrderStatus>Completed</OrderStatus><AdjustmentAmount currencyID='USD'>0.0</AdjustmentAmount><AmountPaid currencyID='USD'>14.84</AmountPaid><AmountSaved currencyID='USD'>0.0</AmountSaved><CheckoutStatus><eBayPaymentStatus>NoPaymentFailure</eBayPaymentStatus><LastModifiedTime>2015-08-18T02:15:10.000Z</LastModifiedTime><PaymentMethod>PayPal</PaymentMethod><Status>Complete</Status><IntegratedMerchantCreditCardEnabled>false</IntegratedMerchantCreditCardEnabled></CheckoutStatus><ShippingDetails><InsuranceFee currencyID='USD'>0.0</InsuranceFee><InsuranceOption>NotOffered</InsuranceOption><InsuranceWanted>false</InsuranceWanted><SalesTax><SalesTaxPercent>0.0</SalesTaxPercent><SalesTaxState /><ShippingIncludedInTax>false</ShippingIncludedInTax><SalesTaxAmount currencyID='USD'>0.0</SalesTaxAmount></SalesTax><ShippingServiceOptions><ShippingService>ShippingMethodStandard</ShippingService><ShippingServiceCost currencyID='USD'>0.0</ShippingServiceCost><ShippingServicePriority>1</ShippingServicePriority><ExpeditedService>false</ExpeditedService><ShippingTimeMin>1</ShippingTimeMin><ShippingTimeMax>6</ShippingTimeMax></ShippingServiceOptions><SellingManagerSalesRecordNumber>2083</SellingManagerSalesRecordNumber><GetItFast>false</GetItFast></ShippingDetails><CreatingUserRole>Buyer</CreatingUserRole><CreatedTime>2015-08-13T03:56:52.000Z</CreatedTime><PaymentMethods>PayPal</PaymentMethods><SellerEmail>stetsonfowler72@gmail.com</SellerEmail><ShippingAddress><Name>Mario Dubon</Name><Street1>5289 w postwood cir</Street1><Street2 /><CityName>Salt Lake City</CityName><StateOrProvince>UT</StateOrProvince><Country>US</Country><CountryName>United States</CountryName><Phone>801 554 2119</Phone><PostalCode>84120-5639</PostalCode><AddressID>1719387994022</AddressID><AddressOwner>eBay</AddressOwner><ExternalAddressID /></ShippingAddress><ShippingServiceSelected><ShippingInsuranceCost currencyID='USD'>0.0</ShippingInsuranceCost><ShippingService>ShippingMethodStandard</ShippingService><ShippingServiceCost currencyID='USD'>0.0</ShippingServiceCost></ShippingServiceSelected><Subtotal currencyID='USD'>14.84</Subtotal><Total currencyID='USD'>14.84</Total><ExternalTransaction><ExternalTransactionID>1T374031BA1523319</ExternalTransactionID><ExternalTransactionTime>2015-08-13T03:56:44.000Z</ExternalTransactionTime><FeeOrCreditAmount currencyID='USD'>0.73</FeeOrCreditAmount><PaymentOrRefundAmount currencyID='USD'>14.84</PaymentOrRefundAmount></ExternalTransaction><DigitalDelivery>false</DigitalDelivery><TransactionArray><Transaction><Buyer><Email>Invalid Request</Email><UserFirstName /><UserLastName /></Buyer><ShippingDetails><SellingManagerSalesRecordNumber>2081</SellingManagerSalesRecordNumber><ShipmentTrackingDetails><ShippingCarrierUsed>USPS</ShippingCarrierUsed><ShipmentTrackingNumber>9400110200793663795811</ShipmentTrackingNumber></ShipmentTrackingDetails></ShippingDetails><CreatedDate>2015-08-13T03:56:51.000Z</CreatedDate><Item><ApplicationData>290-75593</ApplicationData><ItemID>252045446757</ItemID><SKU>754016481081</SKU></Item><QuantityPurchased>1</QuantityPurchased><Status><PaymentHoldStatus>None</PaymentHoldStatus></Status><TransactionID>1699875864015</TransactionID><TransactionPrice currencyID='USD'>7.0</TransactionPrice><TransactionSiteID>US</TransactionSiteID><ActualShippingCost currencyID='USD'>0.0</ActualShippingCost><ActualHandlingCost currencyID='USD'>0.0</ActualHandlingCost></Transaction><Transaction><Buyer><Email>Invalid Request</Email><UserFirstName /><UserLastName /></Buyer><ShippingDetails><SellingManagerSalesRecordNumber>2082</SellingManagerSalesRecordNumber><ShipmentTrackingDetails><ShippingCarrierUsed>USPS</ShippingCarrierUsed><ShipmentTrackingNumber>9400110200793663795811</ShipmentTrackingNumber></ShipmentTrackingDetails></ShippingDetails><CreatedDate>2015-08-13T03:56:51.000Z</CreatedDate><Item><ApplicationData>290-75594</ApplicationData><ItemID>261990369534</ItemID><SKU>4988766207500</SKU></Item><QuantityPurchased>1</QuantityPurchased><Status><PaymentHoldStatus>None</PaymentHoldStatus></Status><TransactionID>1670605127016</TransactionID><TransactionPrice currencyID='USD'>7.84</TransactionPrice><TransactionSiteID>US</TransactionSiteID><ActualShippingCost currencyID='USD'>0.0</ActualShippingCost><ActualHandlingCost currencyID='USD'>0.0</ActualHandlingCost></Transaction></TransactionArray><BuyerUserID>reisok</BuyerUserID><PaidTime>2015-08-13T03:56:52.000Z</PaidTime><ShippedTime>2015-08-13T07:00:00.000Z</ShippedTime><IntegratedMerchantCreditCardEnabled>false</IntegratedMerchantCreditCardEnabled><EIASToken>nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6wNl4KiCpmAog2dj6x9nY+seQ==</EIASToken><MonetaryDetails><Payments><Payment><PaymentStatus>Succeeded</PaymentStatus><Payer type='eBayUser'>reisok</Payer><Payee type='eBayUser'>blastermechi</Payee><PaymentTime>2015-08-13T03:56:44.000Z</PaymentTime><PaymentAmount currencyID='USD'>14.84</PaymentAmount><ReferenceID type='ExternalTransactionID'>1T374031BA1523319</ReferenceID><FeeOrCreditAmount currencyID='USD'>0.73</FeeOrCreditAmount></Payment></Payments></MonetaryDetails></Order><Order><OrderID>198702909019</OrderID><OrderStatus>Completed</OrderStatus><AdjustmentAmount currencyID='USD'>0.0</AdjustmentAmount><AmountPaid currencyID='USD'>47.89</AmountPaid><AmountSaved currencyID='USD'>0.0</AmountSaved><CheckoutStatus><eBayPaymentStatus>NoPaymentFailure</eBayPaymentStatus><LastModifiedTime>2015-08-24T21:19:39.000Z</LastModifiedTime><PaymentMethod>PayPal</PaymentMethod><Status>Complete</Status><IntegratedMerchantCreditCardEnabled>false</IntegratedMerchantCreditCardEnabled></CheckoutStatus><ShippingDetails><InsuranceFee currencyID='USD'>0.0</InsuranceFee><InsuranceOption>NotOffered</InsuranceOption><InsuranceWanted>false</InsuranceWanted><SalesTax><SalesTaxPercent>0.0</SalesTaxPercent><SalesTaxState /><ShippingIncludedInTax>false</ShippingIncludedInTax><SalesTaxAmount currencyID='USD'>0.0</SalesTaxAmount></SalesTax><ShippingServiceOptions><ShippingService>ShippingMethodStandard</ShippingService><ShippingServiceCost currencyID='USD'>0.0</ShippingServiceCost><ShippingServicePriority>1</ShippingServicePriority><ExpeditedService>false</ExpeditedService><ShippingTimeMin>1</ShippingTimeMin><ShippingTimeMax>6</ShippingTimeMax></ShippingServiceOptions><SellingManagerSalesRecordNumber>2174</SellingManagerSalesRecordNumber><GetItFast>false</GetItFast></ShippingDetails><CreatingUserRole>Buyer</CreatingUserRole><CreatedTime>2015-08-20T18:15:25.000Z</CreatedTime><PaymentMethods>PayPal</PaymentMethods><SellerEmail>stetsonfowler72@gmail.com</SellerEmail><ShippingAddress><Name>Palco Telecom Service, Inc.</Name><Street1>2914 Green Cove Rd SW</Street1><Street2 /><CityName>Huntsville</CityName><StateOrProvince>AL</StateOrProvince><Country>US</Country><CountryName>United States</CountryName><Phone>256 883 3481</Phone><PostalCode>35803-3524</PostalCode><AddressID>810945283017</AddressID><AddressOwner>eBay</AddressOwner><ExternalAddressID /></ShippingAddress><ShippingServiceSelected><ShippingInsuranceCost currencyID='USD'>0.0</ShippingInsuranceCost><ShippingService>ShippingMethodStandard</ShippingService><ShippingServiceCost currencyID='USD'>0.0</ShippingServiceCost></ShippingServiceSelected><Subtotal currencyID='USD'>47.89</Subtotal><Total currencyID='USD'>47.89</Total><ExternalTransaction><ExternalTransactionID>68X10356TF162945D</ExternalTransactionID><ExternalTransactionTime>2015-08-20T18:15:17.000Z</ExternalTransactionTime><FeeOrCreditAmount currencyID='USD'>1.69</FeeOrCreditAmount><PaymentOrRefundAmount currencyID='USD'>47.89</PaymentOrRefundAmount></ExternalTransaction><DigitalDelivery>false</DigitalDelivery><TransactionArray><Transaction><Buyer><Email>rkindler@palcotelecom.com</Email><UserFirstName /><UserLastName /></Buyer><ShippingDetails><SellingManagerSalesRecordNumber>2173</SellingManagerSalesRecordNumber><ShipmentTrackingDetails><ShippingCarrierUsed>USPS</ShippingCarrierUsed><ShipmentTrackingNumber>9400110200828711362988</ShipmentTrackingNumber></ShipmentTrackingDetails></ShippingDetails><CreatedDate>2015-08-20T18:15:24.000Z</CreatedDate><Item><ApplicationData>286-67159</ApplicationData><ItemID>251906309576</ItemID><SKU>52837194382</SKU></Item><QuantityPurchased>1</QuantityPurchased><Status><PaymentHoldStatus>None</PaymentHoldStatus></Status><TransactionID>1703645802015</TransactionID><TransactionPrice currencyID='USD'>16.65</TransactionPrice><TransactionSiteID>US</TransactionSiteID><ActualShippingCost currencyID='USD'>0.0</ActualShippingCost><ActualHandlingCost currencyID='USD'>0.0</ActualHandlingCost></Transaction><Transaction><Buyer><Email>rkindler@palcotelecom.com</Email><UserFirstName /><UserLastName /></Buyer><ShippingDetails><SellingManagerSalesRecordNumber>2172</SellingManagerSalesRecordNumber><ShipmentTrackingDetails><ShippingCarrierUsed>USPS</ShippingCarrierUsed><ShipmentTrackingNumber>9400110200828711362988</ShipmentTrackingNumber></ShipmentTrackingDetails></ShippingDetails><CreatedDate>2015-08-20T18:15:24.000Z</CreatedDate><Item><ApplicationData>286-67162</ApplicationData><ItemID>261838572932</ItemID><SKU>58965742685</SKU></Item><QuantityPurchased>2</QuantityPurchased><Status><PaymentHoldStatus>None</PaymentHoldStatus></Status><TransactionID>1674308803016</TransactionID><TransactionPrice currencyID='USD'>15.62</TransactionPrice><TransactionSiteID>US</TransactionSiteID><ActualShippingCost currencyID='USD'>0.0</ActualShippingCost><ActualHandlingCost currencyID='USD'>0.0</ActualHandlingCost></Transaction></TransactionArray><BuyerUserID>palc2914</BuyerUserID><PaidTime>2015-08-20T18:15:25.000Z</PaidTime><ShippedTime>2015-08-21T07:00:00.000Z</ShippedTime><IntegratedMerchantCreditCardEnabled>false</IntegratedMerchantCreditCardEnabled><BuyerCheckoutMessage>REFERENCE PURCHASE ORDER NUMBER 214475</BuyerCheckoutMessage><EIASToken>nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6wAmIahCZmFqQudj6x9nY+seQ==</EIASToken><MonetaryDetails><Payments><Payment><PaymentStatus>Succeeded</PaymentStatus><Payer type='eBayUser'>palc2914</Payer><Payee type='eBayUser'>blastermechi</Payee><PaymentTime>2015-08-20T18:15:17.000Z</PaymentTime><PaymentAmount currencyID='USD'>47.89</PaymentAmount><ReferenceID type='ExternalTransactionID'>68X10356TF162945D</ReferenceID><FeeOrCreditAmount currencyID='USD'>1.69</FeeOrCreditAmount></Payment></Payments></MonetaryDetails></Order><Order><OrderID>198755723019</OrderID><OrderStatus>Completed</OrderStatus><AdjustmentAmount currencyID='USD'>0.0</AdjustmentAmount><AmountPaid currencyID='USD'>83.57</AmountPaid><AmountSaved currencyID='USD'>0.0</AmountSaved><CheckoutStatus><eBayPaymentStatus>NoPaymentFailure</eBayPaymentStatus><LastModifiedTime>2015-08-26T20:22:26.000Z</LastModifiedTime><PaymentMethod>PayPal</PaymentMethod><Status>Complete</Status><IntegratedMerchantCreditCardEnabled>false</IntegratedMerchantCreditCardEnabled></CheckoutStatus><ShippingDetails><InsuranceFee currencyID='USD'>0.0</InsuranceFee><InsuranceOption>NotOffered</InsuranceOption><InsuranceWanted>false</InsuranceWanted><SalesTax><SalesTaxPercent>0.0</SalesTaxPercent><SalesTaxState /><ShippingIncludedInTax>false</ShippingIncludedInTax><SalesTaxAmount currencyID='USD'>0.0</SalesTaxAmount></SalesTax><ShippingServiceOptions><ShippingService>ShippingMethodStandard</ShippingService><ShippingServiceCost currencyID='USD'>0.0</ShippingServiceCost><ShippingServicePriority>1</ShippingServicePriority><ExpeditedService>false</ExpeditedService><ShippingTimeMin>1</ShippingTimeMin><ShippingTimeMax>6</ShippingTimeMax></ShippingServiceOptions><SellingManagerSalesRecordNumber>2189</SellingManagerSalesRecordNumber><GetItFast>false</GetItFast></ShippingDetails><CreatingUserRole>Buyer</CreatingUserRole><CreatedTime>2015-08-21T20:07:05.000Z</CreatedTime><PaymentMethods>PayPal</PaymentMethods><SellerEmail>stetsonfowler72@gmail.com</SellerEmail><ShippingAddress><Name>Aven Bahnan</Name><Street1>5930 W McDowell Rd</Street1><Street2>Ste 106</Street2><CityName>Phoenix</CityName><StateOrProvince>AZ</StateOrProvince><Country>US</Country><CountryName>United States</CountryName><Phone>480 202 8872</Phone><PostalCode>85035-4803</PostalCode><AddressID>601986569025</AddressID><AddressOwner>eBay</AddressOwner><ExternalAddressID /></ShippingAddress><ShippingServiceSelected><ShippingInsuranceCost currencyID='USD'>0.0</ShippingInsuranceCost><ShippingService>ShippingMethodStandard</ShippingService><ShippingServiceCost currencyID='USD'>0.0</ShippingServiceCost></ShippingServiceSelected><Subtotal currencyID='USD'>83.57</Subtotal><Total currencyID='USD'>83.57</Total><ExternalTransaction><ExternalTransactionID>5RW84177FJ770044J</ExternalTransactionID><ExternalTransactionTime>2015-08-21T20:06:59.000Z</ExternalTransactionTime><FeeOrCreditAmount currencyID='USD'>2.72</FeeOrCreditAmount><PaymentOrRefundAmount currencyID='USD'>83.57</PaymentOrRefundAmount></ExternalTransaction><DigitalDelivery>false</DigitalDelivery><TransactionArray><Transaction><Buyer><Email>anj.logistics@yahoo.com</Email><UserFirstName /><UserLastName /></Buyer><ShippingDetails><SellingManagerSalesRecordNumber>2187</SellingManagerSalesRecordNumber><ShipmentTrackingDetails><ShippingCarrierUsed>USPS</ShippingCarrierUsed><ShipmentTrackingNumber>9405510200881713267234</ShipmentTrackingNumber></ShipmentTrackingDetails></ShippingDetails><CreatedDate>2015-08-21T20:07:04.000Z</CreatedDate><Item><ApplicationData>290-75724</ApplicationData><ItemID>261993061304</ItemID><SKU>447609028908</SKU></Item><QuantityPurchased>2</QuantityPurchased><Status><PaymentHoldStatus>None</PaymentHoldStatus></Status><TransactionID>1674822488016</TransactionID><TransactionPrice currencyID='USD'>28.52</TransactionPrice><TransactionSiteID>US</TransactionSiteID><ActualShippingCost currencyID='USD'>0.0</ActualShippingCost><ActualHandlingCost currencyID='USD'>0.0</ActualHandlingCost></Transaction><Transaction><Buyer><Email>anj.logistics@yahoo.com</Email><UserFirstName /><UserLastName /></Buyer><ShippingDetails><SellingManagerSalesRecordNumber>2188</SellingManagerSalesRecordNumber><ShipmentTrackingDetails><ShippingCarrierUsed>USPS</ShippingCarrierUsed><ShipmentTrackingNumber>9405510200881713267234</ShipmentTrackingNumber></ShipmentTrackingDetails></ShippingDetails><CreatedDate>2015-08-21T20:07:05.000Z</CreatedDate><Item><ApplicationData>290-75729</ApplicationData><ItemID>261993061465</ItemID><SKU>93913077195820</SKU></Item><QuantityPurchased>1</QuantityPurchased><Status><PaymentHoldStatus>None</PaymentHoldStatus></Status><TransactionID>1674822489016</TransactionID><TransactionPrice currencyID='USD'>26.53</TransactionPrice><TransactionSiteID>US</TransactionSiteID><ActualShippingCost currencyID='USD'>0.0</ActualShippingCost><ActualHandlingCost currencyID='USD'>0.0</ActualHandlingCost></Transaction></TransactionArray><BuyerUserID>anj.logistics2012</BuyerUserID><PaidTime>2015-08-21T20:07:05.000Z</PaidTime><ShippedTime>2015-08-24T07:00:00.000Z</ShippedTime><IntegratedMerchantCreditCardEnabled>false</IntegratedMerchantCreditCardEnabled><EIASToken>nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6AFkIunDpCDowydj6x9nY+seQ==</EIASToken><MonetaryDetails><Payments><Payment><PaymentStatus>Succeeded</PaymentStatus><Payer type='eBayUser'>anj.logistics2012</Payer><Payee type='eBayUser'>blastermechi</Payee><PaymentTime>2015-08-21T20:06:59.000Z</PaymentTime><PaymentAmount currencyID='USD'>83.57</PaymentAmount><ReferenceID type='ExternalTransactionID'>5RW84177FJ770044J</ReferenceID><FeeOrCreditAmount currencyID='USD'>2.72</FeeOrCreditAmount></Payment></Payments></MonetaryDetails></Order><Order><OrderID>199042229019</OrderID><OrderStatus>Completed</OrderStatus><AdjustmentAmount currencyID='USD'>0.0</AdjustmentAmount><AmountPaid currencyID='USD'>30.67</AmountPaid><AmountSaved currencyID='USD'>0.0</AmountSaved><CheckoutStatus><eBayPaymentStatus>NoPaymentFailure</eBayPaymentStatus><LastModifiedTime>2015-08-29T17:28:48.000Z</LastModifiedTime><PaymentMethod>PayPal</PaymentMethod><Status>Complete</Status><IntegratedMerchantCreditCardEnabled>false</IntegratedMerchantCreditCardEnabled></CheckoutStatus><ShippingDetails><InsuranceFee currencyID='USD'>0.0</InsuranceFee><InsuranceOption>NotOffered</InsuranceOption><InsuranceWanted>false</InsuranceWanted><SalesTax><SalesTaxPercent>0.0</SalesTaxPercent><SalesTaxState /><ShippingIncludedInTax>false</ShippingIncludedInTax><SalesTaxAmount currencyID='USD'>0.0</SalesTaxAmount></SalesTax><ShippingServiceOptions><ShippingService>ShippingMethodStandard</ShippingService><ShippingServiceCost currencyID='USD'>0.0</ShippingServiceCost><ShippingServicePriority>1</ShippingServicePriority><ExpeditedService>false</ExpeditedService><ShippingTimeMin>1</ShippingTimeMin><ShippingTimeMax>6</ShippingTimeMax></ShippingServiceOptions><SellingManagerSalesRecordNumber>2235</SellingManagerSalesRecordNumber><GetItFast>false</GetItFast></ShippingDetails><CreatingUserRole>Buyer</CreatingUserRole><CreatedTime>2015-08-27T05:19:36.000Z</CreatedTime><PaymentMethods>PayPal</PaymentMethods><SellerEmail>stetsonfowler72@gmail.com</SellerEmail><ShippingAddress><Name>francisco  portillo</Name><Street1>1207 11th St</Street1><Street2 /><CityName>north bergen</CityName><StateOrProvince>NJ</StateOrProvince><Country>US</Country><CountryName>United States</CountryName><Phone>201 758 6297</Phone><PostalCode>07047-1833</PostalCode><AddressID>2002290710025</AddressID><AddressOwner>eBay</AddressOwner><ExternalAddressID /></ShippingAddress><ShippingServiceSelected><ShippingInsuranceCost currencyID='USD'>0.0</ShippingInsuranceCost><ShippingService>ShippingMethodStandard</ShippingService><ShippingServiceCost currencyID='USD'>0.0</ShippingServiceCost></ShippingServiceSelected><Subtotal currencyID='USD'>30.67</Subtotal><Total currencyID='USD'>30.67</Total><ExternalTransaction><ExternalTransactionID>94R96584CV395254F</ExternalTransactionID><ExternalTransactionTime>2015-08-27T05:19:35.000Z</ExternalTransactionTime><FeeOrCreditAmount currencyID='USD'>0.0</FeeOrCreditAmount><PaymentOrRefundAmount currencyID='USD'>30.67</PaymentOrRefundAmount></ExternalTransaction><DigitalDelivery>false</DigitalDelivery><TransactionArray><Transaction><Buyer><Email>franckmr@hotmail.com</Email><UserFirstName /><UserLastName /></Buyer><ShippingDetails><SellingManagerSalesRecordNumber>2234</SellingManagerSalesRecordNumber><ShipmentTrackingDetails><ShippingCarrierUsed>USPS</ShippingCarrierUsed><ShipmentTrackingNumber>9400110200829717555824</ShipmentTrackingNumber></ShipmentTrackingDetails></ShippingDetails><CreatedDate>2015-08-27T05:19:35.000Z</CreatedDate><Item><ApplicationData>286-23647</ApplicationData><ItemID>251752014123</ItemID><SKU>9347083696305</SKU></Item><QuantityPurchased>1</QuantityPurchased><Status><PaymentHoldStatus>None</PaymentHoldStatus></Status><TransactionID>1706909512015</TransactionID><TransactionPrice currencyID='USD'>15.4</TransactionPrice><TransactionSiteID>US</TransactionSiteID><ActualShippingCost currencyID='USD'>0.0</ActualShippingCost><ActualHandlingCost currencyID='USD'>0.0</ActualHandlingCost></Transaction><Transaction><Buyer><Email>franckmr@hotmail.com</Email><UserFirstName /><UserLastName /></Buyer><ShippingDetails><SellingManagerSalesRecordNumber>2233</SellingManagerSalesRecordNumber><ShipmentTrackingDetails><ShippingCarrierUsed>USPS</ShippingCarrierUsed><ShipmentTrackingNumber>9400110200829717555824</ShipmentTrackingNumber></ShipmentTrackingDetails></ShippingDetails><CreatedDate>2015-08-27T05:19:35.000Z</CreatedDate><Item><ApplicationData>288-67161</ApplicationData><ItemID>261838572710</ItemID><SKU>55599865898</SKU></Item><QuantityPurchased>1</QuantityPurchased><Status><PaymentHoldStatus>None</PaymentHoldStatus></Status><TransactionID>1677514672016</TransactionID><TransactionPrice currencyID='USD'>15.27</TransactionPrice><TransactionSiteID>US</TransactionSiteID><ActualShippingCost currencyID='USD'>0.0</ActualShippingCost><ActualHandlingCost currencyID='USD'>0.0</ActualHandlingCost></Transaction></TransactionArray><BuyerUserID>samueljvl</BuyerUserID><PaidTime>2015-08-27T05:19:36.000Z</PaidTime><ShippedTime>2015-08-27T07:00:00.000Z</ShippedTime><IntegratedMerchantCreditCardEnabled>false</IntegratedMerchantCreditCardEnabled><EIASToken>nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6wMkoenDpKBpgidj6x9nY+seQ==</EIASToken><MonetaryDetails><Payments><Payment><PaymentStatus>Succeeded</PaymentStatus><Payer type='eBayUser'>samueljvl</Payer><Payee type='eBayUser'>blastermechi</Payee><PaymentTime>2015-08-27T05:19:35.000Z</PaymentTime><PaymentAmount currencyID='USD'>30.67</PaymentAmount><ReferenceID type='ExternalTransactionID'>94R96584CV395254F</ReferenceID><FeeOrCreditAmount currencyID='USD'>0.0</FeeOrCreditAmount></Payment></Payments></MonetaryDetails></Order></OrderArray><OrdersPerPage>100</OrdersPerPage><PageNumber>1</PageNumber><ReturnedOrderCountActual>5</ReturnedOrderCountActual></GetOrdersResponse>";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(rawXml);


                //XmlDocument xmlDoc = APICall.MakeAPIRequest(strReq, "GetOrders", "POST", error, seller.IsProxyRequired,
                //                      seller.ProxyIP, seller.ProxyPort, seller.ProxyUsername, seller.ProxyPassword);
            
                    if (error == "")
                    {
                        //get the root node, for ease of use
                        XmlNode root = xmlDoc["GetOrdersResponse"];
                        if (root != null)
                        {
                            if (root["Errors"] != null)
                            {
                                error += root["Errors"]["ErrorCode"].InnerText + " ";
                                error += root["Errors"]["ShortMessage"].InnerText + " ";
                                error += root["Errors"]["LongMessage"].InnerText;
                            }
                            else
                            {
                                if (root["Ack"].InnerText == "Success")
                                {
                                    string jsonText = JsonConvert.SerializeXmlNode(root);
                                    string xmls = xmlDoc.InnerXml.Replace(" xmlns=\"urn:ebay:apis:eBLBaseComponents\"", "");
                                    using (TextReader sr = new StringReader(xmls))
                                    {
                                        XmlSerializer serializer = new XmlSerializer(typeof(GetOrdersResponse));
                                        view = serializer.Deserialize(sr) as GetOrdersResponse;
                                    }
                                }
                            }
                        }
                    }

                    if (view.OrderArray.Count != 0)
                    {
                        Manage_OrderMessage Manages = new Manage_OrderMessage();
                        Manage_OrderMessage Manage = new Manage_OrderMessage();
                        foreach (var OrderArray in view.OrderArray)
                        {
                            foreach (var Transactions in OrderArray.TransactionArray)
                            {
                                decimal ItemID = Transactions.Item.ItemID;
                                Manages = db.Manage_OrderMessage.FirstOrDefault(k => k.Orderid == OrderArray.OrderID && k.Status == "Sent" && k.ItemID == ItemID);
                                if (Manages == null)
                                {
                                    var Message = db.ItemTags.FirstOrDefault(k=>k.ItemTags.Contains(ItemID.ToString()));
                                    if (Message != null)
                                    {
                                        Manage = new Manage_OrderMessage();
                                        string EmailAddress = Transactions.Buyer.Email;
                                        if (!EmailAddress.Contains("Invalid Request"))
                                        {
                                            MailMessage mail = new MailMessage();
                                            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                                            mail.From = new MailAddress("techinfoplace006@gmail.com");
                                            mail.To.Add("ankitkanojia.cmpica@gmail.com");
                                            mail.Subject = "DEmo Subject";
                                            mail.Body = "Demo Bodyss";

                                            SmtpServer.Port = 587;
                                            SmtpServer.Credentials = new System.Net.NetworkCredential("techinfoplace006@gmail.com", "techinfoplace2@15");
                                            SmtpServer.EnableSsl = true;
                                            List<ManageScheduleMessage> Confirmation = new List<ManageScheduleMessage>();
                                            try
                                            {
                                                SmtpServer.Send(mail);
                                                Manage.Status = "Sent";
                                            }
                                            catch (Exception e)
                                            {
                                                Manage.Status = "Error";
                                            }
                                        }
                                        else
                                        {
                                            //string MessageReq =  @"<?xml version=""1.0"" encoding=""utf-8""?>"+
                                            //        "<AddMemberMessagesAAQToBidderRequest xmlns=\"urn:ebay:apis:eBLBaseComponents\">"+
                                            //          "<RequesterCredentials>"+
                                            //            "<eBayAuthToken>"+seller.UserToken+"</eBayAuthToken>"+
                                            //          "</RequesterCredentials>"+
                                            //          "<AddMemberMessagesAAQToBidderRequestContainer>"+
                                            //            "<ItemID>" + ItemID +"</ItemID>"+
                                            //            "<MemberMessage>"+
                                            //              "<Body>"+ Message.Message +"</Body>"+
                                            //              "<RecipientID>"+ seller.SellarName +"</RecipientID>"+
                                            //            "</MemberMessage>"+
                                            //          "</AddMemberMessagesAAQToBidderRequestContainer>" +
                                            //        "</AddMemberMessagesAAQToBidderRequest>";
                                            //XmlDocument xmlMsgCall = APICall.MakeAPIRequest(MessageReq, "AddMemberMessagesAAQToBidder", "POST", error, seller.IsProxyRequired,
                                            //                          seller.ProxyIP, seller.ProxyPort, seller.ProxyUsername, seller.ProxyPassword);
                                            //XmlNode rootnode = xmlDoc["AddMemberMessagesAAQToBidderResponse"];
                                            //if (rootnode["Ack"].InnerText == "Success")
                                            //{
                                            //    Manage.Status = "Sent";
                                            //}
                                            //else
                                            //{
                                            //    Manage.Status = "Error";
                                            //}
                                        }
                                        Manage.Orderid = OrderArray.OrderID;
                                        Manage.Itemtagid = Message.Itemtagid;
                                        Manage.ItemID = Transactions.Item.ItemID;
                                    }
                                }
                            }
                        }
                        db.Manage_OrderMessage.Add(Manage);
                        db.SaveChanges();
                    }
                }
            }
        }
    }