using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PeoplesSource.Ebay.Models
{
    public class GetMyMessagesResponse
    {
        public DateTime Timestamp { get; set; }

        public string Ack { get; set; }

        public string Version { get; set; }

        public string Build { get; set; }

        public List<Message> Messages { get; set; }
    }

    public class GetOrdersResponse
    {
        public DateTime Timestamp { get; set; }

        public string Ack { get; set; }

        public string Version { get; set; }

        public string Build { get; set; }

        public List<Order> OrderArray { get; set; }
    }

    public class Transaction
    {
        public Item Item { get; set; }
        public Buyer Buyer { get; set; }
    }
    public class Item
    {   
        public decimal ItemID { get; set; }
    }
    public class Buyer
    {
        public string Email { get; set; }
    }
    public class Order
    {
        public List<Transaction> TransactionArray { get; set; }
        public decimal OrderID { get; set; }
        public string OrderStatus { get; set; }
    } 
    public class Message
    {
        
        public string Sender { get; set; }

        public string SendingUserID { get; set; }

        public string RecipientUserID { get; set; }

        public string SendToName { get; set; }

        public string MessageID { get; set; }

        public string ExternalMessageID { get; set; }

        public bool Flagged { get; set; }

        public bool Read { get; set; }

        public DateTime ReceiveDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public ResponseDetail ResponseDetails { get; set; }

        public string Subject { get; set; }

        public Folder Folder { get; set; }

        public string Text { get; set; }

        public string MessageType { get; set; }

        public bool Replied { get; set; }

        public bool Type { get; set; }

        public DateTime ItemEndTime { get; set; }

        public DateTime? DeleteDate { get; set; }

        public string ItemTitle { get; set; }

        public decimal ItemID { get; set; }

        public List<TagList> TagDetail { get; set; }

        public string Note { get; set; }

        public string Disable { get; set; }
    }
        
    public class ResponseDetail
    {   
        public bool ResponseEnabled { get; set; }

        public string ResponseURL { get; set; }
    }

    public class Folder
    {
        public string FolderID { get; set; }
    }

      public class TagList
        {
            public string TagName { get; set; }
            public int Total { get; set; }
            public bool Checked { get; set; }
            public decimal TagsID { get; set; }
            public DateTime? DeleteDate { get; set; }
        }
        
}