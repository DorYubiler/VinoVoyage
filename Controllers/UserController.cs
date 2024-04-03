using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VinoVoyage.Models;
using VinoVoyage.ViewModel;
using VinoVoyage.Dal;
using System.Data.Entity;
using System.Runtime.Remoting.Messaging;


namespace VinoVoyage.Controllers
{
    public class UserController : Controller
    {/*creating new private db, We will use it for any operation with the database*/
        private VinoVoyageDb db=new VinoVoyageDb();

        //public ActionResult LoginView()
        //{
        //    return View();
        //}
        public ActionResult HomePage()
        {

            return View();
        }

        public ActionResult RegisterView()
        {
            return View();
        }
        [HttpPost]
 
        public ActionResult SignUp(UserModel user)
        {
            /* change the customerDal to db (importent!!! I created in line 16 db from VinoVoyageDb. we use it in every use of Db.)*/
            if (ModelState.IsValid)
            {

                db.Users.Add(user);
                db.SaveChanges();
                return View("RegisterView", user);
            }
            return View("RegisterView", user);
        }

        /* public ActionResult Login(LoginModel model)
         {*//* change the customerDal to db (importent!!! I created in line 16 db from VinoVoyageDb. we use it in every use of Db.) *//*
             if (ModelState.IsValid)
             {

                 UserModel user = db.Users.FirstOrDefault(u => u.Username.ToString() == model.Username && u.Password.ToString() == model.Password);

                 if (user != null)
                 {
                     Session["userinfo"] = user;

                     FormsAuthentication.SetAuthCookie(model.Username, false);
                     if (user.Role == "customer")
                     {
                         return RedirectToAction("CustomerHomeView", "Customer");
                     }
                     return RedirectToAction("AdminHomePage", "Admin");
                 }
                 else
                 {
                     ModelState.AddModelError("", "Inavalid username or password");

                     return View("HomePage");
                 }
             }
             return View();
         }*/
        [HttpPost]
        public JsonResult Login(String username, String password) 
        {
            LoginModel model= new LoginModel();
            model.Username = username;
            model.Password = password;
            if (ModelState.IsValid)
            {
                UserModel user = db.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);

                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Username, false);
                    // Return success and a redirect URL.
                    if (user.Role == "customer") {
                        return Json(new { success = true, redirectUrl = Url.Action("CustomerHomeView", "Customer") });
                    }
                    return Json(new { success = true, redirectUrl = Url.Action("AdminHomePage", "Admin") });

                }
                else
                {
                    // Return failure.
                    return Json(new { success = false });
                }
            }
            // In case of model state invalid.
            return Json(new { success = false });
        }




    }
}