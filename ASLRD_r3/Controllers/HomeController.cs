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
            var cart = ASLRDModels.MGetCart(this.HttpContext);
            return View(cart.MGetCommentaire());            
        }

        [HandleError]
        public ActionResult Restaurant()
        {
            ViewBag.Message = "Restaurant";
            ViewBag.error = "Vous devez commencer par entrer l'adresse";
            var cart = ASLRDModels.MGetCart(this.HttpContext);
            return View("Adresse", cart.MGetCommentaire());
        }

        [HandleError]
        public ActionResult Produit()
        {
            ViewBag.Message = "Produit";
            ViewBag.error = "Vous devez commencer par entrer l'adresse";
            var cart = ASLRDModels.MGetCart(this.HttpContext);
            return View("Adresse", cart.MGetCommentaire());
        }

        [HandleError]
        public ActionResult Commande()
        {
            ViewBag.Message = "Produit";
            ViewBag.error = "Vous devez commencer par entrer l'adresse";
            var cart = ASLRDModels.MGetCart(this.HttpContext);
            return View("Adresse", cart.MGetCommentaire());
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
        public ActionResult GetRestaurant(string CityName)
        {
           
            var cart = ASLRDModels.MGetCart(this.HttpContext);
            var listerestaurant = cart.MGetRestaurant(CityName);
            var listcommentaire = cart.MGetCommentaire();
            ViewData["CID"] = cart.MAddCommande();
            //ViewData["SessionID"] = cart.MGetCartId(this.HttpContext);
            if (string.IsNullOrEmpty(CityName))
            {
                ViewBag.error = "Erreur, entrer une ville (exemple: Strasbourg)";
                return View("Adresse", listcommentaire);
            }
            else
            {
                if (listerestaurant.FirstOrDefault() == null)
                {
                    ViewBag.error = "Erreur, entrer une ville existante ou cette ville est non référencé (exemple: Strasbourg)";
                    return View("Adresse", listcommentaire);
                }
                else
                {
                    return View("Restaurant", listerestaurant);
                }
            }
        }

        //liste des produit pour un restaurant
        [HttpGet]
        [HandleError]
        public ActionResult GetProduit(int RestaurantID, int CommandeID)
        {
            ViewData["CID"] = CommandeID;
            var cart = ASLRDModels.MGetCart(this.HttpContext);
            var listeproduit = cart.MGetProduit(RestaurantID);
            if (listeproduit.FirstOrDefault() == null)
            {
                ViewBag.error = "Erreur, la liste des produits est vide pour le restaurant";
                return View("Restaurant");
            }
            else
            {
                return View("Produit", listeproduit);
            }
        }
             
        //Ajouter un produit au panier 
        [HttpGet]
        [HandleError]
        public ActionResult AddToPanierTMP(produit Produit, int RestaurantID, int CommandeID)
        {           
            var cart = ASLRDModels.MGetCart(this.HttpContext);
            cart.MAddToPanierTMP(Produit, RestaurantID, CommandeID, cart.MGetCartId(this.HttpContext));
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

        //Supprimer un produit du panier temporaire
        [HttpGet]
        [HandleError]
        public ActionResult RemoveFromPanierTMP(int ProduitID)
        {
            // GET Session info
            var cart = ASLRDModels.MGetCart(this.HttpContext);
            // LIST of detail commande avec le sessionID and le produit a supprimer
            cart.MRemoveFromPanierTMP(ProduitID, cart.MGetCartId(this.HttpContext));
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

        //Supprimer un produit du panier
        [HttpGet]
        [HandleError]
        public ActionResult RemoveFromPanier(int ProduitID)
        {
            // GET Session info
            var cart = ASLRDModels.MGetCart(this.HttpContext);
            cart.MRemoveFromPanier(ProduitID, cart.MGetCartId(this.HttpContext));
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }        

        //Affiche le panier
        [HttpGet]
        [HandleError]
        public ActionResult GetCommande(int CommandeID)
        {            
            // GET Session info
            var cart = ASLRDModels.MGetCart(this.HttpContext);    
            // LISTE la commande
            var listedetailcommandetmp = cart.GetCommandeTMP(CommandeID, cart.MGetCartId(this.HttpContext));
            // SI il n'y rien dans le panier
            if (listedetailcommandetmp.FirstOrDefault() == null)
            {
                // SI le panier est vide
                // AFFICHER la page Produit avec un message d'erreur
                ViewBag.error = "la commande est vide";
                return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
            }
            else
            {
                // AFFICHER la liste des produits
                return View("Commande", listedetailcommandetmp);
            }           
        }
    }
}
