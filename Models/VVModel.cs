using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VinoVoyage.Models
{
    public class VVModel
    {
        public UserModel user { get; set; }
        public List<UserModel> users { get; set; }
        public List<ProductModel> products { get; set; }
    }
}