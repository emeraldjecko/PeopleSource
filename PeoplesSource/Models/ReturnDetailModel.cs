using System;
using System.Collections.Generic;

namespace PeoplesSource.Models
{
    public class ReturnDetailModel
    {
        public ReturnDetailModel()
        {
            ActualItemizedRefund = new List<ItemizedRefund>();
            EstimateItemizedRefund = new List<ItemizedRefund>();
            EstimateItemizedOptionalRefund = new List<ItemizedRefund>();
            ReturnHistorys = new List<ReturnHistory>();
            ShippingInfo = new List<ShippingInfo>();
            ErrorItems = new List<ErrorModel>();
        }
        
        public List<ErrorModel> ErrorItems { get; set; }
        public string Ack { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Version { get; set; }

        public string BuyerReturnShipmentCarrierUsed { get; set; }
        public DateTime BuyerReturnShipmentDeliveryDate { get; set; }
        public DateTime BuyerReturnShipmentMaxEstDeliveryDate { get; set; }
        public DateTime BuyerReturnShipmentMinEstDeliveryDate { get; set; }
        public string BuyerReturnShipmentMerchandiseAuthorization { get; set; }
        public string BuyerReturnShipmentStatus { get; set; }
        public string BuyerReturnShipmentCity { get; set; }
        public string BuyerReturnShipmentCountry { get; set; }
        public string BuyerReturnShipmentCounty { get; set; }
        public string BuyerReturnShipmentName { get; set; }
        public string BuyerReturnShipmentPostalCode { get; set; }
        public string BuyerReturnShipmentPostalStateOrProvince { get; set; }
        public string BuyerReturnShipmentStreet1 { get; set; }
        public string BuyerReturnShipmentStreet2 { get; set; }
        public double BuyerReturnShipmentCost { get; set; }
        public string BuyerReturnShipmentTrackingNumber { get; set; }

        public string CaseId { get; set; }
        public string CaseType { get; set; }
        public string GlobalId { get; set; }

        public DateTime ActualRefundDate { get; set; }
        public string ActualRefundStatus { get; set; }
        public List<ItemizedRefund> ActualItemizedRefund { get; set; }
        public double ActualRefundTotalAmount { get; set; }

        public List<ItemizedRefund> EstimateItemizedRefund { get; set; }
        public List<ItemizedRefund> EstimateItemizedOptionalRefund { get; set; }
        public double EstimateRefundTotalAmount { get; set; }

        public DateTime RefundDueDate { get; set; }

        public List<ReturnHistory> ReturnHistorys { get; set; }

        public bool ReturnPolicyOptedForMultipleReturnAddress { get; set; }
        public bool ReturnPolicyoptedForRMA { get; set; }

        public List<ShippingInfo> ShippingInfo { get; set; }
        public ReturnItemModel ReturnSummery { get; set; }
       
    }


    public class ItemizedRefund
    {
        public string CurrencyId { get; set; }
        public double Amount { get; set; }
        public string FeeType { get; set; }
    }

    public class ReturnHistory
    {
        public string ActivityCode { get; set; }
        public string ActivityContent { get; set; }
        public string ActivityDescription { get; set; }
        public string AuthorRole { get; set; }
        public string AuthorUserId { get; set; }
        public DateTime CreationDate { get; set; }
        public string Note { get; set; }
    }

    public class ShippingInfo
    {
        public string CarrierUsed { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime MaxEstDeliveryDate { get; set; }
        public DateTime MinEstDeliveryDate { get; set; }
        public string ReturnMerchandiseAuthorization { get; set; }
        public string ShipmentStatus { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string County { get; set; }
        public string Name { get; set; }
        public string PostalCode { get; set; }
        public string StateOrProvince { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public double ShippingCost { get; set; }
        public string CurrencyId { get; set; }
        public string TrackingNumber { get; set; }
    }

}