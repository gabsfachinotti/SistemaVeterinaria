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
    public class VaccinesController : Controller
    {
        private VeterinaryContext db = new VeterinaryContext();

        // GET: Vaccines
        public ActionResult Index()
        {
            var vaccines = db.Vaccines.ToList().FindAll(v => v.VaccineDate >= DateTime.Today & v.VaccineDate < DateTime.Today.AddYears(2) & v.VaccineNumber > 1);
            ViewBag.Title = "Notificaciones Futuras de Vacunas";
            ViewBag.Pets = db.Pets.ToList();
            return View(vaccines);
        }

        public ActionResult Vaccinated()
        {
            var vaccines = db.Vaccines.ToList().FindAll(v => v.VaccineDate < DateTime.Today);
            ViewBag.Title = "Notificaciones Pasadas de Vacunas";
            ViewBag.Pets = db.Pets.ToList();
            return View("Index", vaccines);
        }


        public ActionResult DailyNotification()
        {
            return View(db.Vaccines.ToList().FindAll(v => v.VaccineDate == DateTime.Today & v.VaccineNumber != 1));
        }

        [HttpPost]
        public JsonResult CreateVaccine(Vaccine vaccine)
        {
            vaccine.Pet = db.Pets.Find(vaccine.PetId);

            db.Vaccines.Add(vaccine);
            db.SaveChanges();

            Vaccine vaccine2 = new Vaccine();
            vaccine2.VaccineNumber = 2;
            vaccine2.PetId = vaccine.PetId;
            vaccine2.VaccineDate = vaccine.VaccineDate.AddDays(21);
            if (vaccine2.VaccineDate.DayOfWeek == DayOfWeek.Sunday)
            {
                vaccine2.VaccineDate = vaccine2.VaccineDate.AddDays(1);
            }
            db.Vaccines.Add(vaccine2);
            db.SaveChanges();

            Vaccine vaccine3 = new Vaccine();
            vaccine3.VaccineNumber = 3;
            vaccine3.PetId = vaccine.PetId;
            vaccine3.VaccineDate = vaccine2.VaccineDate.AddDays(21);
            if (vaccine3.VaccineDate.DayOfWeek == DayOfWeek.Sunday)
            {
                vaccine3.VaccineDate = vaccine3.VaccineDate.AddDays(1);
            }
            db.Vaccines.Add(vaccine3);
            db.SaveChanges();

            Vaccine vaccine4 = new Vaccine();
            vaccine4.VaccineNumber = 4;
            vaccine4.PetId = vaccine.PetId;
            vaccine4.VaccineDate = vaccine3.VaccineDate.AddDays(21);
            if (vaccine4.VaccineDate.DayOfWeek == DayOfWeek.Sunday)
            {
                vaccine4.VaccineDate = vaccine4.VaccineDate.AddDays(1);
            }
            db.Vaccines.Add(vaccine4);
            db.SaveChanges();

            Vaccine vaccine5 = new Vaccine();
            vaccine5.VaccineNumber = 5;
            vaccine5.PetId = vaccine.PetId;
            vaccine5.VaccineDate = vaccine4.VaccineDate.AddMonths(1);
            if (vaccine5.VaccineDate.DayOfWeek == DayOfWeek.Sunday)
            {
                vaccine5.VaccineDate = vaccine5.VaccineDate.AddDays(1);
            }
            db.Vaccines.Add(vaccine5);
            db.SaveChanges();

            DateTime annualdate = vaccine4.VaccineDate.AddYears(1);
            int vaccinenumber = vaccine5.VaccineNumber + 1;
            Vaccine annualvaccine = new Vaccine();

            for (int i = 1; i <= 15; i++)
            {
                annualvaccine.VaccineNumber = vaccinenumber;
                annualvaccine.PetId = vaccine.PetId;
                annualvaccine.VaccineDate = annualdate;
                if (annualvaccine.VaccineDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    annualvaccine.VaccineDate = annualvaccine.VaccineDate.AddDays(1);
                }
                db.Vaccines.Add(annualvaccine);
                db.SaveChanges();

                vaccinenumber++;
                annualdate = annualvaccine.VaccineDate.AddYears(1);
            }

            var vaccineId = vaccine.VaccineId;
            var owner = vaccine.Pet.Owner.OwnerLastName + ", " + vaccine.Pet.Owner.OwnerName;
            var pet = vaccine.Pet.PetName;
            var specie = String.Empty;
            if (vaccine.Pet.PetSpecie == Species.Perro)
            {
                specie = "Perro";
            }
            else
            {
                specie = "Gato";
            }
            var date = vaccine.VaccineDate.ToString("yyyy-MM-dd");
            var date2 = vaccine2.VaccineDate.ToString("yyyy-MM-dd");
            var date3 = vaccine3.VaccineDate.ToString("yyyy-MM-dd");
            var date4 = vaccine4.VaccineDate.ToString("yyyy-MM-dd");
            var date5 = vaccine5.VaccineDate.ToString("yyyy-MM-dd");
            var date6 = vaccine4.VaccineDate.AddYears(1).ToString("yyyy-MM-dd");

            return new JsonResult { Data = new { vaccineId = vaccineId, owner = owner, pet = pet, specie = specie, date = date, date2 = date2, date3 = date3, date4 = date4, date5 = date5, date6 = date6 } };
        }

        [HttpPost]
        public JsonResult EditVaccine (Vaccine vaccine)
        {
            var vaccineId = vaccine.VaccineId;
            List<string> vaccineDates = new List<string>();

            vaccine.Pet = db.Pets.Find(vaccine.PetId);
            db.Entry(vaccine).State = EntityState.Modified;
            db.SaveChanges();
            vaccineDates.Add(vaccine.VaccineDate.ToString("yyyy-MM-dd"));

            //Guardo el próximo numero de vacuna a actualizar para modificar las posteriores
            var vaccinenumber = vaccine.VaccineNumber + 1;
            Vaccine nextvaccine = new Vaccine();

            while (vaccinenumber < 21)
            {
                nextvaccine = db.Vaccines.Find(vaccine.VaccineId + 1);

                if (nextvaccine != null)
                {
                    nextvaccine.PetId = vaccine.PetId;
                    nextvaccine.Pet = vaccine.Pet;

                    switch (vaccinenumber)
                    {
                        case 2:
                            nextvaccine.VaccineDate = vaccine.VaccineDate.AddDays(21);
                            break;
                        case 3:
                            nextvaccine.VaccineDate = vaccine.VaccineDate.AddDays(21);
                            break;
                        case 4:
                            nextvaccine.VaccineDate = vaccine.VaccineDate.AddDays(21);
                            break;
                        case 5:
                            nextvaccine.VaccineDate = vaccine.VaccineDate.AddMonths(1);
                            break;
                        case 6:
                            nextvaccine.VaccineDate = db.Vaccines.ToList().Find(v => v.PetId == vaccine.PetId & v.VaccineNumber == 4).VaccineDate.AddYears(1);
                            break;
                        default:
                            nextvaccine.VaccineDate = vaccine.VaccineDate.AddYears(1);
                            break;
                    }

                    if (nextvaccine.VaccineDate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        nextvaccine.VaccineDate = nextvaccine.VaccineDate.AddDays(1);
                    }
                    db.Entry(nextvaccine).State = EntityState.Modified;
                    db.SaveChanges();
                    
                    vaccineDates.Add(nextvaccine.VaccineDate.ToString("yyyy-MM-dd"));

                    vaccine = nextvaccine;
                }
                
                vaccinenumber++;
            }

            var lastnotification = nextvaccine.VaccineId;

            var owner = vaccine.Pet.Owner.OwnerLastName + ", " + vaccine.Pet.Owner.OwnerName;
            var petName = vaccine.Pet.PetName;
            var specie = String.Empty;
            if (vaccine.Pet.PetSpecie == Species.Perro)
            {
                specie = "Perro";
            }
            else
            {
                specie = "Gato";
            }

            return new JsonResult { Data = new { owner = owner, pet = petName, specie = specie, vaccineId = vaccineId, vaccineDates = vaccineDates, lastId = lastnotification } };
        }

        [HttpPost]
        public JsonResult DeleteVaccine(int vaccineId)
        {
            var status = false;
            Vaccine vaccine = db.Vaccines.Find(vaccineId);
            if (vaccine != null)
            {
                db.Vaccines.Remove(vaccine);
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
