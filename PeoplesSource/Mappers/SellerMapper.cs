using System;
using PeoplesSource.Common;
using PeoplesSource.Domain;
using PeoplesSource.Models;

namespace PeoplesSource.Mappers
{
    public class SellerMapper : BaseMapper, IDomainMapper<SellerDetail, Seller>
    {
        public Seller Map(SellerDetail sellerdetail)
        {

            return new Seller
            {
                Sellerid = sellerdetail.Sellerid,
                SellarName = sellerdetail.SellarName,
                DevID = sellerdetail.DevID,
                AppID = sellerdetail.AppID,
                CertID = sellerdetail.CertID,
                UserToken = sellerdetail.UserToken,
                CompanyID = sellerdetail.CompanyID,
                ProxyIP = sellerdetail.ProxyIP,
                ProxyPort = sellerdetail.ProxyPort,
                SiteID = sellerdetail.SiteID,
                CreatedDate = Convert.ToDateTime(sellerdetail.CreatedDate),
                UpdatedDate = sellerdetail.UpdatedDate,
                IsActive = sellerdetail.IsActive,
                ProxyPassword = sellerdetail.ProxyPassword,
                ProxyUserName = sellerdetail.ProxyUserName,
                IsCredentialsRequired = sellerdetail.IsCredentialsRequired,
                IsProxyRequired = sellerdetail.IsProxyRequired,
                CreatedBy = sellerdetail.CreatedBy != null ? Session.Load<User>(sellerdetail.CreatedBy) : null,
                UpdatedBy = sellerdetail.UpdatedBy != null ? Session.Load<User>(sellerdetail.UpdatedBy) : null,
                RuName = sellerdetail.RuName
            };


        }


        public void MapToExisting(SellerDetail sellerdetail, Seller sellers)
        {

            sellers.SellarName = sellerdetail.SellarName;
            sellers.DevID = sellerdetail.DevID;
            sellers.AppID = sellerdetail.AppID;
            sellers.CertID = sellerdetail.CertID;
            sellers.UserToken = sellerdetail.UserToken;
            sellers.CompanyID = sellerdetail.CompanyID;
            sellers.ProxyIP = sellerdetail.ProxyIP;
            sellers.ProxyPort = sellerdetail.ProxyPort;
            sellers.SiteID = sellerdetail.SiteID;
            sellers.CreatedDate = Convert.ToDateTime(sellerdetail.CreatedDate);
            sellers.UpdatedDate = Convert.ToDateTime(sellerdetail.UpdatedDate);
            sellers.ProxyUserName = sellerdetail.ProxyUserName;
            sellers.IsCredentialsRequired = sellerdetail.IsCredentialsRequired;
            sellers.CreatedBy = sellerdetail.CreatedBy != null ? Session.Load<User>(sellerdetail.CreatedBy) : null;
            sellers.UpdatedBy = sellerdetail.UpdatedBy != null ? Session.Load<User>(sellerdetail.UpdatedBy) : null;
            sellers.IsActive = sellerdetail.IsActive;
            sellers.ProxyPassword = sellerdetail.ProxyPassword;
            sellers.ProxyUserName = sellerdetail.ProxyUserName;
            sellers.IsCredentialsRequired = sellerdetail.IsCredentialsRequired;
            sellers.IsProxyRequired = sellerdetail.IsProxyRequired;
            sellers.RuName = sellerdetail.RuName;
        }
    }
}