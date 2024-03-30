using System;
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
            var user = Session["userinfo"] as UserModel;
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
            return View("CustomerHomeView", uvm);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("HomePage", "Home");
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
                        return Json(new { success = true, message = "Product added to cart successfully." });
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
            return Json(new { success = true, message = "Product added to cart successfully." });
        }

    }
}