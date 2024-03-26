using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VinoVoyage.Models;

namespace VinoVoyage.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult CustomerHomeView()
        {
            var user = Session["userinfo"] as UserModel;
            if (user != null)
            {
                ViewBag.Username = user.Username;
                System.Console.WriteLine(ViewBag.Username);
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