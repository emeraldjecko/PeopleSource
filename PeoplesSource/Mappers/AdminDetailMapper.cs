using PeoplesSource.Common;
using PeoplesSource.Domain;
using PeoplesSource.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PeoplesSource.Mappers
{
    public class AdminDetailMapper : BaseMapper, IMapper<Admin, AdminDetail>
    {
        public AdminDetail Map(Admin admin)
        {
            return new AdminDetail
            {
                AdminId = admin.AdminId,
                UserId = admin.UserId ?? Guid.Empty,
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                UserName = admin.UserName,
                Email = admin.Email,
                Password = admin.Password,
                UpdatedDate = admin.UpdatedDate,
                CreatedDate = admin.CreatedDate,
                IsActive = admin.IsActive
            };
        }
    }
}