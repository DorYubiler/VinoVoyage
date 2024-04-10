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
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VinoVoyageDb"/> class to interact with the database.
        /// </summary>
        private VinoVoyageDb db=new VinoVoyageDb();

        /// <summary>
        /// Displays the home page view.
        /// </summary>
        /// <returns>The home page view.</returns>
        public ActionResult HomePage()
        {
            return View();
        }

        /// <summary>
        /// Attempts to sign up a new user with the provided username, password, and email.
        /// Validates the request, checks for uniqueness of the username, and adds the user to the database if successful.
        /// </summary>
        /// <param name="username">The desired username of the new user.</param>
        /// <param name="password">The password for the new user.</param>
        /// <param name="email">The email address for the new user.</param>
        /// <returns>A JSON result indicating whether the sign-up was successful and, if so, the URL to redirect to.</returns>
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
                    Session["userinfo"] = user as UserModel;
                    db.SaveChanges();
                    return Json(new { success = true, redirectUrl = Url.Action("CustomerHomeView", "Customer",user) });
                }
                return Json(new { success = false, errorMsg = "invalid password" });
            }
            return Json(new { success = false, errorMsg = "username is alreday taken" });
        }

        /// <summary>
        /// Attempts to log in a user with the specified username and password.
        /// Validates the request and sets an authentication cookie if the user is found in the database.
        /// </summary>
        /// <param name="username">The username of the user attempting to log in.</param>
        /// <param name="password">The password of the user attempting to log in.</param>
        /// <returns>A JSON result indicating whether the login was successful and, if so, the URL to redirect to.</returns>
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
                    if (user.Role == "customer") {
                        return Json(new { success = true, redirectUrl = Url.Action("CustomerHomeView", "Customer") });
                    }
                    return Json(new { success = true, redirectUrl = Url.Action("AdminHomePage", "Admin") });
                }
                else
                {
                    return Json(new { success = false });
                }
            }
            return Json(new { success = false });
        }

        /// <summary>
        /// Logs in a guest user by creating a temporary user model and setting it into the session.
        /// Guest users are assigned a random username and a default password and email.
        /// </summary>
        /// <returns>A redirection to the customer home view for the guest user.</returns>
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