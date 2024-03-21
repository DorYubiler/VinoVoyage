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
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult LoginView()
        {
            return View();
        }
        [HttpPost]
        // השתמשנו בדף לוגין רק כדי לבדוק שנכנס לדטה בייס, לתקן לדף חדש כשהצלחנו 
        public ActionResult SignUp(UserModel user)
        {
            System.Console.WriteLine("hello");
            if (ModelState.IsValid)
            {
                CustomerDal dal = new CustomerDal();
                dal.Users.Add(user);
                dal.SaveChanges();
                return View("LoginView", user);
            }
            return View("HomePage", user);
        }

    }
}