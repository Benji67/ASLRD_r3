//------------------------------------------------------------------------------
// <auto-generated>
//    Ce code a été généré à partir d'un modèle.
//
//    Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//    Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASLRD_r3.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class detailcommande
    {
        public int detailcommandeID { get; set; }
        public int quantitee { get; set; }
        public Nullable<double> reduction { get; set; }
        public System.DateTime datedetailcommande { get; set; }
        public string clientID { get; set; }
        public int restaurantID { get; set; }
        public Nullable<int> commandeID { get; set; }
        public Nullable<int> produitID { get; set; }
        public Nullable<int> menuID { get; set; }
    
        public virtual client client { get; set; }
        public virtual commande commande { get; set; }
        public virtual restaurant restaurant { get; set; }
        public virtual produit produit { get; set; }
        public virtual menu menu { get; set; }
    }
}
