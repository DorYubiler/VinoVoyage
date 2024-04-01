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
        public ActionResult AdminHomePage()
        {
            var user = db.Users.Find("dorimon");
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
        public ActionResult AddUser(UserModel user)
        {
            
            db.Users.Add(user);
            db.SaveChanges();
            return RedirectToAction("AdminHomePage");
        }


        [HttpPost]
        public ActionResult DeleteProduct(string prod)
        {
            try
            {
                ProductModel product = db.Products.Find(int.Parse(prod));
                var tempCart = db.Orders.Where(order => order.ProductID == product.ProductID).ToList();
                if (tempCart.Any())
                {
                    db.Orders.RemoveRange(tempCart);
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
        public ActionResult AddProduct(string ProductName, string Type, string Description, string Origin, int Amount, int Price, int NewPrice, HttpPostedFileBase ProductImage)
        {
            ProductModel product=new ProductModel();
            product.ProductName= ProductName;
            product.Type= Type;
            product.Description= Description;
            product.Origin= Origin;
            product.Amount= Amount;
            product.Price= Price;
            product.NewPrice= NewPrice;
            db.Products.Add(product);
            db.SaveChanges();
            if (ProductImage != null && ProductImage.ContentLength > 0)
            {
                var fileName = Path.GetFileName(ProductImage.FileName);
                // Ensure the "img" directory exists in the server path where you want to save the image
                var path = Path.Combine(Server.MapPath("../UI/img/"), fileName);

                // Save the image in the specified path
                ProductImage.SaveAs(path);
            }
            return RedirectToAction("AdminHomePage");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
