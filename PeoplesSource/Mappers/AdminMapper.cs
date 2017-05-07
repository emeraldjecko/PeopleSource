using PeoplesSource.Common;
using PeoplesSource.Domain;
using PeoplesSource.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PeoplesSource.Mappers
{
    public class AdminMapper : BaseMapper, IDomainMapper<AdminDetail, Admin>
    {
        public Admin Map(AdminDetail admindetail)
        {
            return new Admin
            {
                AdminId = admindetail.AdminId,
                UserId = admindetail.UserId,
                FirstName = admindetail.FirstName,
                LastName = admindetail.LastName,
                UserName = admindetail.UserName,
                Email = admindetail.Email,
                Password = admindetail.Password,
                UpdatedDate = admindetail.UpdatedDate,
                CreatedDate = admindetail.CreatedDate,
                IsActive = admindetail.IsActive,
                CreatedBy = admindetail.CreatedBy != null ? Session.Load<User>(admindetail.CreatedBy) : null,
                UpdatedBy = admindetail.UpdatedBy != null ? Session.Load<User>(admindetail.UpdatedBy) : null
            };
        }

        public void MapToExisting(AdminDetail admindetail, Admin admin)
        {
            admin.UserId = admindetail.UserId;
            admin.FirstName = admindetail.FirstName;
            admin.LastName = admindetail.LastName;
            admin.UserName = admindetail.UserName;
            admin.Email = admindetail.Email;
            admin.Password = admindetail.Password;
            admin.IsActive = admindetail.IsActive;
            admin.CreatedDate = Convert.ToDateTime(admindetail.CreatedDate);
            admin.UpdatedDate = Convert.ToDateTime(admindetail.UpdatedDate);
            admin.CreatedBy = admindetail.CreatedBy != null ? Session.Load<User>(admindetail.CreatedBy) : null;
            admin.UpdatedBy = admindetail.UpdatedBy != null ? Session.Load<User>(admindetail.UpdatedBy) : null;
        }
    }
}