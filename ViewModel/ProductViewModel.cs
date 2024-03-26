using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VinoVoyage.Models;

namespace VinoVoyage.ViewModel
{
    public class ProductViewModel
    {
        public ProductModel product {  get; set; }
        public List<ProductModel> products { get; set; }
    }
}