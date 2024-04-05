using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace VinoVoyage.Models
{
    public class WishListModel
    {
        [Key]
        [Column(Order = 0)]
        [Required]
        [Display(Name = "Username")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "User name must be between 3 and 10")]
        public string Username { get; set; }

        [Key]
        [Column(Order = 1)]
        [Required]
        [Display(Name = "ProductID")]
        public int ProductID { get; set; }

    }
}