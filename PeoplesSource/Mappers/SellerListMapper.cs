using PeoplesSource.Common;
using PeoplesSource.Domain;
using PeoplesSource.Models;

namespace PeoplesSource.Mappers
{
    public class SellerListMapper : BaseMapper, IMapper<Seller, SellerLine>
    {
        public SellerLine Map(Seller seller)
        {
            return new SellerLine
            {
                SellerId = seller.Sellerid,
                SellarName = seller.SellarName,
                ProxyIP = seller.ProxyIP,
                ProxyPort = seller.ProxyPort,
                IsActive = seller.IsActive ? "True" : "False"
                
            };


        }
    }
}