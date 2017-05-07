using System;
using System.Collections.Generic;

namespace PeoplesSource.Models
{
    public class ReturnModel
    {
        public ReturnModel()
        {
            ReturnItems = new List<ReturnItemModel>();
            ReturnErrorItems = new List<ErrorModel>();
        }
        public int SellerId { get; set; }
        public List<ReturnItemModel> ReturnItems { get; set; }
        public List<ErrorModel> ReturnErrorItems { get; set; }
        public string Ack { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Version { get; set; }        
       
    }

    public class ReturnItemModel
    {
        public ReturnItemModel()
        {
            ReturnRequestItems = new List<ReturnRequestItem>();
        }
        public int selId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        
        public string OtherPartyRole { get; set; }
        public string OtherPartyUserId { get; set; }

        public string ResponseDuePartyRole { get; set; }
        public string ResponseDuePartyUserId { get; set; }
        public DateTime ResponseDueRespondByDate { get; set; }

        public string ReturnId { get; set; }

        public string ItemId { get; set; }

        public string ReturnRequestComment { get; set; }
        public List<ReturnRequestItem> ReturnRequestItems { get; set; }
        public string ReturnRequestReasonCode { get; set; }
        public string ReturnRequestReasonContent { get; set; }
        public string ReturnRequestReasonDescription { get; set; }

        public string ReturnType { get; set; }
        public string Status { get; set; }        
    }

    public class ReturnRequestItem
    {
        public string ReturnRequestItemId { get; set; }
        public int ReturnRequestItemQuantity { get; set; }
        public string ReturnRequestTransactionId { get; set; }
    }

}