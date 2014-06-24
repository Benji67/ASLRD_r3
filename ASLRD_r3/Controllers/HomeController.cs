using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASLRD_r3.DAL;
using ASLRD_r3.Models;

namespace ASLRD_r3.Controllers
{
    public class HomeController : Controller
    {
        private DataBaseASLRDEntities db = new DataBaseASLRDEntities();

        [HandleError]
        public ActionResult Adresse()
        {
            ViewBag.Message = "Adresse";
            ViewBag.error = "";
            var listecommentaire = (from c in db.commentaire select c).ToList();  
            return View(listecommentaire);            
        }

        [HandleError]
        public ActionResult Restaurant()
        {
            ViewBag.Message = "Restaurant";
            ViewBag.error = "Vous devez commencer par entrer l'adresse";
            var listecommentaire = (from c in db.commentaire select c).ToList();
            return View("Adresse", listecommentaire);
        }

        [HandleError]
        public ActionResult Produit()
        {
            ViewBag.Message = "Produit";
            ViewBag.error = "Vous devez commencer par entrer l'adresse";
            var listecommentaire = (from c in db.commentaire select c).ToList();
            return View("Adresse", listecommentaire);
        }

        [HandleError]
        public ActionResult Commande()
        {
            ViewBag.Message = "Produit";
            ViewBag.error = "Vous devez commencer par entrer l'adresse";
            var listecommentaire = (from c in db.commentaire select c).ToList();
            return View("Adresse", listecommentaire);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        //liste des restaurants en fonction de la ville
        [HttpGet]
        [HandleError]
        public ActionResult GetRestaurant(string cityname)
        {
            var listecommentaire = (from c in db.commentaire select c).ToList();    

            if (string.IsNullOrEmpty(cityname))
            {
                ViewBag.error = "Erreur, entrer une ville (exemple: Strasbourg)";
                return View("Adresse", listecommentaire);
            }
            else
            {
                var listerestaurant = (from r in db.restaurant
                                       from a in db.adresse
                                       where a.restaurantID == r.restaurantID
                                       where a.ville.ToUpper() == cityname.ToUpper()
                                       select r).ToList();
                if (listerestaurant.FirstOrDefault() == null)
                {
                    ViewBag.error = "Erreur, entrer une ville existante ou cette ville est non référencé (exemple: Strasbourg)";
                    return View("Adresse", listecommentaire);
                }
                else
                {
                    ViewData["cityname"] = Request.Params["cityname"];
                    return View("Restaurant", listerestaurant);
                }
            }
        }

        //liste des produit pour un restaurant
        [HttpGet]
        [HandleError]
        public ActionResult GetProduit(int RestaurantID)
        {
            var listeproduit = (from p in db.produit
                                from r in db.restaurant
                                where r.restaurantID == RestaurantID
                                where p.restaurantID == r.restaurantID
                                select p).ToList();
            if (listeproduit.FirstOrDefault() == null)
            {
                ViewBag.error = "Erreur, la liste des produits est vide pour le restaurant";
                return View("Restaurant");
            }
            else
            {
                ViewData["cityname"] = Request.Params["cityname"];
                //ViewData["restaurantname"] = GetrestaurantnamebyRestaurantID(RestaurantID);
                return View("Produit", listeproduit);
            }
        }

        // We're using HttpContextBase to allow access to cookies.
        public string GetCartId(HttpContextBase context)
        {
            string CartSessionKey = "CartId";
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] =
                        context.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();
                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }

        //Ajouter au panier un produit
        [HttpGet]
        [HandleError]
        public ActionResult AddToPanier(produit Produit)
        {
            // GET Session info
            string cart = GetCartId(this.HttpContext);
            
            // CHECK si l'utilisateur existe
            var cartItem = db.client.SingleOrDefault(c => c.clientID == cart);

            var commandeItem = new commande();
            commandeItem.prixtotal = 30;
            commandeItem.datecommande = DateTime.Now;
            commandeItem.etatcommande = "brouillon";
            db.commande.Add(commandeItem);

            var commandedetailtmpItem = new detailcommandetmp();
            var commandedetailItem = new detailcommande();
            if (cartItem == null)
            {
                commandedetailtmpItem.sessionID = cart;
                commandedetailtmpItem.datedetailcommande = DateTime.Now;
                commandedetailtmpItem.quantitee = 1;
                commandedetailtmpItem.restaurantID = 1;
                commandedetailtmpItem.commandeID = 1;
                db.detailcommandetmp.Add(commandedetailtmpItem);
            }
            else
            {
                commandedetailItem.clientID = cart;
                commandedetailItem.datedetailcommande = DateTime.Now;
                commandedetailItem.quantitee = 1;
                commandedetailItem.restaurantID = 1;
                commandedetailItem.commandeID = 1;
                db.detailcommandetmp.Add(commandedetailtmpItem);
            }

            db.SaveChanges();

            var listedetailcommande = (from dc in db.detailcommandetmp where dc.sessionID == cart select dc).ToList();
            return View("Commande", listedetailcommande);
        }
    }
}
