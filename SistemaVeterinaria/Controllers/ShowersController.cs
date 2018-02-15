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
    public class ShowersController : Controller
    {
        private VeterinaryContext db = new VeterinaryContext();

        // GET: Showers
        public ActionResult Index()
        {
            var showers = db.Showers.ToList().FindAll(s => s.ShowerDate >= DateTime.Today);

            ViewBag.Pets = db.Pets.ToList();
            return View(showers.ToList());
        }

        public ActionResult DailyNotification()
        {
            return View(db.Showers.ToList().FindAll(s => s.ShowerDate == DateTime.Today));
        }

        // GET: Showers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shower shower = db.Showers.Find(id);
            if (shower == null)
            {
                return HttpNotFound();
            }
            return View(shower);
        }

        public JsonResult ValidateShower(DateTime showerDate, int petId, bool showerTurn, bool edit)
        {
            int data = 0;
            if (db.Showers.ToList().Count(s => s.ShowerDate == showerDate) > 3)
            {
                data = 1;
            }
            else
            {
                if (db.Showers.ToList().Count(s => s.ShowerDate == showerDate & s.ShowerTurn == showerTurn) > 1)
                {
                    data = 2;
                }
                else
                {
                    if (db.Showers.ToList().Exists(s => s.ShowerDate == showerDate & s.PetId == petId & !edit))
                    {
                        data = 3;
                    }
                }
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateShower(Shower shower)
        {
            shower.Pet = db.Pets.Find(shower.PetId);
            var status = false;
            string specie = String.Empty;
            if (shower.ShowerDate != null & shower.PetId != 0)
            {
                db.Showers.Add(shower);
                db.SaveChanges();
                status = true;

                if (shower.Pet.PetSpecie == Species.Perro)
                {
                    specie = "Perro";
                }
                else
                {
                    specie = "Gato";
                }
            }
            else
            {
                status = false;
            }

            return new JsonResult { Data = new { status = status, showerId = shower.ShowerId, petId = shower.PetId, petname = shower.Pet.PetName, owner = shower.Pet.Owner.OwnerLastName + ", " + shower.Pet.Owner.OwnerName, specie = specie, date = shower.ShowerDate.ToShortDateString() } };
        }

        public JsonResult EditShower(Shower shower)
        {
            shower.Pet = db.Pets.Find(shower.PetId);
            var status = false;
            var specie = String.Empty;
            if (shower.ShowerDate != null & shower.PetId != 0)
            {
                status = true;
                db.Entry(shower).State = EntityState.Modified;
                db.SaveChanges();

                if (shower.Pet.PetSpecie == Species.Perro)
                {
                    specie = "Perro";
                }
                else
                {
                    specie = "Gato";
                }
            }

            return new JsonResult { Data = new { status = status, petId = shower.PetId, petName = shower.Pet.PetName, owner = shower.Pet.Owner.OwnerLastName + ", " + shower.Pet.Owner.OwnerName, specie = specie, date = shower.ShowerDate.ToShortDateString() } };
        }

        [HttpPost]
        public JsonResult DeleteShower(int showerId)
        {
            var status = false;
            Shower shower = db.Showers.Find(showerId);
            if (shower != null)
            {
                db.Showers.Remove(shower);
                db.SaveChanges();
                status = true;
            }

            return Json(status, JsonRequestBehavior.AllowGet);
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
