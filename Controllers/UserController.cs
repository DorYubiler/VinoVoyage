using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VinoVoyage.Models;
using VinoVoyage.Dal;
using System.Data.Entity;


namespace VinoVoyage.Controllers
{
    public class UserController : Controller
    {
        
        public ActionResult LoginView()
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

            if (ModelState.IsValid)
            {
                CustomerDal dal = new CustomerDal();
                dal.Users.Add(user);
                dal.SaveChanges();
                return View("RegisterView", user);
            }
            return View("RegisterView", user);
        }

        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                CustomerDal dal = new CustomerDal();
                UserModel user = dal.Users.FirstOrDefault(u => u.Username.ToString() == model.Username && u.Password.ToString() == model.Password);
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
                }
            }
            return View();
        }

    }
}