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
    public class OwnersController : Controller
    {
        private VeterinaryContext db = new VeterinaryContext();

        // GET: Owners
        public ActionResult Index()
        {
            return View(db.Owners.ToList());
        }

        public JsonResult CreateOwner(Owner owner)
        {
            var status = true;
            if (owner.OwnerName != String.Empty & owner.OwnerLastName != String.Empty)
            {
                db.Owners.Add(owner);
                db.SaveChanges();
            }
            else
            {
                status = false;
            }

            return new JsonResult { Data = new { status = status, ownerId = owner.OwnerId} };
        }

        public JsonResult EditOwner(Owner owner)
        {
            var status = true;
            var exist = db.Owners.ToList().Exists(o => o.OwnerId == owner.OwnerId);
            if (exist & owner.OwnerName != String.Empty & owner.OwnerLastName != String.Empty)
            {
                Owner editOwner = db.Owners.Find(owner.OwnerId);
                editOwner.OwnerId = owner.OwnerId;
                editOwner.OwnerLastName = owner.OwnerLastName;
                editOwner.OwnerName = owner.OwnerName;
                editOwner.OwnerAddress = owner.OwnerAddress;
                editOwner.OwnerPhone = owner.OwnerPhone;

                db.Entry(editOwner).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                status = false;
            }

            return new JsonResult { Data = new { status = status } };
        }

        public JsonResult DeleteOwner(int ownerId)
        {
            Owner owner = db.Owners.Find(ownerId);
            bool status;
            if (owner != null & !owner.Pets.Any())
            {
                status = true;
                db.Owners.Remove(owner);
                db.SaveChanges();
            }
            else
            {
                status = false;
            }

            return new JsonResult { Data = new { status = status } };
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
