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

        // Retourne la liste de ville pour l'autocomplete de la page adresse avec JS 
        public JsonResult AAutoComplete(string term)
        {
            // Instancie la base de donnée 
            DataBaseASLRD2Entities db = new DataBaseASLRD2Entities();
            //var cart = ASLRD2Models.MGetCart(this.HttpContext);
            //var listedetailcommandetmp = cart.MGetVille(term);
            var result = (from a in db.adresse
                          where a.ville.ToUpper().Contains(term.ToUpper())
                          select new { a.ville }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //liste de produit pour un restaurant
        [HttpGet]
        [HandleError]
        public ActionResult GetProduit(int RestaurantID)
        {
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
        public ActionResult AddToPanierTMP(int ProduitID, int RestaurantID, int Quantity)
        {           
            var cart = ASLRDModels.MGetCart(this.HttpContext);
            cart.MAddToPanierTMP(ProduitID, RestaurantID, cart.MGetCartId(this.HttpContext), Quantity);
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

        //Supprime un produit du panier temporaire ( panier temporaire -> utilisé si le client n'est pas authentifié)
        [HttpGet]
        [HandleError]
        public ActionResult RemoveFromPanierTMP(int DetailCommandeID)
        {
            var cart = ASLRDModels.MGetCart(this.HttpContext);
            cart.MRemoveFromPanierTMP(DetailCommandeID, cart.MGetCartId(this.HttpContext));
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }
        
        //Affiche la commande avec les différents produits du panier temporaire
        [HttpGet]
        [HandleError]
        public ActionResult GetCommandeTMP()
        {     
            var cart = ASLRDModels.MGetCart(this.HttpContext); 
            var listedetailcommandetmp = cart.MGetCommandeTMP(cart.MGetCartId(this.HttpContext));
            if (listedetailcommandetmp.FirstOrDefault() == null)
            {
                ViewBag.error = "La commande est vide";
                return View("Commande", listedetailcommandetmp);
            }
            else
            {
                // AFFICHER la liste des produits
                return View("Commande", listedetailcommandetmp);
            }
            
        }

        //Valide la commande
        [HttpGet]
        [HandleError]
        public ActionResult GetFinish()
        {
            var cart = ASLRDModels.MGetCart(this.HttpContext);
            var listedetailcommandetmp = cart.MGetCommandeTMP(cart.MGetCartId(this.HttpContext));
            if (cart.MGetRegister(cart.MGetCartId(this.HttpContext)) == true)
            {
                ViewBag.error = "Vous etes connecté... mais...";
                return View("Commande", listedetailcommandetmp);
            }
            else
            {
                ViewBag.error = "Vous devez vous connecter";
                return View("Commande", listedetailcommandetmp);
            }

        }

    }
}
