using PeoplesSource.Common;
using PeoplesSource.Domain;
using PeoplesSource.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PeoplesSource.Mappers
{
    public class AdminListMapper : BaseMapper, IMapper<Admin, AdminLine>
    {
        public AdminLine Map(Admin admin)
        {
            return new AdminLine
            {
                AdminId = admin.AdminId,
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                UserId = admin.UserId ?? Guid.Empty,
                UserName = admin.UserName,
                Email = admin.Email,
                Password = admin.Password,
                IsActive = admin.IsActive ? true : false,
                CreatedDate = admin.CreatedDate,
                UpdatedDate = admin.UpdatedDate
            };


        }
    }
}