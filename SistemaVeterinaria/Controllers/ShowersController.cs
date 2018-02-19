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

        public JsonResult ValidateShower(DateTime showerDate, int petId, bool showerTurn)
        {
            int data = 0; //Baño Validado
            if (db.Showers.ToList().Count(s => s.ShowerDate == showerDate) > 3)
            {
                //Sin disponibilidad en el día
                data = 1;
            }
            else
            {
                if (db.Showers.ToList().Count(s => s.ShowerDate == showerDate & s.ShowerTurn == showerTurn & s.PetId != petId) > 1)
                {
                    //Sin disponibilidad en el turno
                    data = 2;
                }
                else
                {
                    if (db.Showers.ToList().Exists(s => s.ShowerDate == showerDate & s.PetId == petId) & petId > 0)
                    {
                        //Ya existe baño para ese paciente en el día especificado.
                        data = 3;
                    }
                }
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateShower(Shower shower)
        {
            if (shower.ShowerDate != null)
            {
                Pet pet = db.Pets.Find(shower.PetId);

                if (pet == null)
                {
                    shower.PetId = 0;
                    shower.PetName = String.Empty;
                    shower.PetSpecie = String.Empty;
                    shower.Owner = String.Empty;
                    shower.OwnerPhone = String.Empty;
                }
                else
                {
                    shower.PetName = pet.PetName;
                    shower.Owner = pet.Owner.OwnerLastName + ", " + pet.Owner.OwnerName;
                    shower.OwnerPhone = pet.Owner.OwnerPhone;

                    if (pet.PetSpecie == Species.Perro)
                    {
                        shower.PetSpecie = "Perro";
                    }
                    else
                    {
                        shower.PetSpecie = "Gato";
                    }
                }

                db.Showers.Add(shower);
                db.SaveChanges();                

                return new JsonResult { Data = new { status = true, showerId = shower.ShowerId, petId = shower.PetId, petname = shower.PetName, owner = shower.Owner, specie = shower.PetSpecie, date = shower.ShowerDate.ToShortDateString() } };
            }
            else
            {
                return new JsonResult { Data = new { status = false } };
            }
        }

        public JsonResult EditShower(Shower shower)
        {
            if (shower.ShowerDate != null)
            {
                Pet pet = db.Pets.Find(shower.PetId);
                if (pet == null)
                {
                    shower.PetId = 0;
                    shower.PetName = String.Empty;
                    shower.PetSpecie = String.Empty;
                    shower.Owner = String.Empty;
                    shower.OwnerPhone = String.Empty;
                }
                else
                {
                    shower.PetName = pet.PetName;
                    shower.Owner = pet.Owner.OwnerLastName + ", " + pet.Owner.OwnerName;
                    shower.OwnerPhone = pet.Owner.OwnerPhone;

                    if (pet.PetSpecie == Species.Perro)
                    {
                        shower.PetSpecie = "Perro";
                    }
                    else
                    {
                        shower.PetSpecie = "Gato";
                    }
                }

                db.Entry(shower).State = EntityState.Modified;
                db.SaveChanges();

                return new JsonResult { Data = new { status = true, petId = shower.PetId, petName = shower.PetName, owner = shower.Owner, specie = shower.PetSpecie, date = shower.ShowerDate.ToShortDateString() } };
            }
            else
            {
                return new JsonResult { Data = new { status = false } };
            }

            
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
