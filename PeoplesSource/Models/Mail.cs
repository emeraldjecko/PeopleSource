using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc.Html;
using NHibernate.Mapping;

namespace PeoplesSource.Models
{
    public class Mail
    {
        [Required(ErrorMessage = "Message Subject is required")]
        [Display(Name = "Message Subject")]
        public string MailSubject { get; set; }

        [Required(ErrorMessage = "Message Body is required")]
        [Display(Name = "Message Body")]
        public string MailBody { get; set; }

        public string ItemId { get; set; }

        public string RecepientId { get; set; }

        public int SellerId { get; set; }

        [Display(Name="Select Template")]
        public int TemplateId { get; set; }

        public List<LookupItem> TemplateList { get; set; }
    }
}