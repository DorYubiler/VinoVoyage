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
        public JsonResult SignUp(String username, String password, String email)
        {   UserModel user = new UserModel();
            user.Username = username;
            user.Password = password;
            user.Email = email;
            if (db.Users.Find(username) == null)
            {
                if (ModelState.IsValid)
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                    return Json(new { success = true, redirectUrl = Url.Action("CustomerHomeView", "Customer",user) });

                }
                return Json(new { success = false, errorMsg = "invalid password" });
            }
            return Json(new { success = false, errorMsg = "username is alreday taken" });
        }


         
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
                    Session["userinfo"] = user as UserModel;
                    // Return success and a redirect URL.
                    if (user.Role == "customer") {
                        
                        return Json(new { success = true, redirectUrl = Url.Action("CustomerHomeView", "Customer"/*,user*/) });
                    }
                    return Json(new { success = true, redirectUrl = Url.Action("AdminHomePage", "Admin"/*,user*/) });

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

        public ActionResult LoginGuest()
        {
            UserModel model= new UserModel();
            Random rnd= new Random();
            int num = rnd.Next(1,500);
            model.Username = "guest"+num.ToString();
            model.Password = "123456";
            model.Email = "guest@guest.com";
            Session["userinfo"]=model;
            return RedirectToAction("CustomerHomeView", "Customer", model);
            
        }
    }

}