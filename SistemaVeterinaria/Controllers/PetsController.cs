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
    public class PetsController : Controller
    {
        private VeterinaryContext db = new VeterinaryContext();
        PetRepository petRepository = new PetRepository();

        // GET: Pets
        public ActionResult Index()
        {
            var pets = db.Pets.Include(p => p.Owner);

            ViewBag.Owners = db.Owners.ToList();
            return View(pets.ToList());
        }

        // GET: Pets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pet pet = db.Pets.Find(id);
            if (pet == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.Age = petRepository.CalculateAge(pet.PetBirthday);

            ViewBag.Showers = db.Showers.ToList().FindAll(s => s.PetId == pet.PetId);
            return View(pet);
        }

        public JsonResult CreatePet(Pet pet)
        {
            pet.Owner = db.Owners.Find(pet.OwnerId);
            db.Pets.Add(pet);
            db.SaveChanges();

            return new JsonResult { Data = new { PetId = pet.PetId, OwnerName = pet.Owner.OwnerFullName } };
        }

        public JsonResult EditPet(Pet pet)
        {
            pet.Owner = db.Owners.Find(pet.OwnerId);

            db.Entry(pet).State = EntityState.Modified;
            db.SaveChanges();

            return new JsonResult { Data = new { OwnerId = pet.OwnerId, OwnerName = pet.Owner.OwnerFullName, Birthday = pet.PetBirthday.ToString("yyyy-MM-dd"), Color = pet.PetColor } };
        }

        public JsonResult ValidatePet(int petId, string petName, int ownerId, int petSpecie, int petSex)
        {
            var specie = Species.Perro;
            if (petSpecie == 2)
            {
                specie = Species.Gato;
            }

            var sex = false;
            if (petSex == 1)
            {
                sex = true;
            }

            var exist = db.Pets.ToList().Exists(p => p.PetName == petName & p.OwnerId == ownerId & p.PetSpecie == specie & p.PetSex == sex & p.PetId != petId);

            return Json(exist, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeletePet(int petId)
        {
            Pet pet = db.Pets.Find(petId);
            bool status;
            if (pet != null)
            {
                status = true;
                db.Pets.Remove(pet);
                db.SaveChanges();
            }
            else
            {
                status = false;
            }

            return new JsonResult { Data = new { status = status}};
        }

        public void CreateClinicHistory(ClinicHistory clinicHistory)
        {
            db.ClinicHistories.Add(clinicHistory);
            db.SaveChanges();
        }

        public void EditClinicHistory(ClinicHistory clinicHistory)
        {
            db.Entry(clinicHistory).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void DeleteClinicHistory(ClinicHistory clinicHistoryId)
        {
            ClinicHistory clinicHistory = db.ClinicHistories.Find(clinicHistoryId);
            if (clinicHistory != null)
            {
                db.ClinicHistories.Remove(clinicHistory);
                db.SaveChanges();
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
