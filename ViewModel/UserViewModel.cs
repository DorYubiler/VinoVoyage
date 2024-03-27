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
        // public UserViewModel() {
        //    products = new List<ProductModel>();
        //}


    }
}