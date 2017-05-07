using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PeoplesSource.Models
{
    public class ChangePassword
    {
        [Required(ErrorMessage = "Current password is required")]
        [DisplayName(@"Current password")]
        public string OldPassword { get; set; }

        //[Required(ErrorMessage = "New password is required")]
       // [DisplayName(@"New password")]
        [Required(ErrorMessage = "New password is required")]
        [RegularExpression(@"^.*(?=.{7,})(?=.*[a-zA-Z0-9])(?=.*[@#$%^&+=]).*$", ErrorMessage = "Password length minimum 7 and Non-alphanumeric character required")]
        [DataType(DataType.Password)]
        [DisplayName(@"New password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [DisplayName(@"Confirm password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirm password does not match")]
        public string ConfirmPassword { get; set; }
    }
}