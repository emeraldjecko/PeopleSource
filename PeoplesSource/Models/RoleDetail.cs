using System;
using System.ComponentModel.DataAnnotations;

namespace PeoplesSource.Models
{
    public class RoleDetail
    {
        public Guid RoleId { get; set; }

        [Required(ErrorMessage = "Role Name is required")]
        public string RoleName { get; set; }

    }
}