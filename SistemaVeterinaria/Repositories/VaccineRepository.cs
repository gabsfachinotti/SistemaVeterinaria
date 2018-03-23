using SistemaVeterinaria.Context;
using SistemaVeterinaria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaVeterinaria.Repositories
{
    public class VaccineRepository
    {
        //private VeterinaryContext db = new VeterinaryContext();
        ////VaccineRepository vaccineRepository = new VaccineRepository();

        //public void CreateVaccineNumberOne(Vaccine v)
        //{
        //    if (v.VaccineDate.DayOfWeek == DayOfWeek.Sunday)
        //    {
        //        v.VaccineDate = v.VaccineDate.AddDays(1);
        //    }
            
        //    db.Vaccines.Add(v);
        //    db.SaveChanges();

        //    Vaccine vaccine2 = new Vaccine();
        //    vaccine2.VaccineNumber = v.VaccineNumber + 1;
        //    vaccine2.PetId = v.PetId;
        //    vaccine2.Pet = v.Pet;

        //    if (vaccine2.VaccineNumber < 5)
        //    {
        //        vaccine2.VaccineDate = v.VaccineDate.AddDays(21);
        //        CreateVaccineNumberOne(vaccine2);
        //    }
        //    else
        //    {
        //        vaccine2.VaccineDate = v.VaccineDate.AddMonths(1);
        //        CreateVaccineNumberFive(vaccine2);
        //    }            
        //}

        //public void CreateVaccineNumberFive(Vaccine vaccine)
        //{
        //    if (vaccine.VaccineDate.DayOfWeek == DayOfWeek.Sunday)
        //    {
        //        vaccine.VaccineDate = vaccine.VaccineDate.AddDays(1);
        //    }
        //    db.Vaccines.Add(vaccine);
        //    db.SaveChanges();

        //    Vaccine vaccine6 = new Vaccine();
        //    vaccine6.VaccineNumber = vaccine.VaccineNumber + 1;
        //    vaccine6.VaccineDate = vaccine.VaccineDate.AddMonths(11);
        //    vaccine6.PetId = vaccine.PetId;
        //    vaccine6.Pet = vaccine.Pet;
        //    CreateVaccineNumberSix(vaccine6);
        //}

        //public void CreateVaccineNumberSix(Vaccine vaccine)
        //{
        //    if (vaccine.VaccineNumber < 25)
        //    {
        //        if (vaccine.VaccineDate.DayOfWeek == DayOfWeek.Sunday)
        //        {
        //            vaccine.VaccineDate = vaccine.VaccineDate.AddDays(1);
        //        }
        //        db.Vaccines.Add(vaccine);
        //        db.SaveChanges();

        //        Vaccine vaccine7 = new Vaccine();
        //        vaccine7.VaccineNumber = vaccine.VaccineNumber + 1;
        //        vaccine7.VaccineDate = vaccine.VaccineDate.AddYears(1);
        //        vaccine7.PetId = vaccine.PetId;
        //        vaccine7.Pet = vaccine.Pet;
        //        CreateVaccineNumberSix(vaccine7);
        //    }
        //}
    }
}