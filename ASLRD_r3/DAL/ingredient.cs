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
    
    public partial class ingredient
    {
        public ingredient()
        {
            this.produit = new HashSet<produit>();
        }
    
        public int ingredientID { get; set; }
        public string nom { get; set; }
        public Nullable<double> quantite { get; set; }
        public string description { get; set; }
    
        public virtual ICollection<produit> produit { get; set; }
    }
}
