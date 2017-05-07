using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeoplesSource.Domain.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISellerServices
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sellerId"></param>
        /// <returns></returns>
        Seller GetSellerDetail(int sellerId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sellerDetail"></param>
        void SaveSellerDetail(Seller sellerDetail);
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sellerId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedList<Seller> GetSeller(int pageIndex, int pageSize);

        List<Seller> GetSeller();

        void Delete(int Id);
    }
}
