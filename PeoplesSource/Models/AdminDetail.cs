using System;
using System.ComponentModel.DataAnnotations;

namespace PeoplesSource.Models
{
    public class AdminDetail
    {
        public int AdminId { get; set; }

        public Guid UserId { get; set; }

        [Required(ErrorMessage = "User Name is required")]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [StringLength(200)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(200)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [StringLength(100)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "IsActive is required")]
        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}