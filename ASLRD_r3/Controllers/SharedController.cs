using ASLRD_r3.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASLRD_r3.Controllers
{
    public class SharedController : Controller
    {
        private DataBaseASLRDEntities db = new DataBaseASLRDEntities();

        public ActionResult Index()
        {
            return View();
        }

        [HandleError(View = "Error")]
        [HttpGet]
        public ActionResult _Commentaire()
        {
            var listecommentaire = (from c in db.commentaire
                                    select c).ToList();

            if (listecommentaire.FirstOrDefault() == null)
            {
                return PartialView();
            }
            else
            {
                return PartialView(listecommentaire);
            }
        }

    }
}
