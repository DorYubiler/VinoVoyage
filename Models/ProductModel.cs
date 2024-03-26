using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VinoVoyage.Models
{
    public class ProductModel
    {
        [Key]
        [Required]
        [Display(Name = "ProductId")]
        public int ProductId { get; set; }

        [Required]
        [Display(Name = "ProductName")]
        [StringLength(30, MinimumLength = 4)]
        public string ProductName {  get; set; }
        [Required]
        [Display(Name = "Type")]
        [StringLength(30, MinimumLength = 4)]
        public string Type { get; set; }
        [Required]
        [Display(Name = "Description")]
        [StringLength(500, MinimumLength = 4)]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Origin")]
        [StringLength(50, MinimumLength = 4)]
        public string Origin { get; set; }
        [Required]
        [Display(Name = "Amount")]
        public int Amount { get; set; }

        [Required]
        [Display(Name = "Price")]
        public float Price { get; set; }

    }
}