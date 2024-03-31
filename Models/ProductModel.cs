﻿using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        
        [Range(0,200,ErrorMessage="Amount must be between 0-200") ]
        [Required]
        [Display(Name = "Amount")]
        public int Amount { get; set; }
        /* %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%  in price, changed the type to int !!!!!!!! %%%%%%%%%%% */
        [Range(0, int.MaxValue, ErrorMessage = "price must be >0")]
        [Required]
        [Display(Name = "Price")]
        public int Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "new price must be >0")]
        [Required]
        [Display(Name = "NewPrice")]
        public int NewPrice { get; set; }

        public ProductModel() 
        {
          
        }

    }
}