using System;
using System.Collections.Generic;

namespace PeoplesSource.Models
{
    public class SellerList
    {
        public List<SellerLine> SellerItems { get; set; }

        public string SellarName { get; set; }

        public int SellerId { get; set; }

        public List<TemplateLine> templateList { get; set; }

        public string tdId { get; set; }
    }

   
}