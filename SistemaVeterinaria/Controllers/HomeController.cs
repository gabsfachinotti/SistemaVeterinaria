using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaVeterinaria.Context;
using SistemaVeterinaria.Models;

namespace SistemaVeterinaria.Controllers
{
    public class HomeController : Controller
    {
        private VeterinaryContext db = new VeterinaryContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult InitialNotification()
        {
            ViewBag.Vaccines = db.Vaccines.Count(v => v.VaccineDate == DateTime.Today & v.VaccineNumber != 1);
            ViewBag.Surgeries = db.Surgeries.Count(s => s.SurgeryDate == DateTime.Today);
            ViewBag.Showers = db.Showers.Count(sh => sh.ShowerDate == DateTime.Today);

            return View("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}