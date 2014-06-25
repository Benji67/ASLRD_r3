using ASLRD_r3.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASLRD_r3.Models
{
    public class ASLRDModels
    {
        private DataBaseASLRDEntities db = new DataBaseASLRDEntities();
        string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";

        public static ASLRDModels MGetCart(HttpContextBase context)
        {
            var cart = new ASLRDModels();
            cart.ShoppingCartId = cart.MGetCartId(context);
            return cart;
        }

        // Helper method to simplify shopping cart calls
        public static ASLRDModels MGetCart(Controller controller)
        {
            return MGetCart(controller.HttpContext);
        }

        // We're using HttpContextBase to allow access to cookies.
        public string MGetCartId(HttpContextBase context)
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


        //retourne la liste des commentaires        
        public List<commentaire> MGetCommentaire()
        {

            var listecommentaire = (from c in db.commentaire select c).ToList();
            if (listecommentaire.FirstOrDefault() == null)
            {
                List<commentaire> listecommentaireE = new List<commentaire>();
                return listecommentaireE;
            }
            else
            {
                return listecommentaire;
            }
        }

        //retourne la liste des commentaires        
        public List<restaurant> MGetRestaurant(string CityName)
        {
            var listerestaurant = (from r in db.restaurant
                                   from a in db.adresse
                                   where a.restaurantID == r.restaurantID
                                   where a.ville.ToUpper() == CityName.ToUpper()
                                   select r).ToList();
            return listerestaurant;
        }
               
        //ajout une commande        
        public int MAddCommande()
        {
            var commandeItem = new commande
            {
                prixtotal = 0,
                datecommande = DateTime.Now,
                etatcommande = "brouillon"
            };
            db.commande.Add(commandeItem);
            db.SaveChanges();
            var CommandeIDItem = (from c in db.commande select c).OrderByDescending(c => c.datecommande).FirstOrDefault();
            return CommandeIDItem.commandeID;
        }

        //retourne la liste de produit suivant un restaurant        
        public List<produit> MGetProduit(int RestaurantID)
        {
            var listeproduit = (from p in db.produit
                                from r in db.restaurant
                                where r.restaurantID == RestaurantID
                                where p.restaurantID == r.restaurantID
                                select p).ToList();
            return listeproduit;
        }

        //Ajouter un produit au panier        
        public void MAddToPanierTMP(produit Produit, int RestaurantID, int CommandeID, string SessionID)
        {

            var CommandeIDItem = (from c in db.commande select c).OrderByDescending(c => c.datecommande).FirstOrDefault();
            var commandedetailtmpItem = new detailcommandetmp
            {
                sessionID = SessionID,
                datedetailcommande = DateTime.Now,
                quantitee = 1,
                restaurantID = RestaurantID,
                commandeID = CommandeID
            };
            db.detailcommandetmp.Add(commandedetailtmpItem);
            db.SaveChanges();
        }

        //Supprimer un produit du panier temporaire
        public void MRemoveFromPanierTMP(int ProduitID, string SessionID)
        {           
            // LIST of detail commande avec le sessionID and le produit a supprimer
            var cartItem = db.detailcommandetmp.Single(dctmp => dctmp.sessionID == SessionID /*&& dctmp.produit == Produit*/);
            // Si il y a bien un produit
            if (cartItem != null)
            {
                // SUPPRESSION du produit
                db.detailcommandetmp.Remove(cartItem);
                // SAUVEGARDE
                db.SaveChanges();
            }
        }

        //Supprimer un produit du panier
        public void MRemoveFromPanier(int ProduitID, string ClientID)
        {
            // LIST of detail commande avec le sessionID and le produit a supprimer
            var cartItem = db.detailcommande.Single(dctmp => dctmp.clientID == ClientID /*&& dctmp.produit == Produit*/);
            // Si il y a bien un produit
            if (cartItem != null)
            {
                // SUPPRESSION du produit
                db.detailcommande.Remove(cartItem);
                // SAUVEGARDE
                db.SaveChanges();
            }
        }

        public List<detailcommandetmp> GetCommandeTMP(int CommandeID, string SessionID)
        {          
            // LISTE la commande
            var listedetailcommandetmp = (from dc in db.detailcommandetmp 
                                          where dc.commandeID == CommandeID 
                                          where dc.sessionID == SessionID
                                          select dc).ToList();
            // SI il n'y rien dans le panier
            if (listedetailcommandetmp.FirstOrDefault() == null)
            {
                List<detailcommandetmp> listedetailcommandetmpE = new List<detailcommandetmp>();
                return listedetailcommandetmpE;
            }
            else
            {
                return listedetailcommandetmp;
            }
        }

        public List<detailcommande> GetCommande(int CommandeID)
        {
            // LISTE la commande
            var listedetailcommande = (from dc in db.detailcommande where dc.commandeID == CommandeID select dc).ToList();
            // SI il n'y rien dans le panier
            if (listedetailcommande.FirstOrDefault() == null)
            {
                List<detailcommande> listedetailcommandeE = new List<detailcommande>();
                return listedetailcommandeE;
            }
            else
            {
                return listedetailcommande;
            }
        }

           
    }
}