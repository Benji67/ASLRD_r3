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
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    
    public partial class detailcommande
    {
        [DisplayName("N° ligne commande")]
        public int detailcommandeID { get; set; }
        [DisplayName("Quantité")]
        public int quantitee { get; set; }
        [DisplayName("Réduction")]
        public Nullable<double> reduction { get; set; }
        [DisplayName("Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public System.DateTime datedetailcommande { get; set; }
        [DisplayName("Client")]
        public string clientID { get; set; }
        [DisplayName("Restaurant")]
        public int restaurantID { get; set; }
        [DisplayName("Commande")]
        public Nullable<int> commandeID { get; set; }
        [DisplayName("Produit")]
        public Nullable<int> produitID { get; set; }
        [DisplayName("Menu")]
        public Nullable<int> menuID { get; set; }
    
        public virtual client client { get; set; }
        public virtual commande commande { get; set; }
        public virtual restaurant restaurant { get; set; }
        public virtual produit produit { get; set; }
        public virtual menu menu { get; set; }
    }
}
