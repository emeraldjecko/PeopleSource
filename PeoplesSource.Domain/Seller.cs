using System;
using System.Web.UI;

namespace PeoplesSource.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public class Seller
    {
        /// <summary>
        /// 
        /// </summary>
        public virtual int Sellerid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string SellarName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string DevID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string AppID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string CertID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string UserToken { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual int CompanyID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string ProxyIP { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string ProxyPort { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual int SiteID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string ProxyUserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string ProxyPassword { get; set; }

        /// <summary>
        /// 
        /// </summary>
       public virtual User CreatedBy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime? CreatedDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual User UpdatedBy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsCredentialsRequired { get; set; }

        public virtual bool IsProxyRequired { get; set; }

        public virtual string RuName { get; set; }

        public virtual string Email { get; set; }

    }
}
