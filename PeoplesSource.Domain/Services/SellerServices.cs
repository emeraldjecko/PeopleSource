using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeoplesSource.Common;

namespace PeoplesSource.Domain.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class SellerServices : ISellerServices
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly IRepository<Seller> _SellerDetailRepository;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sellerDetailRepository"></param>
        public SellerServices(IRepository<Seller> sellerDetailRepository)
        {
            _SellerDetailRepository = sellerDetailRepository.ThrowIfNull("sellerDetailRepository");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sellerId"></param>
        /// <returns></returns>
        public Seller GetSellerDetail(int sellerId)
        {
            return _SellerDetailRepository.GetById(sellerId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sellerDetail"></param>
        public void SaveSellerDetail(Seller sellerDetail)
        {
            _SellerDetailRepository.Save(sellerDetail);
        }
      
        /// <summary>
        /// 
        /// </summary>

        /// <param name="sellerId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PagedList<Seller> GetSeller(int pageIndex, int pageSize)
        {
            var sellerlist =
               _SellerDetailRepository.Query().GroupBy(x=>x.Sellerid).Select(n => new { Items = n.ToList() });

            var SellerPagedList = new List<Seller>();
            foreach (var seller_list in sellerlist)
            {
                foreach (var s in seller_list.Items)
                {
                    SellerPagedList.Add(s);
                    break;
                }
            }

            var query = SellerPagedList;
            return new PagedList<Seller>
            {
                Total = query.Count(),
                Items = query
                    .Skip(pageSize * (pageIndex - 1))
                    .Take(pageSize)
                    .ToList()
            };
        }
          public List<Seller> GetSeller()
          {
             return _SellerDetailRepository.GetAll().ToList();
          }

          public void Delete(int Id)
          {
              _SellerDetailRepository.Delete(Id);
          }

    }
}
