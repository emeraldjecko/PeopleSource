using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PeoplesSource.Models
{
    public class ForgotPassword
    {
        [Required(ErrorMessage = @"User Name is required")]
        [DisplayName(@"User Name")]
        public string UserName { get; set; }
    }
}