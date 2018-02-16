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

namespace SistemaVeterinaria.Controllers
{
    public class PetsController : Controller
    {
        private VeterinaryContext db = new VeterinaryContext();

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
            return View(pet);
        }

        public JsonResult CreatePet(Pet pet)
        {
            pet.Owner = db.Owners.Find(pet.OwnerId);
            db.Pets.Add(pet);
            db.SaveChanges();

            return new JsonResult { Data = new { PetId = pet.PetId, OwnerName = pet.Owner.OwnerLastName + ", " + pet.Owner.OwnerName } };
        }

        public JsonResult EditPet(Pet pet)
        {
            Pet editPet = db.Pets.Find(pet.PetId);
            editPet.PetName = pet.PetName;
            editPet.PetSpecie = pet.PetSpecie;
            editPet.PetBreed = pet.PetBreed;
            editPet.PetSex = pet.PetSex;
            editPet.OwnerId = pet.OwnerId;
            editPet.Owner = db.Owners.Find(pet.OwnerId);

            db.Entry(editPet).State = EntityState.Modified;
            db.SaveChanges();

            return new JsonResult { Data = new { OwnerName = editPet.Owner.OwnerLastName + ", " + editPet.Owner.OwnerName } };
        }

        public JsonResult ValidatePet(string petName, int ownerId, int petSpecie, int petSex)
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

            var exist = db.Pets.ToList().Exists(p => p.PetName == petName & p.OwnerId == ownerId & p.PetSpecie == specie & p.PetSex == sex);

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