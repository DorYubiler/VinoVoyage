using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using VinoVoyage.Models;
/* our Db initalizer, in the first time, when the Db is not exist, the initializer, with the seed method, creates new data base.
 we also added in web.config the initializer. after creating one, its not working again*/
namespace VinoVoyage.Dal{
        public class VVInitializer: System.Data.Entity.DropCreateDatabaseIfModelChanges<VinoVoyageDb>
        {
        protected override void Seed(VinoVoyageDb context)
        {
            var users = new List<UserModel>
            {
                new UserModel { Username = "dorimon", Password = "1234567", Role = "admin", Email = "dori199791@gmail.com" },
                new UserModel { Username = "shanik", Password = "1111111", Role = "customer", Email = "shaniriven@gmail.com" },
                new UserModel { Username = "dorimo", Password = "12345678", Role = "customer", Email = "dor199791@gmail.com" },
                new UserModel { Username = "dorim", Password = "123456789", Role = "customer", Email = "dori1997@gmail.com" },
            };
            users.ForEach(s => context.Users.Add(s));
            context.SaveChanges();
            var products = new List<ProductModel>
            {
                new ProductModel {ProductID=1, ProductName="SalomonsDaughter",Type="rose",Description="The primary flavors of SalomonsDaughter rosé wine are red fruit, flowers, citrus, and melon, with a pleasant crunchy green flavor on the finish similar to celery or rhubarb. Of course, depending on the type of grape the rosé wine is made with will greatly vary the flavor.",
                Origin="Israel",Amount=100,Price=100},
                new ProductModel {ProductID=2,ProductName="DavidsSon",Type="white",Description="The primary flavors of DavidsSon whith wine are good,very good,very very good, flowers, citrus, and some other bullshit, with a pleasant crunchy green flavor on the finish similar to celery or rhubarb. Of course, depending on the type of grape the  wine is made with will greatly vary the flavor.",
                Origin="Israel",Amount=100,Price=200}};
            products.ForEach(s=> context.Products.Add(s));
            context.SaveChanges();
            var orders = new List<OrderModel>
            {
                new OrderModel{ ProductID=1, Username="shanik", Quantity=2}
            };
            orders.ForEach(s=> context.Orders.Add(s));
            context.SaveChanges();
        }
            
        }
}