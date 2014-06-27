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
        // Instancie la base de donnée 
        private DataBaseASLRDEntities db = new DataBaseASLRDEntities();

        // Page adresse simple avec la liste de commentaires
        [HandleError]
        public ActionResult AdresseSTD()
        {
            ViewBag.Message = "Adresse";
            ViewBag.error = "";
            var cart = ASLRDModels.MGetCart(this.HttpContext);
            return View(cart.MGetCommentaire());            
        }

        // Page adresse avec AJAX et un autocomplete avec la liste de commentaires
       [HandleError]
        public ActionResult AdresseAC()
        {
            ViewBag.Message = "Adresse";
            ViewBag.error = "";
            var cart = ASLRDModels.MGetCart(this.HttpContext);
            return View(cart.MGetCommentaire());  
        }

       // Page restaurant + redirection vers la page "Adresse" si l'on commence par cette page 
        [HandleError]
        public ActionResult Restaurant()
        {
            ViewBag.Message = "Restaurant";
            ViewBag.error = "Vous devez commencer par entrer l'adresse";
            var cart = ASLRDModels.MGetCart(this.HttpContext);
            return View("AdresseAC", cart.MGetCommentaire());
        }

        // Page produit + redirection vers la page "Adresse" si l'on commence par cette page  
        [HandleError]
        public ActionResult Produit()
        {
            ViewBag.Message = "Produit";
            ViewBag.error = "Vous devez commencer par entrer l'adresse";
            var cart = ASLRDModels.MGetCart(this.HttpContext);
            return View("AdresseAC", cart.MGetCommentaire());
        }

        // Page commande + redirection vers la page "Adresse" si l'on commence par cette page  
        [HandleError]
        public ActionResult Commande()
        {
            ViewBag.Message = "Produit";
            ViewBag.error = "Vous devez commencer par entrer l'adresse";
            var cart = ASLRDModels.MGetCart(this.HttpContext);
            return View("AdresseAC", cart.MGetCommentaire());
        }

        // Page "A propos de"
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";
            return View();
        }

        // Page de contact
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        //liste de restaurant en fonction de la ville
        [HttpGet]
        [HandleError]
        public ActionResult GetRestaurant(string CityName)
        {
            var cart = ASLRDModels.MGetCart(this.HttpContext);
            var listerestaurant = cart.MGetRestaurant(CityName);
            var listcommentaire = cart.MGetCommentaire();
            ViewData["CID"] = cart.MAddCommande();
            if (string.IsNullOrEmpty(CityName))
            {
                ViewBag.error = "Erreur, entrer une ville (exemple: Strasbourg)";
                return View("AdresseAC", listcommentaire);
            }
            else
            {
                if (listerestaurant.FirstOrDefault() == null)
                {
                    ViewBag.error = "Erreur, entrer une ville existante ou cette ville est non référencé (exemple: Strasbourg)";
                    return View("AdresseAC", listcommentaire);
                }
                else
                {
                    return View("Restaurant", listerestaurant);
                }
            }
        }

        //liste de produit pour un restaurant
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

        //Ajoute un produit au panier temporaire ( panier temporaire -> utilisé si le client n'est pas authentifié)
        [HttpGet]
        [HandleError]
        public ActionResult AddToPanierTMP(produit Produit, int RestaurantID, int CommandeID)
        {           
            var cart = ASLRDModels.MGetCart(this.HttpContext);
            cart.MAddToPanierTMP(Produit, RestaurantID, CommandeID, cart.MGetCartId(this.HttpContext));
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

        //Supprime un produit du panier temporaire ( panier temporaire -> utilisé si le client n'est pas authentifié)
        [HttpGet]
        [HandleError]
        public ActionResult RemoveFromPanierTMP(int DetailCommandetmpID)
        {
            var cart = ASLRDModels.MGetCart(this.HttpContext);
            cart.MRemoveFromPanierTMP(DetailCommandetmpID, cart.MGetCartId(this.HttpContext));
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }
        
        //Affiche la commande avec les différents produits du panier temporaire
        [HttpGet]
        [HandleError]
        public ActionResult GetCommande(int CommandeID)
        {     
            var cart = ASLRDModels.MGetCart(this.HttpContext); 
            var listedetailcommandetmp = cart.GetCommandeTMP(CommandeID, cart.MGetCartId(this.HttpContext));
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

        // Retourne la liste de ville pour l'autocomplete de la page adresse avec AJAX 
        public JsonResult AAutoComplete(string term)
        {
            var result = (from a in db.adresse
                          where a.ville.ToLower().Contains(term.ToLower())
                          select new { a.ville }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
