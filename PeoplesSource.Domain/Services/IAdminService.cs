using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeoplesSource.Domain.Services
{
    public interface IAdminService
    {
        Admin GetAdminDetail(int adminId);

        void SaveAdminDetail(Admin adminDetail);

        PagedList<Admin> GetAdmin(int pageIndex, int pageSize);

        List<Admin> GetAdmin();

        void Delete(int Id);
    }
}
