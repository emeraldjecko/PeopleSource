using System;
using System.ComponentModel.DataAnnotations;

namespace PeoplesSource.Models
{
    public class SellerLine
    {

        public int SellerId { get; set; }

        public string SellarName { get; set; }

        public string DevID { get; set; }

        public string AppID { get; set; }
        
        public string CertID { get; set; }

        public string UserToken { get; set; }

        public string ProxyIP { get; set; }

        public string ProxyPort { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }
        
        public string IsActive { get; set; }

        public string ProxyPassword { get; set; }

        public string ProxyUserName { get; set; }

        public bool IsCredentialsRequired { get; set; }

        public string RuName { get; set; }

        public string Email { get; set; }
    }
}