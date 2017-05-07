using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PeoplesSource.Ebay.Models
{
    public class EbayIssueRefundModel
    {
        public int SellerId { get; set; }

        public string ReturnId { get; set; }

        [Required]
        public string Comments { get; set; }

        public RefundDetail RefundDetail { get; set; }
    }

    public class RefundDetail
    {
        public double TotalAmount { get; set; }
        public List<ItemizedRefund> ItemizedRefund { get; set; }
    }

    public class ItemizedRefund
    {
        public double Amount { get; set; }

        public RefundFeeType RefundFeeType { get; set; }
    }

    public enum RefundFeeType
    {
        PURCHASE_PRICE,
        ORIGINAL_SHIPPING,
        RESTOCKING_FEE,
        OTHER,
    }
}