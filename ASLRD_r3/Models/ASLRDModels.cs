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
        // Instancie la base de donnée 
        private DataBaseASLRD2Entities db = new DataBaseASLRD2Entities();
        string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";

        public static ASLRDModels MGetCart(HttpContextBase context)
        {
            var cart = new ASLRDModels();
            cart.ShoppingCartId = cart.MGetCartId(context);
            return cart;
        }

        // Méthode d'aide pour simplier l'appel de Cart (MGetCart)
        public static ASLRDModels MGetCart(Controller controller)
        {
            return MGetCart(controller.HttpContext);
        }

        // Utilisation de HttpContextBase pour authoriser l'acces aux cookies.
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
                    // Génere un nouveau GUID avec la classe System.Guid
                    Guid tempCartId = Guid.NewGuid();
                    // Envoi tempCartId au client commme un cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }

        //Retourne la liste des commentaires        
        public List<commentaire> MGetCommentaire()
        {
            // liste de commantaire
            var listecommentaire = (from c in db.commentaire orderby c.datecommentaire select c).Distinct().ToList();
            // Test si il a y quelque chose dans la liste
            if (listecommentaire.FirstOrDefault() == null)
            {
                // Si la liste de commentaire est vide, on créer un liste vide de type commentaire et on retourne une liste vide
                List<commentaire> listecommentaireE = new List<commentaire>();
                return listecommentaireE;
            }
            else
            {
                return listecommentaire;
            }
        }

        //Retourne la liste des villes      
        public List<string> MGetVille()
        {
            var listeville = (from a in db.adresse select a.ville).ToList();
            if (listeville.FirstOrDefault() == null)
            {
                List<string> listevilleeE = new List<string>();
                return listevilleeE;
            }
            else
            {
                return listeville;
            }
        }
        
        // Retourne la liste de ville en fonction de la valeur "term"
        public List<string> MGetVille(string term)        
        {
            var listeville = (from a in db.adresse where a.ville.ToUpper().Contains(term.ToUpper()) select a.ville).ToList();
            if (listeville.FirstOrDefault() == null)
            {
                List<string> listevilleeE = new List<string>();
                return listevilleeE;
            }
            else
            {
                return listeville;
            }
        }

        //retourne la liste des restaurants        
        public List<restaurant> MGetRestaurant(string CityName)
        {
            var listerestaurant = (from r in db.restaurant
                                   from a in db.adresse
                                   where a.restaurantID == r.restaurantID
                                   where a.ville.ToUpper() == CityName.ToUpper()
                                   select r).ToList();
            return listerestaurant;
        }
               
        //Initialise une commande et retourne un numero de commande
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

        //Retourne la liste de produit suivant un restaurant        
        public List<produit> MGetProduit(int RestaurantID)
        {
            var listeproduit = (from p in db.produit
                                from r in db.restaurant
                                where r.restaurantID == RestaurantID
                                where p.restaurantID == r.restaurantID
                                select p).ToList();
             if (listeproduit.FirstOrDefault() == null)
            {
                List<produit> listeproduitE = new List<produit>();
                return listeproduitE;
            }
            else
            {
                return listeproduit;
            }
        }

        //Ajoute un produit au panier  temporaire ( panier temporaire -> utilisé si le client n'est pas authentifié)   
        public void MAddToPanierTMP(int ProduitID, int RestaurantID, string SessionID)
        {
            var commandedetailtmpItem = new detailcommandetmp
            {
                //detailcommandeID = "",
                quantitee = 1,
                //reduction = "1,2",
                datedetailcommande = DateTime.Now,
                sessionID = SessionID,
                restaurantID = RestaurantID,
                //commandeID = 0,
                produitID = ProduitID
                //menuID = 0,
            };
            db.detailcommandetmp.Add(commandedetailtmpItem);
            db.SaveChanges();
        }

        //Supprime un produit du panier temporaire ( panier temporaire -> utilisé si le client n'est pas authentifié) 
        public void MRemoveFromPanierTMP(int DetailCommandeID, string SessionID)
        {
            var cartItem = (from dctmp in db.detailcommandetmp
                            where dctmp.sessionID == SessionID
                            where dctmp.detailcommandeID == DetailCommandeID
                            select dctmp).First();
            // Si il y a bien un produit
            if (cartItem != null)
            {
                // SUPPRESSION du produit
                db.detailcommandetmp.Remove(cartItem);
                // SAUVEGARDE
                db.SaveChanges();
            }
        }

        //Ajoute un produit au panier      
        public void MAddToPanier(int ProduitID, int RestaurantID, string ClientID)
        {
            var commandedetailItem = new detailcommande
            {
                //detailcommandeID = "",
                quantitee = 1,                
                //reduction = "1,2",
                datedetailcommande = DateTime.Now,
                clientID = ClientID,                            
                restaurantID = RestaurantID,
                //commandeID = 0,
                produitID = ProduitID
                //menuID = 0,
            };
            db.detailcommande.Add(commandedetailItem);
            db.SaveChanges();
        }

        //Supprime un produit du panier
        public void MRemoveFromPanier(int DetailCommandeID, string ClientID)
        {
            var cartItem = (from dc in db.detailcommande
                            where dc.clientID == ClientID
                            where dc.detailcommandeID == DetailCommandeID
                            select dc).First();
            // Si il y a bien un produit
            if (cartItem != null)
            {
                // SUPPRESSION du produit
                db.detailcommande.Remove(cartItem);
                // SAUVEGARDE
                db.SaveChanges();
            }
        }

        //Retourne la commande avec les différents produits du panier
        public List<detailcommandetmp> MGetCommandeTMP(string ClientID)
        {
            // LISTE la commande
            var listedetailcommandetmp = (from dc in db.detailcommandetmp
                                          where dc.sessionID == ClientID
                                          select dc).ToList();
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

        //Retourne si l'utilisateur est connecté ou pas pour finir de la commande
        public Boolean MGetRegister(string SessionID)
        {
            var listeclient = (from c in db.client
                                          where c.clientID == SessionID
                                          select c).ToList();
            if (listeclient.FirstOrDefault() == null)
            {                
                return false;
            }
            else
            {
                return true;
            }
        }
           
    }
}