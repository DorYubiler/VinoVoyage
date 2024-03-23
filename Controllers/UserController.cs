using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VinoVoyage.Models;
using VinoVoyage.Dal;

namespace VinoVoyage.Controllers
{
    public class UserController : Controller
    {
        public ActionResult LoginView()
        {
            return View();
        }

        //public ActionResult Login(string Uname,string pass)
        //{
        //    using (var context=new YourDbContext())
        //    {
        //        var user=context.Users.FirstOrDefault(u=>u.UserName==user);
        //    }
        //}
        // GET: Customer
        public ActionResult RegisterView()
        {
            return View();
        }
        [HttpPost]
        // השתמשנו בדף לוגין רק כדי לבדוק שנכנס לדטה בייס, לתקן לדף חדש כשהצלחנו 
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
                    FormsAuthentication.SetAuthCookie(model.Username, false);
                    if (user.Role == "customer")
                    {
                        return RedirectToAction("HomePage", "Home");
                    }
                    return RedirectToAction("AdminHomePage", "Home");
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