﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SistemaVeterinaria.Context;
using SistemaVeterinaria.Models;
using SistemaVeterinaria.Repositories;

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
            ViewBag.Title = "Vacunas Realizadas";
            ViewBag.Pets = db.Pets.ToList().FindAll(p => !p.Vaccinations.Any());
            ViewBag.AllPets = db.Pets.ToList();
            return View("Index", vaccines);
        }


        public ActionResult DailyNotification()
        {
            return View(db.Vaccines.ToList().FindAll(v => v.VaccineDate == DateTime.Today & v.VaccineNumber != 1));
        }

        [HttpPost]
        public JsonResult CreateVaccines(Vaccine vaccine)
        {
            if (!db.Vaccines.ToList().Exists(v => v.PetId == vaccine.PetId & v.VaccineNumber >= vaccine.VaccineNumber))
            {
                //db.Vaccines.RemoveRange(db.Vaccines.ToList().FindAll(v => v.PetId == vaccine.PetId & v.VaccineNumber >= vaccine.VaccineNumber));
                //db.SaveChanges();

                vaccine.Pet = db.Pets.Find(vaccine.PetId);

                if (vaccine.VaccineNumber < 5)
                {
                    CreateVaccineNumberOne(vaccine);
                }
                else
                {
                    if (vaccine.VaccineNumber == 5)
                    {
                        CreateVaccineNumberFive(vaccine);
                    }
                    else
                    {
                        CreateVaccineNumberSix(vaccine);
                    }
                }

                var vaccineId = vaccine.VaccineId;
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
                DateTime date = db.Vaccines.ToList().Find(v => v.PetId == vaccine.PetId & v.VaccineNumber == vaccine.VaccineNumber).VaccineDate;
                DateTime date2 = DateTime.MinValue;
                DateTime date3 = DateTime.MinValue;
                DateTime date4 = DateTime.MinValue;
                DateTime date5 = DateTime.MinValue;
                DateTime date6 = DateTime.MinValue;

                switch (vaccine.VaccineNumber)
                {
                    case 1:
                        date2 = vaccine.VaccineDate.AddDays(21);
                        if (date2.DayOfWeek == DayOfWeek.Sunday)
                        {
                            date2 = vaccine.VaccineDate.AddDays(1);
                        }
                        date3 = date2.AddDays(21);
                        if (date3.DayOfWeek == DayOfWeek.Sunday)
                        {
                            date3 = date3.AddDays(1);
                        }
                        date4 = date3.AddDays(21);
                        if (date4.DayOfWeek == DayOfWeek.Sunday)
                        {
                            date4 = date4.AddDays(1);
                        }
                        date5 = date4.AddMonths(1);
                        if (date5.DayOfWeek == DayOfWeek.Sunday)
                        {
                            date5 = date5.AddDays(1);
                        }
                        date6 = date4.AddYears(1);
                        if (date6.DayOfWeek == DayOfWeek.Sunday)
                        {
                            date6 = date6.AddDays(1);
                        }
                        break;
                    case 2:
                        date3 = date.AddDays(21);
                        if (date3.DayOfWeek == DayOfWeek.Sunday)
                        {
                            date3 = date3.AddDays(1);
                        }
                        date4 = date3.AddDays(21);
                        if (date4.DayOfWeek == DayOfWeek.Sunday)
                        {
                            date4 = date4.AddDays(1);
                        }
                        date5 = date4.AddMonths(1);
                        if (date5.DayOfWeek == DayOfWeek.Sunday)
                        {
                            date5 = date5.AddDays(1);
                        }
                        date6 = date4.AddYears(1);
                        if (date6.DayOfWeek == DayOfWeek.Sunday)
                        {
                            date6 = date6.AddDays(1);
                        }
                        break;
                    case 3:
                        date4 = date.AddDays(21);
                        if (date4.DayOfWeek == DayOfWeek.Sunday)
                        {
                            date4 = date4.AddDays(1);
                        }
                        date5 = date4.AddMonths(1);
                        if (date5.DayOfWeek == DayOfWeek.Sunday)
                        {
                            date5 = date5.AddDays(1);
                        }
                        date6 = date4.AddYears(1);
                        if (date6.DayOfWeek == DayOfWeek.Sunday)
                        {
                            date6 = date6.AddDays(1);
                        }
                        break;
                    case 4:
                        date5 = date.AddMonths(1);
                        if (date5.DayOfWeek == DayOfWeek.Sunday)
                        {
                            date5 = date5.AddDays(1);
                        }
                        date6 = date.AddMonths(11);
                        if (date6.DayOfWeek == DayOfWeek.Sunday)
                        {
                            date6 = date6.AddDays(1);
                        }
                        break;
                    case 5:
                        date6 = date.AddMonths(11);
                        if (date6.DayOfWeek == DayOfWeek.Sunday)
                        {
                            date6 = date6.AddDays(1);
                        }
                        break;
                }

                return new JsonResult { Data = new { status = true, vaccineId = vaccineId, owner = vaccine.Pet.Owner.OwnerFullName, pet = pet, specie = specie, date = date.ToString("yyyy-MM-dd") + "." + date.ToString("D"), date2 = date2.ToString("yyyy-MM-dd") + "." + date2.ToString("D"), date3 = date3.ToString("yyyy-MM-dd") + "." + date3.ToString("D"), date4 = date4.ToString("yyyy-MM-dd") + "." + date4.ToString("D"), date5 = date5.ToString("yyyy-MM-dd") + "." + date5.ToString("D"), date6 = date6.ToString("yyyy-MM-dd") + "." + date6.ToString("D") } };
            }

            return new JsonResult { Data = new { status = false } };
        }

        public JsonResult CreateVaccine(Vaccine vaccine)
        {
            if (!db.Vaccines.ToList().Exists(v => v.PetId == vaccine.PetId & v.VaccineNumber == vaccine.VaccineNumber))
            {
                vaccine.Pet = db.Pets.Find(vaccine.PetId);
                db.Vaccines.Add(vaccine);
                db.SaveChanges();

                var specie = "Perro";
                if (vaccine.Pet.PetSpecie == Species.Gato)
                {
                    specie = "Gato";
                }

                return new JsonResult { Data = new { status = true, vaccineId = vaccine.VaccineId, owner = vaccine.Pet.Owner.OwnerFullName, pet = vaccine.Pet.PetName, specie = specie, date = vaccine.VaccineDate.ToString("yyyy-MM-dd") + "." + vaccine.VaccineDate.ToString("D") } };
            }

            return new JsonResult { Data = new { status = false } };
        }

        public JsonResult ValidateVaccine(Vaccine vaccine)
        {
            var status = true;
            var message = String.Empty;

            if (db.Vaccines.ToList().Exists(v => v.PetId == vaccine.PetId & v.VaccineNumber == vaccine.VaccineNumber))
            {
                status = false;
                message = "Ya existe una vacuna con el Ordinal ingresado";
            }

            if (db.Vaccines.ToList().Exists(v => v.PetId == vaccine.PetId & v.VaccineNumber < vaccine.VaccineNumber & v.VaccineDate > vaccine.VaccineDate))
            {
                status = false;
                message = "No es posible agregar una vacuna con fecha inferior a la de su Nro Ordinal anterior";
            }

            if (db.Vaccines.ToList().Exists(v => v.PetId == vaccine.PetId & v.VaccineNumber > vaccine.VaccineNumber & v.VaccineDate < vaccine.VaccineDate))
            {
                status = false;
                message = "No es posible agregar una vacuna con fecha superior a la de su Nro Ordinal posterior";
            }

            if (status)
            {
                vaccine.Pet = db.Pets.Find(vaccine.PetId);
                db.Vaccines.Add(vaccine);
                db.SaveChanges();
            }

            var position = 1;
            if (db.Vaccines.ToList().Exists(v => v.PetId == vaccine.PetId & v.VaccineNumber < vaccine.VaccineNumber))
            {
                position = db.Vaccines.ToList().OrderBy(v => v.VaccineNumber).Last(v => v.PetId == vaccine.PetId & v.VaccineNumber < vaccine.VaccineNumber).VaccineNumber;
            }            

            return new JsonResult { Data = new { status = status, message = message, position = position, vaccineId = vaccine.VaccineId } };
        }

        public JsonResult ValidateEditVaccine(Vaccine vaccine)
        {
            var status = true;
            var message = String.Empty;

            if (db.Vaccines.ToList().Exists(v => v.PetId == vaccine.PetId & v.VaccineNumber < vaccine.VaccineNumber & v.VaccineDate > vaccine.VaccineDate))
            {
                status = false;
                message = "No es posible modificar la fecha de una vacuna a una fecha inferior a la de su Nro Ordinal anterior";
            }

            if (db.Vaccines.ToList().Exists(v => v.PetId == vaccine.PetId & v.VaccineNumber > vaccine.VaccineNumber & v.VaccineDate < vaccine.VaccineDate))
            {
                status = false;
                message = "No es posible modificar la fecha de una vacuna a una fecha superior a la de su siguiente Nro Ordinal";
            }

            if (status)
            {
                var editVaccine = db.Vaccines.Find(vaccine.VaccineId);
                editVaccine.VaccineDate = vaccine.VaccineDate;
                db.Entry(editVaccine).State = EntityState.Modified;
                db.SaveChanges();
            }

            return new JsonResult { Data = new { status = status, message = message, vaccineId = vaccine.VaccineId }};
        }

        [HttpPost]
        public JsonResult EditVaccine (Vaccine vaccine)
        {
            var vaccineId = vaccine.VaccineId;
            List<string> vaccineDates = new List<string>();

            var v = db.Vaccines.Find(vaccine.VaccineId);
            v.VaccineDate = vaccine.VaccineDate;
            if (v.VaccineDate.DayOfWeek == DayOfWeek.Sunday)
            {
                v.VaccineDate = v.VaccineDate.AddDays(1);
            }
            db.Entry(v).State = EntityState.Modified;
            db.SaveChanges();
            vaccineDates.Add(v.VaccineDate.ToString("yyyy-MM-dd") + "." + v.VaccineDate.ToString("D"));

            //Guardo el próximo numero de vacuna a actualizar para modificar las posteriores
            var pet = v.Pet;
            Vaccine nextvaccine = new Vaccine();
            var lastnotification = v.VaccineId;

            foreach (var item in pet.Vaccinations.ToList().Where(va => va.VaccineNumber > v.VaccineNumber).OrderBy(va => va.VaccineNumber))
            {
                //nextvaccine.PetId = vaccine.PetId;
                //nextvaccine.Pet = pet;
                nextvaccine = item;
                lastnotification = nextvaccine.VaccineId;

                switch (item.VaccineNumber)
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
                        nextvaccine.VaccineDate = db.Vaccines.ToList().Find(va => va.PetId == vaccine.PetId & va.VaccineNumber == 4).VaccineDate.AddYears(1);
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

                vaccineDates.Add(nextvaccine.VaccineDate.ToString("yyyy-MM-dd") + "." + nextvaccine.VaccineDate.ToString("D"));

                vaccine = nextvaccine;
            }

            var petName = v.Pet.PetName;
            var specie = String.Empty;
            if (v.Pet.PetSpecie == Species.Perro)
            {
                specie = "Perro";
            }
            else
            {
                specie = "Gato";
            }

            return new JsonResult { Data = new { owner = v.Pet.Owner.OwnerFullName, pet = petName, specie = specie, vaccineId = vaccineId, vaccineDates = vaccineDates, lastId = lastnotification } };
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
        public void CreateVaccineNumberOne(Vaccine v)
        {
            if (v.VaccineDate.DayOfWeek == DayOfWeek.Sunday)
            {
                v.VaccineDate = v.VaccineDate.AddDays(1);
            }

            db.Vaccines.Add(v);
            db.SaveChanges();

            Vaccine vaccine2 = new Vaccine();
            vaccine2.VaccineNumber = v.VaccineNumber + 1;
            vaccine2.PetId = v.PetId;
            vaccine2.Pet = v.Pet;

            if (vaccine2.VaccineNumber < 5)
            {
                vaccine2.VaccineDate = v.VaccineDate.AddDays(21);
                CreateVaccineNumberOne(vaccine2);
            }
            else
            {
                vaccine2.VaccineDate = v.VaccineDate.AddMonths(1);
                CreateVaccineNumberFive(vaccine2);
            }
          
        }

        public void CreateVaccineNumberFive(Vaccine vaccine)
        {
            if (vaccine.VaccineDate.DayOfWeek == DayOfWeek.Sunday)
            {
                vaccine.VaccineDate = vaccine.VaccineDate.AddDays(1);
            }
            db.Vaccines.Add(vaccine);
            db.SaveChanges();

            Vaccine vaccine6 = new Vaccine();
            vaccine6.VaccineNumber = vaccine.VaccineNumber + 1;
            vaccine6.VaccineDate = vaccine.VaccineDate.AddMonths(11);
            vaccine6.PetId = vaccine.PetId;
            vaccine6.Pet = vaccine.Pet;
            CreateVaccineNumberSix(vaccine6);
        }

        public void CreateVaccineNumberSix(Vaccine vaccine)
        {
            if (vaccine.VaccineNumber < 25)
            {
                if (vaccine.VaccineDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    vaccine.VaccineDate = vaccine.VaccineDate.AddDays(1);
                }
                db.Vaccines.Add(vaccine);
                db.SaveChanges();

                Vaccine vaccine7 = new Vaccine();
                vaccine7.VaccineNumber = vaccine.VaccineNumber + 1;
                vaccine7.VaccineDate = vaccine.VaccineDate.AddYears(1);
                vaccine7.PetId = vaccine.PetId;
                vaccine7.Pet = vaccine.Pet;
                CreateVaccineNumberSix(vaccine7);
            }
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
