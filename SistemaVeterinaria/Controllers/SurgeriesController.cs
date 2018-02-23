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
            ViewBag.SurgeryTypes = db.SurgeryTypes.ToList();
            if (db.SurgeryTypes.Any())
            {
                ViewBag.LastSurgeryTypeId = db.SurgeryTypes.ToList().Last().SurgeryTypeId;
            }
            else
            {
                ViewBag.LastSurgeryTypeId = 0;
            }

            ViewBag.Pets = db.Pets.ToList();
            return View(surgeries.ToList());
        }

        public ActionResult DailyNotification()
        {
            return View(db.Surgeries.ToList().FindAll(s => s.SurgeryDate == DateTime.Today));
        }

        [HttpPost]
        public JsonResult CreateSurgeryType(SurgeryType surgeryType)
        {
            var exist = db.SurgeryTypes.ToList().Exists(st => st.SurgeryTypeName == surgeryType.SurgeryTypeName);
            if (exist)
            {
                return new JsonResult { Data = new { status = false } };
            }
            else
            {
                db.SurgeryTypes.Add(surgeryType);
                db.SaveChanges();

                return new JsonResult { Data = new { status = true, surgeryTypeId = surgeryType.SurgeryTypeId} };
            }
        }

        [HttpPost]
        public JsonResult EditSurgeryType(int surgerytypeId, string surgerytypename)
        {
            var status = true;
            var exist = db.SurgeryTypes.ToList().Exists(st => st.SurgeryTypeName == surgerytypename & st.SurgeryTypeId != surgerytypeId);
            if (exist)
            {
                status = false;
            }
            else
            {
                SurgeryType surgeryType = db.SurgeryTypes.Find(surgerytypeId);
                if (surgeryType == null)
                {
                    status = false;
                }
                else
                {
                    surgeryType.SurgeryTypeName = surgerytypename;
                    db.Entry(surgeryType).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }

            return Json(status, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteSurgeryType(int surgeryTypeId)
        {
            SurgeryType surgeryType = db.SurgeryTypes.Find(surgeryTypeId);
            if (surgeryType != null & !surgeryType.Surgeries.Any())
            {
                db.SurgeryTypes.Remove(surgeryType);
                db.SaveChanges();

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CreateSurgery(Surgery surgery)
        {
            surgery.Pet = db.Pets.Find(surgery.PetId);
            if (surgery.Pet != null)
            {
                surgery.SurgeryType = db.SurgeryTypes.Find(surgery.SurgeryTypeId);
                if (surgery.SurgeryType != null)
                {
                    db.Surgeries.Add(surgery);
                    db.SaveChanges();

                    string specie;
                    if (surgery.Pet.PetSpecie == Species.Perro)
                    {
                        specie = "Perro";
                    }
                    else
                    {
                        specie = "Gato";
                    }

                    string sex;
                    if (surgery.Pet.PetSex)
                    {
                        sex = "Macho";
                    }
                    else
                    {
                        sex = "Hembra";
                    }

                    return new JsonResult { Data = new { status = true, surgeryId = surgery.SurgeryId, surgeryType = surgery.SurgeryType.SurgeryTypeName, petName = surgery.Pet.PetName, petSpecie = specie, petSex = sex, owner = surgery.Pet.Owner.OwnerFullName, date = surgery.SurgeryDate.ToString("yyyy-MM-dd") } };
                }
            }
            return new JsonResult { Data = new { status = false } };
        }

        public JsonResult EditSurgery(Surgery surgery)
        {
            if (db.Pets.Find(surgery.PetId) != null & surgery.SurgeryDate != null)
            {
                if (db.SurgeryTypes.Find(surgery.SurgeryTypeId) != null)
                {
                    surgery.Pet = db.Pets.Find(surgery.PetId);
                    surgery.SurgeryType = db.SurgeryTypes.Find(surgery.SurgeryTypeId);

                    db.Entry(surgery).State = EntityState.Modified;
                    db.SaveChanges();

                    var specie = String.Empty;
                    if (surgery.Pet.PetSpecie == Species.Perro)
                    {
                        specie = "Perro";
                    }
                    else
                    {
                        specie = "Gato";
                    }

                    var sex = String.Empty;
                    if (surgery.Pet.PetSex)
                    {
                        sex = "Macho";
                    }
                    else
                    {
                        sex = "Hembra";
                    }

                    return new JsonResult { Data = new { status = true, date = surgery.SurgeryDate.ToString("yyyy-MM-dd"), owner = surgery.Pet.Owner.OwnerFullName, pet = surgery.Pet.PetName, specie = specie, sex = sex, surgeryTypeId = surgery.SurgeryTypeId, surgeryType = surgery.SurgeryType.SurgeryTypeName} };
                }
            }

            return new JsonResult { Data = new { status = false } };
        }

        [HttpPost]
        public JsonResult DeleteSurgery(int surgeryId)
        {
            Surgery surgery = db.Surgeries.Find(surgeryId);
            if (surgery != null)
            {
                db.Surgeries.Remove(surgery);
                db.SaveChanges();

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
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
