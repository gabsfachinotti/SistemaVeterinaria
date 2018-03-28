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
using SistemaVeterinaria.ViewModels;

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
                if (db.Showers.ToList().Count(s => s.ShowerDate == showerDate & s.ShowerTurn == showerTurn) > 1)
                {
                    //Sin disponibilidad en el turno
                    data = 2;
                }
                else
                {
                    if (db.Showers.ToList().Exists(s => s.ShowerDate == showerDate & s.ShowerTurn == showerTurn & s.PetId == petId) & petId > 0)
                    {
                        //Ya existe baño para ese paciente en el día y turno especificado.
                        data = 3;
                    }
                }
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidateEditShower(DateTime showerDate, bool showerTurn, int petId, int showerId)
        {
            var data = 0; //Validado
            if (db.Showers.ToList().Count(sh => sh.ShowerDate == showerDate & sh.ShowerId != showerId) > 3)
            {
                //Sin disponibilidad para la fecha
                data = 1;
            }
            else
            {
                if (db.Showers.ToList().Count(sh => sh.ShowerDate == showerDate & sh.ShowerTurn == showerTurn & sh.ShowerId != showerId) > 1)
                {
                    //Sin disponibilidad para el turno
                    data = 2;
                }
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateShower(Shower shower)
        {
            if (shower.ShowerDate != null)
            {
                Pet pet = db.Pets.Find(shower.PetId);

                if (pet != null)
                {
                    shower.PetName = pet.PetName;
                    shower.Owner = pet.Owner.OwnerFullName;
                    shower.OwnerPhone = pet.Owner.OwnerPhone;

                    if (pet.PetSpecie == Species.Canina)
                    {
                        shower.PetSpecie = "Canina";
                    }
                    else
                    {
                        shower.PetSpecie = "Felina";
                    }
                }

                db.Showers.Add(shower);
                db.SaveChanges();                

                return new JsonResult { Data = new { status = true, showerId = shower.ShowerId, petId = shower.PetId, petname = shower.PetName, owner = shower.Owner, ownerPhone = shower.OwnerPhone, specie = shower.PetSpecie, date = shower.ShowerDate.ToString("yyyy-MM-dd"), dateTitle = shower.ShowerDate.ToString("D") } };
            }
            else
            {
                return new JsonResult { Data = new { status = false } };
            }
        }

        public JsonResult CreateOwnerAndShower(OwnerPetShower ownerPetShower)
        {
            if (ownerPetShower.Shower.ShowerDate != null)
            {
                Owner owner = ownerPetShower.Owner;                
                db.Owners.Add(owner);
                db.SaveChanges();

                Pet pet = ownerPetShower.Pet;
                pet.OwnerId = owner.OwnerId;
                pet.Owner = owner;               
                pet.PetBreed = String.Empty;
                pet.PetColor = String.Empty;
                db.Pets.Add(pet);
                db.SaveChanges();

                ownerPetShower.Shower.PetId = pet.PetId;

                db.Showers.Add(ownerPetShower.Shower);
                db.SaveChanges();

                return new JsonResult { Data = new { status = true, showerId = ownerPetShower.Shower.ShowerId, petId = pet.PetId, petname = pet.PetName, owner = owner.OwnerFullName, ownerPhone = owner.OwnerPhone, specie = ownerPetShower.Shower.PetSpecie, date = ownerPetShower.Shower.ShowerDate.ToString("yyyy-MM-dd"), dateTitle = ownerPetShower.Shower.ShowerDate.ToString("D") } };
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
                if (pet != null)
                {
                    shower.PetName = pet.PetName;
                    shower.Owner = pet.Owner.OwnerFullName;
                    shower.OwnerPhone = pet.Owner.OwnerPhone;

                    if (pet.PetSpecie == Species.Canina)
                    {
                        shower.PetSpecie = "Canina";
                    }
                    else
                    {
                        shower.PetSpecie = "Felina";
                    }
                }

                db.Entry(shower).State = EntityState.Modified;
                db.SaveChanges();

                return new JsonResult { Data = new { status = true, petId = shower.PetId, petName = shower.PetName, owner = shower.Owner, ownerPhone = shower.OwnerPhone, specie = shower.PetSpecie, date = shower.ShowerDate.ToString("yyyy-MM-dd"), dateTitle = shower.ShowerDate.ToString("D") } };
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
