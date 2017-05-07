using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeoplesSource.Common;

namespace PeoplesSource.Domain.Services
{
    public class AdminService : IAdminService
    {
        public readonly IRepository<Admin> _AdminDetailRepository;

        public AdminService(IRepository<Admin> adminDetailRepository)
        {
            _AdminDetailRepository = adminDetailRepository.ThrowIfNull("AdminDetailRepository");
        }

        public Admin GetAdminDetail(int adminId)
        {
            return _AdminDetailRepository.GetById(adminId);
        }

        public void SaveAdminDetail(Admin adminDetail)
        {
            _AdminDetailRepository.Save(adminDetail);
        }

        public PagedList<Admin> GetAdmin(int pageIndex, int pageSize)
        {
            var Adminlist = _AdminDetailRepository.Query().GroupBy(x => x.AdminId).Select(n => new { Items = n.ToList() });

            var AdminPagedList = new List<Admin>();
            foreach (var Admin_list in Adminlist)
            {
                foreach (var s in Admin_list.Items)
                {
                    AdminPagedList.Add(s);
                    break;
                }
            }

            var query = AdminPagedList;
            return new PagedList<Admin>
            {
                Total = query.Count(),
                Items = query.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList()
            };
        }

        public List<Admin> GetAdmin()
        {
            return _AdminDetailRepository.GetAll().ToList();
        }

        public void Delete(int Id)
        {
            _AdminDetailRepository.Delete(Id);
        }
    }
}
