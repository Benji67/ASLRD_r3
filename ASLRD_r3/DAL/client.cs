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
    
    public partial class client
    {
        public client()
        {
            this.adresse = new HashSet<adresse>();
            this.commentaire = new HashSet<commentaire>();
            this.detailcommande = new HashSet<detailcommande>();
        }

        [DisplayName("N° client")]
        public string clientID { get; set; }
        [DisplayName("Adresse mail")]
        public string email { get; set; }
        [DisplayName("Mot de passe")]
        public string motdepasse { get; set; }
        [DisplayName("Nom")]
        public string nom { get; set; }
        [DisplayName("Prénom")]
        public string prenom { get; set; }
        [DisplayName("N° de téléphone")]
        public Nullable<int> telephone { get; set; }
        [DisplayName("Status")]
        public string status { get; set; }
        [DisplayName("Genre")]
        public string genre { get; set; }
    
        public virtual ICollection<adresse> adresse { get; set; }
        public virtual ICollection<commentaire> commentaire { get; set; }
        public virtual ICollection<detailcommande> detailcommande { get; set; }
    }
}
