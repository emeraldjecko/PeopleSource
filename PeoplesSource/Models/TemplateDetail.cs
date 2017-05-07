using System;
using System.ComponentModel.DataAnnotations;

namespace PeoplesSource.Models
{
    public class TemplateDetail
    {
        public int Id { get; set; }

        public int SellerId { get; set; }

        [Required(ErrorMessage = "Template Name is required")]
        [Display(Name = "Name")]
        public string TemplateName { get; set; }

        [Required(ErrorMessage = "Template Content is required")]
        [Display(Name = "Content")]
        public string TemplateContent { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}