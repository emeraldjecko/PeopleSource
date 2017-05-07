using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeoplesSource.Data.Models
{

    [Table("BestMatchPosition")]
    public partial class BestMatchPosition
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string RealSKU { get; set; }

        [StringLength(255)]
        public string Title { get; set; }

     
        [StringLength(255)]
        public string SellerId { get; set; }

        [StringLength(255)]
        public string ItemNumber { get; set; }

        public int Position { get; set; }
        public double? Price { get; set; }
        public double? Price2 { get; set; }
        public string TotalListings { get; set; }
        public double? sold { get; set; }
        public string search_term { get; set; }

        [StringLength(255)]
        public string url { get; set; }
        

        [StringLength(255)]
        public string shipping { get; set; }

        public DateTime? DateCreated { get; set; }
        public DateTime? Date { get; set; }
        
    }
}
