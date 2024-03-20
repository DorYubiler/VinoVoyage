using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VinoVoyage.Models;

namespace VinoVoyage.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult LoginView()
        {
            return View();
        }
    }


}