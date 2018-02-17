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
            return View(surgeries.ToList());
        }

        public ActionResult DailyNotification()
        {
            return View(db.Surgeries.ToList().FindAll(s => s.SurgeryDate == DateTime.Today));
        }

        [HttpPost]
        public JsonResult CreateSurgeryType(string surgerytypename)
        {
            var status = true;
            var exist = db.SurgeryTypes.ToList().Exists(st => st.SurgeryTypeName == surgerytypename);
            if (exist)
            {
                status = false;
            }
            else
            {
                SurgeryType surgeryType = new SurgeryType();
                surgeryType.SurgeryTypeName = surgerytypename;
                db.SurgeryTypes.Add(surgeryType);
                db.SaveChanges();
            }

            return Json(status, JsonRequestBehavior.AllowGet );
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
