using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VinoVoyage.Models
{
    public class ProductModel
    {
       
        [Required]
        [Key]
        [Display(Name ="ProductID")]
        public int ProductID { get; set; }
        
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
        /* %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%  in price, changed the type to int !!!!!!!! %%%%%%%%%%% */
        [Required]
        [Display(Name = "Price")]
        public int Price { get; set; }

        public ProductModel() 
        {
          
        }

    }
}