using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
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
        public ActionResult CustomerHomeView(UserModel user)
        {
            
    
            Session["userinfo"] = user as UserModel;       
             
            UserViewModel uvm = new UserViewModel();
            uvm.user = user;
            uvm.users = db.Users.ToList<UserModel>();
            uvm.products = db.Products.ToList<ProductModel>();
            uvm.cart = new List<OrderModel>();
            uvm.wishList = new List<WishListModel>();
            uvm.shipping = new List<ShippingModel>();
            
            if (user != null)
            {
                ViewBag.Username = user.Username;
                uvm.cart = db.Orders.Where(order => order.Username == user.Username).ToList();
                uvm.wishList = db.wishList.Where(item => item.Username == user.Username).ToList();
                uvm.shipping=db.ShippingList.Where(item=>item.UserName == user.Username).ToList();
                
            }

            Session["userCart"] = uvm.cart;
            Session["userList"] = uvm.wishList;
            CalcCartTotal(uvm.cart);
            return View("CustomerHomeView", uvm);
        }


        [HttpPost]
        public JsonResult Buynow(String CityAddress)
        {
            ShippingModel newShipping = new ShippingModel();
            var user = Session["userinfo"] as UserModel;
            DateTime today = DateTime.Today;
            DateTime shipDay = today.AddDays(14);
            newShipping.UserName = user.Username;
            newShipping.OrderDate = today;
            newShipping.ShippingDate = shipDay;
            newShipping.Address = CityAddress;

            db.ShippingList.Add(newShipping);
            db.SaveChanges();
            if (user.Username.Contains("guest"))
            {
                return Json(new { success = true, redirectUrl = Url.Action("Logout", "Customer", user) });
            }
            return Json(new { success = true, redirectUrl = Url.Action("CustomerHomeView", "Customer", user) });

        }



        [HttpPost]
        public JsonResult Payment(String cityAddress)
        {
            ShippingModel newShipping = new ShippingModel();
            var user = Session["userinfo"] as UserModel;
            DateTime today = DateTime.Today;
            DateTime shipDay = today.AddDays(14);
            newShipping.UserName = user.Username;
            newShipping.OrderDate = today;
            newShipping.ShippingDate = shipDay;
            newShipping.Address = cityAddress;

            db.ShippingList.Add(newShipping);
            db.SaveChanges();

            var allorders = Session["userCart"] as List<OrderModel>;

            foreach (OrderModel order in allorders)
            {
                // delete from orders db
                var orderToDelete = db.Orders.FirstOrDefault(o => o.ProductID == order.ProductID && o.Username == order.Username);
                db.Orders.Remove(orderToDelete);
                db.SaveChanges();
            }

            // reset user's cart session
            Session["cartTotal"] = 0;
            allorders.Clear();
            Session["userCart"] = allorders;
            if (user.Username.Contains("guest"))
            {
                return Json(new { success = true, redirectUrl = Url.Action("Logout", "Customer", user) });
            }
            return Json(new { success = true, redirectUrl = Url.Action("CustomerHomeView", "Customer",user) });
        }
        public ActionResult CheckoutView(UserModel user)
        {
            /*var user = db.Users.Find("shanik");
            Session["userinfo"] = user as UserModel;*/
            Session["userinfo"]=user as UserModel;
            UserViewModel uvm = new UserViewModel();
            uvm.user = user;
            uvm.users = db.Users.ToList<UserModel>();
            uvm.products = db.Products.ToList<ProductModel>();
            uvm.cart = new List<OrderModel>();
            if (user != null)
            {
                ViewBag.Username = user.Username;
                uvm.cart = db.Orders.Where(order => order.Username == user.Username).ToList();
                uvm.wishList = db.wishList.Where(item => item.Username== user.Username).ToList();
            }
            Session["userCart"] = uvm.cart;
            Session["userList"] = uvm.wishList;
            CalcCartTotal(uvm.cart);
            return View("CheckoutView", uvm);
        }

        public ActionResult Logout()
        {
            
            Session.Clear();
            List<UserModel> users = db.Users.Where(u => u.Username.Contains("guest")).ToList();
            List<WishListModel> wishListModels = db.wishList.Where(i => i.Username.Contains("guest")).ToList();
            db.Users.RemoveRange(users);
            db.wishList.RemoveRange(wishListModels);
            db.SaveChanges();
            return RedirectToAction("HomePage", "User");

            

        }

        public ActionResult Checkout()
        {

            UserViewModel uvm = TempData["UserVieModel"] as UserViewModel;
            if (uvm != null)
            {
                if (uvm.cart.Count > 0)
                {
                    return View("CheckoutView", uvm);
                }
            }
            return View("CustomerHomeView");
        }

        public ActionResult AddToList(int prodId)
        {
            ProductModel stockItems = db.Products.FirstOrDefault(p => p.ProductID == prodId);
            var tempList = Session["userList"] as List<WishListModel>;
            var user = Session["userinfo"] as UserModel;
            bool containsProduct = tempList.Any(item => item.ProductID == prodId);
            // if user has cart
            if ((!containsProduct))
            {
                WishListModel newItem = new WishListModel();
                newItem.ProductID = prodId;
                newItem.Username = user.Username;
                tempList.Add(newItem);
                db.wishList.Add(newItem);
                db.SaveChanges();
                return Json(new { success = true, message = "Product added to cart successfully.", prod = stockItems });
            }
            Session["userList"] = tempList;
            return Json(new { success = false, message = "Product already in cart.", prod = stockItems});
        }
        [HttpPost]
        public ActionResult AddToCart(int prodId)
        {
            // find product in db and change amount
            ProductModel stockItems = db.Products.FirstOrDefault(p => p.ProductID == prodId);
            if (stockItems == null)
            {
                return HttpNotFound();
            }
            stockItems.Amount -= 1;
            db.SaveChanges();

            // update cart total
            var cartTotal = (int)Session["cartTotal"];
            if (stockItems.NewPrice != 0)
            {
                cartTotal += stockItems.NewPrice;
            }
            else
            {
                cartTotal += stockItems.Price;
            }
            Session["cartTotal"] = cartTotal;

            // get user info
            var tempCart = Session["userCart"] as List<OrderModel>;
            var user = Session["userinfo"] as UserModel;

            // if user has cart
            if (tempCart.Count > 0)
            {
                // check if product in the cart
                bool containsProduct = tempCart.Any(item => item.ProductID == prodId);
                if (containsProduct)
                {
                    // if so, incrase quantity of order in db and in cart
                    var existingOrder = db.Orders.FirstOrDefault(item => item.Username == user.Username && item.ProductID == prodId);
                    if (existingOrder != null)
                    {
                        existingOrder.Quantity += 1;
                        db.SaveChanges();
                        var temp = tempCart.FirstOrDefault(item => item.ProductID == prodId);
                        temp.Quantity += 1;
                        Session["userCart"] = tempCart;
                        return Json(new { success = true, message = "Product added to cart successfully.", newQuantity = temp.Quantity, prod = stockItems, total = Session["cartTotal"] });
                    }
                }
            }
            // if cart is empty or product is not in the cart

            // create a new order
            OrderModel newOrder = new OrderModel();
            newOrder.ProductID = prodId;
            newOrder.Username = user.Username;
            newOrder.Quantity = 1;

            // add to cart
            tempCart.Add(newOrder);

            // add to db
            db.Orders.Add(newOrder);
            db.SaveChanges();
            Session["userCart"] = tempCart;
            return Json(new { success = true, message = "Product added to cart successfully.", newQuantity = 1, prod = stockItems, total = Session["cartTotal"] });
        }

        [HttpPost]
        public ActionResult EmptyList()
        {
            // get user info
            var user = Session["userinfo"] as UserModel;
            var wishlist = Session["userList"] as List<WishListModel>;
            var userWishList = db.wishList.Where(i => i.Username == user.Username).ToList();
            foreach (WishListModel item in userWishList)
            {
                var dbItem = db.wishList.FirstOrDefault(i => i.Username == user.Username && i.ProductID == item.ProductID);
                if (dbItem != null)
                {
                    db.wishList.Remove(dbItem);
                    db.SaveChanges();
                }
            }


            // reset user's cart session
            wishlist.Clear();
            Session["userList"] = wishlist;
            return Json(new { success = true, message = "All product deleted from list successfully." });
        }

        [HttpPost]
        public ActionResult EmptyCart()
        {
            var allorders = Session["userCart"] as List<OrderModel>;

            foreach (OrderModel order in allorders)
            {
                // add product to stock
                ProductModel stockItems = db.Products.FirstOrDefault(p => p.ProductID == order.ProductID);
                if (stockItems == null)
                {
                    return HttpNotFound();
                }
                stockItems.Amount += order.Quantity;
                db.SaveChanges();

                // delete from orders db
                var orderToDelete = db.Orders.FirstOrDefault(o => o.ProductID == order.ProductID && o.Username == order.Username);
                db.Orders.Remove(orderToDelete);
                db.SaveChanges();
            }

            // reset user's cart session
            Session["cartTotal"] = 0;
            allorders.Clear();
            Session["userCart"] = allorders;

            return Json(new { success = true, message = "cart is empty" });
        }
        [HttpPost]
        public ActionResult DeleteFromList(int prodId)
        {
            // get user info
            var user = Session["userinfo"] as UserModel;
            var wishlist = Session["userList"] as List<WishListModel>;
            
            var itemToRemove = wishlist.FirstOrDefault(i => i.ProductID == prodId);
            if (itemToRemove != null) {
                wishlist.Remove(itemToRemove);
                var dbItem = db.wishList.FirstOrDefault(item => item.Username == user.Username && item.ProductID == prodId);
                if (dbItem != null)
                {
                    db.wishList.Remove(dbItem);
                    db.SaveChanges();
                }
                Session["userList"] = wishlist;
                return Json(new { success = true, message = "Product deleted from list successfully." });
            }                     
            return Json(new { success = true, message = "Product deleted from list successfully." });
        }

        [HttpPost]
        public ActionResult ReturnNewToCart()
        {
            var user = Session["userinfo"] as UserModel;
            var wishlist = Session["userList"] as List<WishListModel>;
            var cart = Session["userCart"] as List<OrderModel>;
            var itemsAdded = new List<WishListModel>(); 

            foreach (WishListModel wishItem in wishlist)
            {
                bool containsProduct = cart.Any(i => i.ProductID == wishItem.ProductID);
                if (!containsProduct)
                {
                    itemsAdded.Add(wishItem);
                }
            }
            if (itemsAdded.Count > 0)
            {
                return Json(new { success = true, message = "Products added to cart successfully.", newItems = itemsAdded });
            }
            return Json(new { success = false, message = "Products added to cart successfully.", newItems = itemsAdded });
        }
        [HttpPost]
        public ActionResult DeleteFromCart(int prodId)
        {
            // find product in db and change amount
            ProductModel stockItems = db.Products.FirstOrDefault(p => p.ProductID == prodId);
            if (stockItems == null)
            {
                return HttpNotFound();
            }
            stockItems.Amount += 1;
            db.SaveChanges();

            // update cart total
            var cartTotal = (int)Session["cartTotal"];
            if (stockItems.NewPrice != 0)
            {
                cartTotal -= stockItems.NewPrice;
            }
            else
            {
                cartTotal -= stockItems.Price;
            }
            Session["cartTotal"] = cartTotal;

            // get user info
            var tempCart = Session["userCart"] as List<OrderModel>;
            var user = Session["userinfo"] as UserModel;
            OrderModel cartOrder = tempCart.FirstOrDefault(order => order.ProductID == prodId);
            var dbOrder = db.Orders.FirstOrDefault(item => item.Username == user.Username && item.ProductID == prodId);
            //var dbOrder = db.Orders.FirstOrDefault(item => item.Username == user.Username && item.ProductID == prodId);



            // if user has more then 1 product
            if (cartOrder.Quantity > 1)
            {
                cartOrder.Quantity -= 1;
                dbOrder.Quantity -= 1;
                db.SaveChanges();
                Session["userCart"] = tempCart;
                return Json(new { success = true, message = "Product deleted to cart successfully.", newQuantity = cartOrder.Quantity, prod = stockItems, total = Session["cartTotal"] });
            }

            else
            {

                tempCart.RemoveAll(order => order.ProductID == cartOrder.ProductID);
                db.Orders.Remove(dbOrder);
                db.SaveChanges();
                Session["userCart"] = tempCart;
                return Json(new { success = true, message = "Product deleted to cart successfully.", newQuantity = 0, prod = stockItems, total = Session["cartTotal"] });
            }

        }
        private void CalcCartTotal(List<OrderModel> userCart)
        {
            int total = 0;
            if (userCart.Count > 0)
            {
                foreach (OrderModel order in userCart)
                {
                    var prod = db.Products.FirstOrDefault(p => p.ProductID == order.ProductID);
                    //var p = db.Products.FirstOrDefaultAsync(product => product.ProductID == order.ProductID);
                    var price = prod.Price;
                    if (prod.NewPrice != 0)
                    {
                        price = prod.NewPrice;
                    }
                    var amount = order.Quantity;
                    total += price * amount;
                }
            }
            Session["cartTotal"] = total;
        }

        public ActionResult SortProducts(string sortBy ,string wineColor)
        {
            wineColor = (wineColor.Substring(0, wineColor.Length - 1)).ToLower(); 
            var products = db.Products.Where(p => p.Type.ToString() == wineColor).AsQueryable();
            if (wineColor == "allwine")
            {
                products = db.Products.AsQueryable();

            }
            if(wineColor == "sale")
            {
                products = db.Products.Where(p=>p.NewPrice!=0).AsQueryable();
            }

            switch (sortBy)
            {
                case "PLH":
                    products = products.OrderBy(p => p.NewPrice != 0 ? p.NewPrice : p.Price);
                    break;
                case "PHL":
                    products = products.OrderByDescending(p => p.NewPrice != 0 ? p.NewPrice : p.Price);
                    break;
                case "popular":
                    products = products.OrderByDescending(p => p.Rating);
                    break;
            }
            return PartialView("_ProductsGrid", products);
        }

        public ActionResult SearchProducts(string query)
        {
            // Simulate fetching filtered products from database
            // Replace this with your actual database query using Entity Framework, Dapper, or another ORM
            var filteredProducts = db.Products.Where(p=> p.Type.ToString().Contains(query) || p.Winery.ToString().Contains(query) || p.ProductName.ToString().Contains(query) || p.Origin.ToString().Contains(query) || p.Description.ToString().Contains(query));
            var user = Session["userinfo"] as UserModel;
            if (user.Username.Contains("guest"))
            {
                // Return the Partial View with the filtered products
                return PartialView("_GuestGrid", filteredProducts);
            }
            return PartialView("_ProductsGrid", filteredProducts);
        }

        public JsonResult UpdateInfo(String username,String password,String email)
        {
            UserModel model = db.Users.Find(username);
            if(model != null)
            {
                model.Password = password;
                model.Email = email;
                db.SaveChanges();
                return Json(new { success = true });

            }
            return Json(new { success = false });

        }
    }
}