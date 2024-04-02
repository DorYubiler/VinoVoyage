﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VinoVoyage.Dal;
using VinoVoyage.Models;
using VinoVoyage.ViewModel;

namespace VinoVoyage.Controllers
{
    public class CustomerController : Controller
    {
        private VinoVoyageDb db = new VinoVoyageDb();
        // GET: Customer
        public ActionResult CustomerHomeView()
        {
            var user = db.Users.Find("shanik");
            Session["userinfo"] = user as UserModel;
            //var user = Session["userinfo"] as UserModel;
            UserViewModel uvm = new UserViewModel();
            uvm.user = user;
            uvm.users = db.Users.ToList<UserModel>();
            uvm.products = db.Products.ToList<ProductModel>();
            uvm.cart = new List<OrderModel>();       
            if (user != null)
            {
                ViewBag.Username = user.Username;
                uvm.cart = db.Orders.Where(order => order.Username == user.Username).ToList();

            }
            Session["userCart"] = uvm.cart;
            CalcCartTotal(uvm.cart);
            return View("CustomerHomeView", uvm);
        }

        public ActionResult CheckoutView()
        {
            var user = db.Users.Find("shanik");
            Session["userinfo"] = user as UserModel;
            UserViewModel uvm = new UserViewModel();
            uvm.user = user;
            uvm.users = db.Users.ToList<UserModel>();
            uvm.products = db.Products.ToList<ProductModel>();
            uvm.cart = new List<OrderModel>();
            if (user != null)
            {
                ViewBag.Username = user.Username;
                uvm.cart = db.Orders.Where(order => order.Username == user.Username).ToList();
            }
            Session["userCart"] = uvm.cart;
            CalcCartTotal(uvm.cart);
            return View("CheckoutView", uvm);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("HomePage", "Home");
        }

        public ActionResult Checkout()
        {

            UserViewModel uvm = TempData["UserVieModel"] as UserViewModel;
            if (uvm != null)
            {
                if (uvm.cart.Count > 0)
                {
                    return View("CheckoutView", uvm);
                }
            }
            return View("CustomerHomeView");
        }

        [HttpPost]
        public ActionResult AddToCart(int prodId)
        {
            // find product in db and change amount
            ProductModel stockItems = db.Products.Find(prodId);
            if (stockItems == null)
            {
                return HttpNotFound();
            }
            stockItems.Amount -= 1;
            db.SaveChanges();

            // update cart total
            var cartTotal = (int)Session["cartTotal"];
            if (stockItems.NewPrice != 0)
            {
                cartTotal += stockItems.NewPrice;
            }
            else
            {
                cartTotal += stockItems.Price;
            }
            Session["cartTotal"] = cartTotal;

            // get user info
            var tempCart = Session["userCart"] as List<OrderModel>;
            var user = Session["userinfo"] as UserModel;

            // if user has cart
            if (tempCart.Count > 0)
            {
                // check if product in the cart
                bool containsProduct = tempCart.Any(item => item.ProductID == prodId);
                if (containsProduct) 
                {  
                    // if so, incrase quantity of order in db and in cart
                    var existingOrder = db.Orders.FirstOrDefault(item => item.Username == user.Username && item.ProductID == prodId);
                    if (existingOrder != null)
                    {
                        existingOrder.Quantity += 1;
                        db.SaveChanges();
                        var temp = tempCart.FirstOrDefault(item => item.ProductID == prodId);
                        temp.Quantity += 1;
                        Session["userCart"] = tempCart;
                        return Json(new { success = true, message = "Product added to cart successfully.", newQuantity = temp.Quantity, prod = stockItems, total = Session["cartTotal"] });
                    }
                }
            }
            // if cart is empty or product is not in the cart

            // create a new order
            OrderModel newOrder = new OrderModel();
            newOrder.ProductID = prodId;
            newOrder.Username = user.Username;
            newOrder.Quantity = 1;
                
            // add to cart
            tempCart.Add(newOrder);

            // add to db
            db.Orders.Add(newOrder);
            db.SaveChanges();
            Session["userCart"] = tempCart;
            return Json(new { success = true, message = "Product added to cart successfully.", newQuantity = 1, prod = stockItems, total = Session["cartTotal"] });
        }

        [HttpPost]
        public ActionResult DeleteFromCart(int prodId)
        {
            // find product in db and change amount
            ProductModel stockItems = db.Products.Find(prodId);
            if (stockItems == null)
            {
                return HttpNotFound();
            }
            stockItems.Amount += 1;
            db.SaveChanges();

            // update cart total
            var cartTotal = (int)Session["cartTotal"];
            if (stockItems.NewPrice != 0)
            {
                cartTotal -= stockItems.NewPrice;
            }
            else
            {
                cartTotal -= stockItems.Price;
            }
            Session["cartTotal"] = cartTotal;

            // get user info
            var tempCart = Session["userCart"] as List<OrderModel>;
            var user = Session["userinfo"] as UserModel;
            OrderModel cartOrder = tempCart.FirstOrDefault(order => order.ProductID == prodId);
            var dbOrder = db.Orders.FirstOrDefault(item => item.Username == user.Username && item.ProductID == prodId);
            //var dbOrder = db.Orders.FirstOrDefault(item => item.Username == user.Username && item.ProductID == prodId);



            // if user has more then 1 product
            if (cartOrder.Quantity > 1)
            {
                cartOrder.Quantity -= 1;
                dbOrder.Quantity -= 1;
                db.SaveChanges();
                Session["userCart"] = tempCart;
                return Json(new { success = true, message = "Product added to cart successfully.", newQuantity = cartOrder.Quantity, prod = stockItems, total = Session["cartTotal"] });
            }

            else
            {

                tempCart.RemoveAll(order => order.ProductID == cartOrder.ProductID);
                db.Orders.Remove(dbOrder);
                db.SaveChanges();
                Session["userCart"] = tempCart;
                return Json(new { success = true, message = "Product added to cart successfully.", newQuantity = 0, prod = stockItems, total = Session["cartTotal"] });
            }

        }
        private void CalcCartTotal(List<OrderModel> userCart)
        {
            int total = 0;
            if (userCart.Count > 0)
            {
                foreach (OrderModel order in userCart)
                {
                    var p = db.Products.Find(order.ProductID);
                    var price = p.Price;
                    if (p.NewPrice != 0)
                    {
                        price = p.NewPrice;
                    }
                    var amount = order.Quantity;
                    total += price * amount;
                }  
            }
            Session["cartTotal"] = total;
        }

    }
}