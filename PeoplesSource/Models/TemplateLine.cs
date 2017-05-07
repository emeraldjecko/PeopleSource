using System;

namespace PeoplesSource.Models
{
    public class TemplateLine
    {
        public int Id { get; set; }

        public string Seller { get; set; }

        public string TemplateName { get; set; }

        public int SellerId { get; set; }
        
        public string TemplateContent { get; set; }        

        public DateTime CreatedDate { get; set; }
    }
}