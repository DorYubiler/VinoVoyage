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
            System.Console.WriteLine(prodId);

            return Json(new { success = true, message = "Product added to cart successfully." });
        }

    }
}