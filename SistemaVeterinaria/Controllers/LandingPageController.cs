using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaVeterinaria.Context;
using SistemaVeterinaria.Models;

namespace SistemaVeterinaria.Controllers
{
    public class LandingPageController : Controller
    {
        private VeterinaryContext db = new VeterinaryContext();
        // GET: LandingPage
        public ActionResult Index()
        {
            ViewBag.Vaccines = db.Vaccines.Count(v => v.VaccineDate == DateTime.Today & v.VaccineNumber != 1);
            ViewBag.Surgeries = db.Surgeries.Count(s => s.SurgeryDate == DateTime.Today);
            ViewBag.Showers = db.Showers.Count(sh => sh.ShowerDate == DateTime.Today);

            return View();
        }
    }
}