using PeoplesSource.Common;
using PeoplesSource.Domain;
using PeoplesSource.Models;

namespace PeoplesSource.Mappers
{
    public class SellerDetailMapper : BaseMapper, IMapper<Seller, SellerDetail>
    {
        public SellerDetail Map(Seller seller)
        {
            return new SellerDetail
            {
                Sellerid = seller.Sellerid,
                SellarName = seller.SellarName,
                DevID = seller.DevID,
                AppID = seller.AppID,
                CertID = seller.CertID,
                ProxyIP = seller.ProxyIP,
                ProxyPort = seller.ProxyPort,
                UpdatedDate = seller.UpdatedDate,
                CreatedDate = seller.CreatedDate,
                UserToken = seller.UserToken,
                IsActive = seller.IsActive,
                CompanyID = seller.CompanyID,
                ProxyPassword = seller.ProxyPassword,
                ProxyUserName = seller.ProxyUserName,
                IsCredentialsRequired = seller.IsCredentialsRequired,
                IsProxyRequired = seller.IsProxyRequired,
                RuName = seller.RuName,
                Email = seller.Email

            };
        }
    }
}