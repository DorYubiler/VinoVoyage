using System;
using System.Collections.Generic;
using System.Linq;
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
            if (user != null)
            {
                ViewBag.Username = user.Username;
                UserViewModel uvm = new UserViewModel();
                uvm.user = user;
                uvm.users = db.Users.ToList<UserModel>();
                uvm.products = db.Products.ToList<ProductModel>();

                return View("CustomerHomeView", uvm);
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("HomePage", "Home");
        }
/*
        public int Testt()
        {

        }*/
    }
}