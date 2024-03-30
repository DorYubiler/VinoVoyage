using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VinoVoyage.Models
{
    public class OrderModel
    {
        [Key]
        [Column(Order = 1)]
        [Required]
        [Display(Name = "Username")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "User name must be between 3 and 10")]
        public string Username { get; set; }

        [Key]
        [Column(Order = 2)]
        [Required]
        [Display(Name = "ProductID")]
        public int ProductID { get; set; }
        
        [Required]
        [Display(Name = "Quantity")]
        public int Quantity {  get; set; }
    }
}