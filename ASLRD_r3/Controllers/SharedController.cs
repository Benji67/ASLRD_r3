using ASLRD_r3.DAL;
using ASLRD_r3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASLRD_r3.Controllers
{
    public class SharedController : Controller
    {
        // Instancie la base de donnée 
        private DataBaseASLRDEntities db = new DataBaseASLRDEntities();

        public ActionResult Error()
        {
            return View();
        }

        // Liste des commentaires pour la vue partiel
        [HandleError]
        [HttpGet]
        public ActionResult _Commentaire()
        {
            var cart = ASLRDModels.MGetCart(this.HttpContext);
            return PartialView(cart.MGetCommentaire());
        }

    }
}
