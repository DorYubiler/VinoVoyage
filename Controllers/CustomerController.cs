﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
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
            uvm.wishList = new List<WishListModel>();

            if (user != null)
            {
                ViewBag.Username = user.Username;
                uvm.cart = db.Orders.Where(order => order.Username == user.Username).ToList();
                uvm.wishList = db.wishList.Where(item => item.Username == user.Username).ToList();
            }

            Session["userCart"] = uvm.cart;
            Session["userList"] = uvm.wishList;
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
                uvm.wishList = db.wishList.Where(item => item.Username== user.Username).ToList();
            }
            Session["userCart"] = uvm.cart;
            Session["userList"] = uvm.wishList;
            CalcCartTotal(uvm.cart);
            return View("CheckoutView", uvm);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("HomePage", "User");
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

        public ActionResult AddToList(int prodId)
        {
            ProductModel stockItems = db.Products.FirstOrDefault(p => p.ProductID == prodId);
            var tempList = Session["userList"] as List<WishListModel>;
            var user = Session["userinfo"] as UserModel;
            bool containsProduct = tempList.Any(item => item.ProductID == prodId);
            // if user has cart
            if ((!containsProduct))
            {
                WishListModel newItem = new WishListModel();
                newItem.ProductID = prodId;
                newItem.Username = user.Username;
                tempList.Add(newItem);
                db.wishList.Add(newItem);
                db.SaveChanges();
                return Json(new { success = true, message = "Product added to cart successfully.", prod = stockItems });
            }
            Session["userList"] = tempList;
            return Json(new { success = false, message = "Product already in cart.", prod = stockItems});
        }
        [HttpPost]
        public ActionResult AddToCart(int prodId)
        {
            // find product in db and change amount
            ProductModel stockItems = db.Products.FirstOrDefault(p => p.ProductID == prodId);
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
        public ActionResult EmptyList()
        {
            // get user info
            var user = Session["userinfo"] as UserModel;
            var wishlist = Session["userList"] as List<WishListModel>;
            var userWishList = db.wishList.Where(i => i.Username == user.Username).ToList();
            foreach (WishListModel item in userWishList)
            {
                var dbItem = db.wishList.FirstOrDefault(i => i.Username == user.Username && i.ProductID == item.ProductID);
                if (dbItem != null)
                {
                    db.wishList.Remove(dbItem);
                    db.SaveChanges();
                }
            }


            // reset user's cart session
            wishlist.Clear();
            Session["userList"] = wishlist;
            return Json(new { success = true, message = "All product deleted from list successfully." });
        }

        [HttpPost]
        public ActionResult EmptyCart()
        {
            var allorders = Session["userCart"] as List<OrderModel>;
            
            foreach (OrderModel order in allorders)
            {
                // add product to stock
                ProductModel stockItems = db.Products.FirstOrDefault(p => p.ProductID == order.ProductID);
                if (stockItems == null)
                {
                    return HttpNotFound();
                }
                stockItems.Amount += order.Quantity;
                db.SaveChanges();

                // delete from orders db
                var orderToDelete = db.Orders.FirstOrDefault(o => o.ProductID == order.ProductID && o.Username == order.Username);
                db.Orders.Remove(orderToDelete);
                db.SaveChanges();
            }

            // reset user's cart session
            Session["cartTotal"] = 0;
            allorders.Clear();
            Session["userCart"] = allorders; 
            return Json(new { success = true, message = "cart is empty"});
        }

        [HttpPost]
        public ActionResult DeleteFromList(int prodId)
        {
            // get user info
            var user = Session["userinfo"] as UserModel;
            var wishlist = Session["userList"] as List<WishListModel>;
            
            var itemToRemove = wishlist.FirstOrDefault(i => i.ProductID == prodId);
            if (itemToRemove != null) {
                wishlist.Remove(itemToRemove);
                var dbItem = db.wishList.FirstOrDefault(item => item.Username == user.Username && item.ProductID == prodId);
                if (dbItem != null)
                {
                    db.wishList.Remove(dbItem);
                    db.SaveChanges();
                }
                Session["userList"] = wishlist;
                return Json(new { success = true, message = "Product deleted from list successfully." });
            }                     
            return Json(new { success = true, message = "Product deleted from list successfully." });
        }

        [HttpPost]
        public ActionResult ReturnNewToCart()
        {
            var user = Session["userinfo"] as UserModel;
            var wishlist = Session["userList"] as List<WishListModel>;
            var cart = Session["userCart"] as List<OrderModel>;
            var itemsAdded = new List<WishListModel>(); 

            foreach (WishListModel wishItem in wishlist)
            {
                bool containsProduct = cart.Any(i => i.ProductID == wishItem.ProductID);
                if (!containsProduct)
                {
                    itemsAdded.Add(wishItem);
                }
            }
            if (itemsAdded.Count > 0)
            {
                return Json(new { success = true, message = "Products added to cart successfully.", newItems = itemsAdded });
            }
            return Json(new { success = false, message = "Products added to cart successfully.", newItems = itemsAdded });
        }
        [HttpPost]
        public ActionResult DeleteFromCart(int prodId)
        {
            // find product in db and change amount
            ProductModel stockItems = db.Products.FirstOrDefault(p => p.ProductID == prodId);
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
                return Json(new { success = true, message = "Product deleted to cart successfully.", newQuantity = cartOrder.Quantity, prod = stockItems, total = Session["cartTotal"] });
            }

            else
            {

                tempCart.RemoveAll(order => order.ProductID == cartOrder.ProductID);
                db.Orders.Remove(dbOrder);
                db.SaveChanges();
                Session["userCart"] = tempCart;
                return Json(new { success = true, message = "Product deleted to cart successfully.", newQuantity = 0, prod = stockItems, total = Session["cartTotal"] });
            }

        }
        private void CalcCartTotal(List<OrderModel> userCart)
        {
            int total = 0;
            if (userCart.Count > 0)
            {
                foreach (OrderModel order in userCart)
                {
                    var prod = db.Products.FirstOrDefault(p =>  p.ProductID == order.ProductID);
                    //var p = db.Products.FirstOrDefaultAsync(product => product.ProductID == order.ProductID);
                    var price = prod.Price;
                    if (prod.NewPrice != 0)
                    {
                        price = prod.NewPrice;
                    }
                    var amount = order.Quantity;
                    total += price * amount;
                }
            }
            Session["cartTotal"] = total;
        }



    }
}