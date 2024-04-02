using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VinoVoyage.ViewModel;

namespace VinoVoyage.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult CartView()
        {
            UserViewModel uvm = TempData["UserVieModel"] as UserViewModel;
            return View(uvm);
        }
    }
}