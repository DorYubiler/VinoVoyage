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
       


        // GET: Admin
        public ActionResult AdminHomePage(UserModel user)
        {
            
            Session["userinfo"]=user as UserModel;
            UserViewModel uvm = new UserViewModel();
            //creating new uvm, copy all of the users and removing the current admin
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
            /*    
            db.Users.Add(user);
            db.SaveChanges();
            return RedirectToAction("AdminHomePage");*/
        }


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
                    var directoryPath = Server.MapPath("~/UI/img/wines/"); // For ASP.NET MVC
                    var fullPath = Path.Combine(directoryPath, newFileName);

                    // Save the file to the new path
                    ProductImage.SaveAs(fullPath);
                    return Json(new { success = true });
                }
                return Json(new { success = false });
            }
            return Json(new { success = false });
        }
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
