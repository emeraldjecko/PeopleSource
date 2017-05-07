using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PeoplesSource.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public string order_source { get; set; }

     
        public string account { get; set; }

       
        public string txn_id { get; set; }

        public DateTime? date { get; set; }
        public string Datetest { get; set; }
       
        public string status { get; set; }

        
        public string name { get; set; }

      
        public string payer_email { get; set; }

        
        public string address_country { get; set; }
    
        public string address_state { get; set; }
        
        public string address_zip { get; set; }
        
        public string address_city { get; set; }

        
        public string address_street { get; set; }

        
        public string address_street2 { get; set; }

        
        public string tracking { get; set; }

        
        public string item_name { get; set; }
        
        public string item_sku { get; set; }

        
        public string item_description { get; set; }

        public string TrackerXML { get; set; }
        public TrackResponse trackResponse { get; set; }
    }
    
}