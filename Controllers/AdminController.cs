using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VinoVoyage.Dal;
using VinoVoyage.Models;
using VinoVoyage.ViewModel;

namespace VinoVoyage.Controllers
{
    public class AdminController : Controller
    {
        private VinoVoyageDb db = new VinoVoyageDb();




        /// <summary>
        /// Displays the admin home page with user and product information.
        /// Retrieves the current user from the session, lists all users excluding the current admin, and displays all products.
        /// </summary>
        /// <returns>A view populated with user and product data for the admin.</returns>

        public ActionResult AdminHomePage()
        {
            var user = Session["userinfo"] as UserModel;
            UserViewModel uvm = new UserViewModel();
            UserViewModel uvmA = new UserViewModel();
            uvmA.users = db.Users.ToList<UserModel>();
            uvm.user = user;
            List<UserModel> Users = new List<UserModel>();
            if (user != null)
            {              
                for(int i = 0; i < uvmA.users.Count; i++)
                {
                    if (uvmA.users[i].Username != user.Username)
                    {
                        Users.Add(uvmA.users[i]);
                    }
                }
                uvm.users = Users;
                uvm.products=db.Products.ToList<ProductModel>();
                uvm.shipping = db.ShippingList.ToList<ShippingModel>();
                ViewBag.Username = user.Username;
                return View(uvm);
            }
            return View();
        }

        /// <summary>
        /// Updates an existing user's information in the database.
        /// </summary>
        /// <param name="model">The user model containing updated information.</param>
        /// <returns>A JSON object indicating the success or failure of the update operation.</returns>
        [HttpPost]
        public ActionResult UpdateUser(UserModel model)
        {
            try
            {
                UserModel userUpdate= db.Users.Find(model.Username);
                if (userUpdate != null) {
                    userUpdate.Password = model.Password;
                    userUpdate.Email = model.Email;
                    userUpdate.Role = model.Role;
                    db.SaveChanges();
                }
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        /// <summary>
        /// Deletes a user from the database based on their username.
        /// </summary>
        /// <param name="usern">The username of the user to delete.</param>
        /// <returns>A JSON object indicating the success or failure of the deletion.</returns>

        [HttpPost]
        public ActionResult DeleteUser(string usern)
        {
            try
            {
                UserModel userModel = db.Users.Find(usern);
                db.Users.Remove(userModel);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        /// <summary>
        /// Adds a new user to the database if they do not already exist.
        /// </summary>
        /// <param name="user">The user model to add to the database.</param>
        /// <returns>A JSON object indicating the success or failure of the add operation.</returns>
        [HttpPost]
        public JsonResult AddUser(UserModel user)
        {
            if (db.Users.Find(user.Username) == null)
            {
                if (ModelState.IsValid)
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return Json(new { success = false });
            }
            return Json(new { success = false });
        }

        /// <summary>
        /// Deletes a product from the database based on its ID and name, along with associated orders and wishlist items.
        /// </summary>
        /// <param name="prod">The ID of the product to delete.</param>
        /// <param name="prodn">The name of the product to delete.</param>
        /// <returns>A JSON object indicating the success or failure of the deletion.</returns>
        [HttpPost]
        public ActionResult DeleteProduct(string prod, string prodn)
        {
            try
            {
                ProductModel product = db.Products.Find(int.Parse(prod),prodn);
                var tempCart = db.Orders.Where(order => order.ProductID == product.ProductID).ToList();
                var wishlist = db.wishList.Where(item => item.ProductID == product.ProductID).ToList();
                if (tempCart.Any())
                {
                    db.Orders.RemoveRange(tempCart);
                }
                if (wishlist.Any())
                {
                    db.wishList.RemoveRange(wishlist);
                }
                db.Products.Remove(product);              
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        /// <summary>
        /// Updates an existing product's information in the database.
        /// </summary>
        /// <param name="pmodel">The product model containing updated information.</param>
        /// <returns>A JSON object indicating the success or failure of the update operation.</returns>

        [HttpPost]
        public ActionResult UpdateProduct(ProductModel pmodel)
        {
            try
            {
                ProductModel prodUpdate = db.Products.Find(pmodel.ProductID,pmodel.ProductName);
                if (prodUpdate != null)
                {
                    prodUpdate.Winery=pmodel.Winery;
                    prodUpdate.Type = pmodel.Type;
                    prodUpdate.Description = pmodel.Description;
                    prodUpdate.Origin = pmodel.Origin;
                    prodUpdate.Amount = pmodel.Amount;
                    prodUpdate.Price = pmodel.Price;
                    prodUpdate.NewPrice = pmodel.NewPrice; 
                    db.SaveChanges();
                }
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        /// <summary>
        /// Adds a new product to the database if a product with the same name does not already exist.
        /// If a product image is provided, it is saved to a specified directory.
        /// </summary>
        /// <param name="ProductName">Name of the product.</param>
        /// <param name="Winery">Winery of the product.</param>
        /// <param name="Type">Type of the product.</param>
        /// <param name="Description">Description of the product.</param>
        /// <param name="Origin">Origin of the product.</param>
        /// <param name="Amount">Amount in stock.</param>
        /// <param name="Price">Price of the product.</param>
        /// <param name="NewPrice">Discounted price of the product, if applicable.</param>
        /// <param name="ProductImage">The uploaded image file for the product.</param>
        /// <returns>A JSON object indicating the success or failure of the add operation.</returns>
        [HttpPost]
        public JsonResult AddProduct(string ProductName,string Winery, string Type, string Description, string Origin, int Amount, int Price, int NewPrice, HttpPostedFileBase ProductImage)
        {
            if (db.Products.FirstOrDefault(p=>p.ProductName==ProductName)==null) {
                ProductModel product = new ProductModel();
                product.ProductName = ProductName;
                product.Winery = Winery;
                product.Type = Type;
                product.Description = Description;
                product.Origin = Origin;
                product.Amount = Amount;
                product.Price = Price;
                product.NewPrice = NewPrice;
                db.Products.Add(product);
                db.SaveChanges();
                if (ProductImage != null && ProductImage.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(ProductImage.FileName);
                    var newFileName = ProductName + product.ProductID.ToString()+".jpg";
                    var directoryPath = Server.MapPath("~/UI/img/wines/"); 
                    var fullPath = Path.Combine(directoryPath, newFileName);
                    ProductImage.SaveAs(fullPath);
                    return Json(new { success = true });
                }
                return Json(new { success = false });
            }
            return Json(new { success = false });
        }

        /// <summary>
        /// Disposes of the database context resources if disposing is true.
        /// </summary>
        /// <param name="disposing">Indicates whether the method call comes from a Dispose method (its value is true) or from a finalizer (its value is false).</param>

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("HomePage", "User");
        }


    }
}
