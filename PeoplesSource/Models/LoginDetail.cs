using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PeoplesSource.Models
{
    public class LoginDetail
    {
        [Required(ErrorMessage = "User Name is required")]
        //[RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "The username or password provided is incorrect.")]
        [DisplayName("User Name")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }

        [DisplayName("Remember me?")]
        public bool RememberMe { get; set; }

        public bool Errorflag { get; set; }

        public string ErrorMessage { get; set; }

        public string ColorCookie { get; set; }
    }
}