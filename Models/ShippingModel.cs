using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace VinoVoyage.Models
{
    public class ShippingModel
    {
        [Required]
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ShippingID")]
        public int ShippingID { get; set; }

        [Required]
        [Key, Column(Order = 1)]
        [Display(Name = "UserName")]
        [StringLength(10, MinimumLength = 3)]
        public string UserName { get; set; }

        [Required]
        [Display(Name ="OrderDate")]
        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; }

        [Required]
        [Display(Name ="ShippingDate")]
        [DataType(DataType.DateTime)]
        public DateTime ShippingDate { get; set; }

        [Required]
        [Display(Name ="Address")]
        [StringLength (30, MinimumLength = 3)]
        public string Address { get; set;}
    }
}