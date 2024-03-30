using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Data;
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
    public class AdminController : Controller
    {
        private VinoVoyageDb db = new VinoVoyageDb();
       


        // GET: Admin
        public ActionResult AdminHomePage()
        {   
            var user = Session["userinfo"] as UserModel;
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

        
        public ActionResult Create()
        {
            return View();
        }

       

        [HttpPost]
        public ActionResult AddUser(UserModel user)
        {
            db.Users.Add(user);
            db.SaveChanges();
            return RedirectToAction("AdminHomePage");
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

 /*       public ActionResult EditUser(List<string> userd)
        {
            UserModel um= new UserModel();
            
        }    */

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
