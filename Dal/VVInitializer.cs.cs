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
                new ProductModel {ProductID=1, ProductName="Salomons Daughter",Type="rose",Description="The primary flavors of SalomonsDaughter rosé wine are red fruit, flowers, citrus, and melon, with a pleasant crunchy green flavor on the finish similar to celery or rhubarb. Of course, depending on the type of grape the rosé wine is made with will greatly vary the flavor.",
                Origin="Israel",Amount=100,Price=100,Winery="Bat Shlomo"},
                new ProductModel {ProductID=2,ProductName="Davids Son",Type="white",Description="The primary flavors of DavidsSon whith wine are good,very good,very very good, flowers, citrus, and some other bullshit, with a pleasant crunchy green flavor on the finish similar to celery or rhubarb. Of course, depending on the type of grape the  wine is made with will greatly vary the flavor.",
                Origin="Israel",Amount=100,Price=200,Winery="The Kastel"},
                new ProductModel {ProductID = 3,ProductName = "Crimson Valley",Winery = "Golan Heights",Type = "red",Description = "A bold, structured red,Aromatic with hints of black cherries and a touch of oak, this wine is perfect with hearty meals",Origin = "Israel",Amount = 100,Price = 200,NewPrice = 0},
                new ProductModel {ProductID = 4,ProductName = "Château du Soleil",Winery = "Château Lafite Rothschild",Type = "red",Description = "An exquisite Bordeaux blend  With layers of cassis, plum, and a velvety finish, it’s the epitome of elegance. ",Origin = "France",Amount = 100,Price = 200,NewPrice = 0},
                new ProductModel {ProductID = 5,ProductName = "Tuscan Twilight",Winery = "Antinori",Type = "red",Description = "Tuscan Twilight is a vibrant Chianti with a lively character.  notes of red berry and subtle spices make it a delightful companion.",Origin = "Italy",Amount = 100,Price = 200,NewPrice = 0},
                new ProductModel {ProductID = 6,ProductName = "Ruby Echo",Winery = "Ramat Negev",Type = "red",Description = "Ruby Echo is a Merlot that surprises with its robust flavor profile, featuring ripe berries and a hint of Mediterranean herbs.",Origin = "Israel",Amount = 100,Price = 200,NewPrice = 0},
                new ProductModel {ProductID = 7,ProductName = "Vega Dorada",Winery = "Vega Sicilia",Type = "red",Description = "Vega Dorada is a Tempranillo that balances intensity with finesse. Aged in oak, it offers deep flavors of dark fruit, leather, and tobacco.",Origin = "Spain",Amount = 100,Price = 200,NewPrice = 0},
                new ProductModel {ProductID = 8,ProductName = "Lune Noire",Winery = "Domaine de la Romanée-Conti",Type = "red",Description = "luxurious Pinot Noir that embodies the depth and complexity of Burgundy. With a bouquet of dark fruits, earth, and a hint of truffle",Origin = "France",Amount = 100,Price = 200,NewPrice = 0},
                new ProductModel {ProductID = 9,ProductName = "Siena Sanguine",Winery = "Biondi-Santi",Type = "red",Description = "Biondi-Santi's Siena Sanguine is a testament to the grace of Italian reds. It showcases a perfect harmony of acidity and tannins, with a whisper of oak.",Origin = "Italy",Amount = 100,Price = 200,NewPrice = 0},
                new ProductModel {ProductID = 10,ProductName = "Esencia de la Tierra",Winery = "Bodegas Torres",Type = "red",Description = "Esencia de la Tierra by Bodegas Torres is a Garnacha that captivates with its vibrant red fruit flavors and a subtle hint of vanilla",Origin = "Spain",Amount = 100,Price = 200,NewPrice = 0},
                new ProductModel {ProductID = 11,ProductName = "Dewdrop Essence",Winery = "Carmel",Type = "white",Description = "A crisp, refreshing Chardonnay with hints of apple and pear; perfect for a sunny day.",Origin = "Israel",Amount = 100,Price = 200,NewPrice = 0},
                new ProductModel {ProductID = 12,ProductName = "Lumière d'Alsace",Winery = "Trimbach",Type = "white",Description = "Elegant Riesling, floral and fruity, with a vibrant acidity and a touch of minerality.",Origin = "France",Amount = 100,Price = 200,NewPrice = 0},
                new ProductModel {ProductID = 13,ProductName = "Venetian Harmony",Winery = "Pieropan",Type = "white",Description = "Light Soave with nuanced almond notes, marrying well with delicate fish dishes.",Origin = "Italy",Amount = 100,Price = 200,NewPrice = 0},
                new ProductModel {ProductID = 14,ProductName = "Blanca Cádiz",Winery = "González Byass",Type = "white",Description = "A zesty Fino Sherry, offering almond and green apple notes with a saline finish.",Origin = "Spain",Amount = 100,Price = 200,NewPrice = 0},
                new ProductModel {ProductID = 15,ProductName = "Golan Mist",Winery = "Yarden",Type = "white",Description = "Bright Sauvignon Blanc with tropical fruit flavors and a crisp, refreshing finish.",Origin = "Israel",Amount = 100,Price = 200,NewPrice = 0},
                new ProductModel {ProductID = 16,ProductName = "Chablis Charm",Winery = "William Fèvre",Type = "white",Description = "Classic Chablis with vibrant lemon zest and flinty notes; perfect with oysters.",Origin = "France",Amount = 100,Price = 200,NewPrice = 0},
                new ProductModel {ProductID = 17,ProductName = "Tuscan Breeze",Winery = "Antinori",Type = "white",Description = "Aromatic Vermentino, offering a bouquet of herbs and citrus, ideal for Mediterranean cuisine.",Origin = "Italy",Amount = 100,Price = 200,NewPrice = 0},
                new ProductModel {ProductID = 18,ProductName = "Valencia Sunrise",Winery = "Marqués de Cáceres",Type = "white",Description = "Fresh and lively Albariño, with peach and citrus notes, embodying the Spanish coast.",Origin = "Spain",Amount = 100,Price = 200,NewPrice = 0},
                new ProductModel {ProductID = 19,ProductName = "ChardoNana",Winery = "Nana",Type = "white",Description = "From the Negev Desert, great handmade chardoney.",Origin = "Israel",Amount = 100,Price = 200,NewPrice = 0},
                new ProductModel {ProductID = 20,ProductName = "Desert Blush",Winery = "Tabor",Type = "rose",Description = "A delicate rosé with watermelon and strawberry notes, perfect for a desert sunset.",Origin = "Israel",Amount = 100,Price = 200,NewPrice = 0},
                new ProductModel {ProductID = 21,ProductName = "Provence Petal",Winery = "Domaines Ott",Type = "rose",Description = " Iconic, with elegant floral and citrus aromas; the quintessence of French rosé.",Origin = "France",Amount = 100,Price = 200,NewPrice = 0},
                new ProductModel {ProductID = 22,ProductName = "Venetian Pink",Winery = "Bertani",Type = "rose",Description = "Refreshing and crisp, with hints of berries and a subtle minerality.",Origin = "Italy",Amount = 100,Price = 200,NewPrice = 0},
                new ProductModel {ProductID = 23,ProductName = "Coral de Almería",Winery = "Bodegas Muga",Type = "rose",Description = "Vibrant and fruity, offering a taste of Spanish summer in every sip.",Origin = "Spain",Amount = 100,Price = 200,NewPrice = 0},
                new ProductModel {ProductID = 24,ProductName = "Blossom Mirage",Winery = "Flam Winery",Type = "rose",Description = "A symphony of pomegranate and rose petals, light and refreshing.",Origin = "Israel",Amount = 100,Price = 200,NewPrice = 0},
                new ProductModel {ProductID = 25,ProductName = "Riviera Whisper",Winery = "Château Minuty",Type = "rose",Description = "A sophisticated blend with peach and orange zest, capturing the Riviera's essence.",Origin = "France",Amount = 100,Price = 200,NewPrice = 0},
            };
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