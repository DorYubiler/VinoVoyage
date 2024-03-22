using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VinoVoyage.Models
{
    public class UserModel
    {
        [Key]
        [Required]
        [Display(Name = "Username")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "User name must be between 3 and 10")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(10, MinimumLength = 6, ErrorMessage = "Password must be beteen 6 and 10")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Email")]
        [StringLength(30, MinimumLength = 12, ErrorMessage = "Email must be beteen 12 and 30")]
        public string Email { get; set; }
        public string Role { get; set; } = "customer";




    }
}