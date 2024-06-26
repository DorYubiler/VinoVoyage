﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VinoVoyage.Models;

namespace VinoVoyage.ViewModel
{
    public class UserViewModel
    {
        public UserModel user {  get; set; }
        public List<UserModel> users { get; set; }
        public List<ProductModel> products { get; set; }
        public List<OrderModel> cart {  get; set; }

        public List<WishListModel> wishList { get; set; }
        public List<ProductModel> filter { get; set; }
        public List<ShippingModel> shipping { get; set; }
    }
}