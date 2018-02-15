using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SistemaVeterinaria.Context;
using SistemaVeterinaria.Models;

namespace SistemaVeterinaria.Controllers
{
    public class SurgeriesController : Controller
    {
        private VeterinaryContext db = new VeterinaryContext();

        // GET: Surgeries
        public ActionResult Index()
        {
            var surgeries = db.Surgeries.Include(s => s.Pet).Include(s => s.SurgeryType);
            return View(surgeries.ToList());
        }

        public ActionResult DailyNotification()
        {
            return View(db.Surgeries.ToList().FindAll(s => s.SurgeryDate == DateTime.Today));
        }

        // GET: Surgeries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Surgery surgery = db.Surgeries.Find(id);
            if (surgery == null)
            {
                return HttpNotFound();
            }
            return View(surgery);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
