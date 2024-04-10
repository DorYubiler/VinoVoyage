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

        /// <summary>
        /// Displays the customer home view with their user information, cart, wishlist, and shipping details.
        /// </summary>
        /// <returns>A view populated with the customer's information and product details.</returns>
        public ActionResult CustomerHomeView(/*UserModel user*/)
        {
            var user = Session["userinfo"] as UserModel;   
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

        /// <summary>
        /// Processes a "buy now" action, adding shipping information for the current user's purchase and determining redirection based on user type.
        /// </summary>
        /// <param name="CityAddress">The shipping address for the current purchase.</param>
        /// <returns>A JSON result indicating success and where to redirect the user.</returns>
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


        /// <summary>
        /// Completes the payment process by adding shipping information and clearing the user's cart.
        /// </summary>
        /// <param name="cityAddress">The shipping address for the current purchase.</param>
        /// <returns>A JSON result indicating success and where to redirect the user.</returns>
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

        /// <summary>
        /// Displays the checkout view for the current user, populated with their cart and wishlist.
        /// </summary>
        /// <param name="user">The current user model.</param>
        /// <returns>A view for the checkout process.</returns>
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

        /// <summary>
        /// Logs out the current user, clearing the session and removing guest user records from the database.
        /// </summary>
        /// <returns>A redirection to the home page.</returns>
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

        /// <summary>
        /// Displays the checkout view if there are items in the cart, otherwise redirects to the customer home view.
        /// </summary>
        /// <returns>A checkout view with user and cart information or a redirect to the customer home view.</returns>
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

        /// <summary>
        /// Adds a specified product to the user's wishlist.
        /// </summary>
        /// <param name="prodId">The product ID to add to the wishlist.</param>
        /// <returns>A JSON result indicating success or failure of adding the product to the wishlist.</returns>
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

        /// <summary>
        /// Adds a specified product to the user's cart, decrementing stock and updating the cart total.
        /// </summary>
        /// <param name="prodId">The product ID to add to the cart.</param>
        /// <returns>A JSON result indicating success or failure, along with updated cart information.</returns>
        [HttpPost]
        public ActionResult AddToCart(int prodId)
        {
            // find product in db and change amount
            ProductModel stockItems = db.Products.FirstOrDefault(p => p.ProductID == prodId);
            if (stockItems.Amount > 0)
            {
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
                bool isGuest = user.Username.Contains("guest");
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
                            return Json(new { success = true, message = "Product added to cart successfully.", newQuantity = temp.Quantity, prod = stockItems, total = Session["cartTotal"], guest = isGuest });
                        }
                    }
                }
                
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
                return Json(new { success = true, message = "Product added to cart successfully.", newQuantity = 1, prod = stockItems, total = Session["cartTotal"], guest = isGuest });
            }
            return Json(new { success = false, message = "Product is out of stock.", newQuantity = 0, prod = stockItems, total = Session["cartTotal"] });
        }

        /// <summary>
        /// Clears all items from the user's wishlist.
        /// </summary>
        /// <returns>A JSON result indicating success of clearing the wishlist.</returns>
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

        /// <summary>
        /// Clears all items from the user's cart, restocking items and resetting the cart total.
        /// </summary>
        /// <returns>A JSON result indicating success of emptying the cart.</returns>
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

        /// <summary>
        /// Deletes a specific product from the user's wishlist.
        /// </summary>
        /// <param name="prodId">The product ID to remove from the wishlist.</param>
        /// <returns>A JSON result indicating success of the deletion.</returns>
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

        /// <summary>
        /// Moves items from the wishlist to the cart, updating quantities and stock as needed.
        /// </summary>
        /// <returns>A JSON result indicating the success of adding wishlist items to the cart, along with a list of newly added items.</returns>
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

        /// <summary>
        /// Deletes a specific product from the user's cart, restocking the item and updating the cart total.
        /// </summary>
        /// <param name="prodId">The product ID to remove from the cart.</param>
        /// <returns>A JSON result indicating success of the deletion, along with updated cart information.</returns>
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

        /// <summary>
        /// Calculates the total cost of the user's cart based on product prices and quantities.
        /// </summary>
        /// <param name="userCart">The list of orders in the user's cart.</param>
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

        /// <summary>
        /// Sorts products based on the specified criteria and filters by wine color if provided.
        /// </summary>
        /// <param name="sortBy">The sorting criteria (e.g., price low to high).</param>
        /// <param name="wineColor">The wine color to filter by.</param>
        /// <returns>A partial view with the sorted and filtered products.</returns>
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

        /// Searches for products based on a query string, including product type, winery, name, origin, or description.
        /// </summary>
        /// <param name="query">The search query.</param>
        /// <returns>A partial view with products that match the search criteria.</returns>
        public ActionResult SearchProducts(string query)
        {
            var filteredProducts = db.Products.Where(p=> p.Type.ToString().Contains(query) || p.Winery.ToString().Contains(query) || p.ProductName.ToString().Contains(query) || p.Origin.ToString().Contains(query) || p.Description.ToString().Contains(query));
            var user = Session["userinfo"] as UserModel;
            if (user.Username.Contains("guest"))
            {
                // Return the Partial View with the filtered products
                return PartialView("_GuestGrid", filteredProducts);
            }
            return PartialView("_ProductsGrid", filteredProducts);
        }

        /// <summary>
        /// Updates user information such as password and email, based on provided values.
        /// </summary>
        /// <param name="username">The username of the user to update.</param>
        /// <param name="password">The new password for the user. If empty, the password is not updated.</param>
        /// <param name="email">The new email for the user. If empty, the email is not updated.</param>
        /// <returns>A JSON result indicating the success of the update operation.</returns>
        public JsonResult UpdateInfo(String username,String password,String email)
        {
            UserModel model = db.Users.Find(username);
            if(model != null)
            {
                if (password.Length!=0) { model.Password = password; }
                if(email.Length!=0) { model.Email = email; }
                db.SaveChanges();
                return Json(new { success = true });

            }
            return Json(new { success = false });

        }
    }
}