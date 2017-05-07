using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PeoplesSource.Models
{
    public class SellerDetail
    {
        public int Sellerid { get; set; }

        [Required(ErrorMessage = "Seller Name is required")]
        public string SellarName { get; set; }

        [Required(ErrorMessage = "DevID is required")]
        public string DevID { get; set; }

        [Required(ErrorMessage = "AppID is required")]
        public string AppID { get; set; }

        [Required(ErrorMessage = "CertID is required")]
        public string CertID { get; set; }

        [Required(ErrorMessage = "UserToken is required")]
        public string UserToken { get; set; }

        public int CompanyID { get; set; }

        public bool IsProxyRequired { get; set; }

        [RequiredIf("IsProxyRequired", true, ErrorMessage = "ProxyIP is required")]
        [RegularExpression(@"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$", ErrorMessage = "Enter ProxyIp In correct format(000.000.00.00)")]
        public string ProxyIP { get; set; }

        [RequiredIf("IsProxyRequired", true, ErrorMessage = "ProxyPort is required")]
        [RegularExpression(@"6553[0-5]|655[0-2][0-9]|65[0-4][0-9][0-9]|6[0-4][0-9][0-9][0-9]|\d{2,4}|[1-9]",ErrorMessage = "Enter proxyport upto 4 degit")]
        public string ProxyPort { get; set; }

        public int SiteID { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [Required(ErrorMessage = "IsActive is required")]
        public bool IsActive { get; set; }

        public bool IsCopytemplate { get; set; }

        [RequiredIf("IsCopytemplate", true, ErrorMessage = "Please select Seller")]
        public int? SellId { get; set; }

        public List<LookupItem> sellerList { get; set; }

        public bool IsCredentialsRequired { get; set; }

        [RequiredIf("IsCredentialsRequired", true,ErrorMessage = "Proxy Password is required")]
        [Display(Name = "Proxy Password")]
        public string ProxyPassword { get; set; }

        [RequiredIf("IsCredentialsRequired",true, ErrorMessage = "Proxy Username is required")]
        [Display(Name = "Proxy Username")]
        public string ProxyUserName { get; set; }

        //[Required(ErrorMessage = "RuName is required")]
        public string RuName { get; set; }

        [Required(ErrorMessage = "Email Address is required")]
        public string Email { get; set; }
    }
}