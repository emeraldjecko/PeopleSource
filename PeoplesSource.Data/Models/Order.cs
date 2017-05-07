using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeoplesSource.Data.Models
{
    [Table("Order")]
    public  class Order
    {
        [Key]
        public int Id { get; set; }
        public string order_source { get; set; }

        [StringLength(255)]
        public string account { get; set; }

        [StringLength(255)]
        public string txn_id { get; set; }

        public DateTime? date { get; set; }
        public string Datetest { get; set; }
        [StringLength(255)]
        public string status { get; set; }

        [StringLength(255)]
        public string name { get; set; }

        [StringLength(255)]
        public string payer_email { get; set; }

        [StringLength(255)]
        public string address_country { get; set; }
        [StringLength(255)]
        public string address_state { get; set; }
        [StringLength(255)]
        public string address_zip { get; set; }
        [StringLength(255)]
        public string address_city { get; set; }

        [StringLength(255)]
        public string address_street { get; set; }

        [StringLength(255)]
        public string address_street2 { get; set; }

        [StringLength(255)]
        public string tracking { get; set; }

        [StringLength(255)]
        public string item_name { get; set; }
        [StringLength(255)]
        public string item_sku { get; set; }

        [StringLength(255)]
        public string item_description { get; set; }

        public string TrackerXML { get; set; }
        public bool TrackingClosedStatus { get; set; }

    }
    
}
