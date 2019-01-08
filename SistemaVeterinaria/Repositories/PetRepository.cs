using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaVeterinaria.Repositories
{
    public class PetRepository
    {
        public dynamic CalculateAge(DateTime petBirthday)
        {
            var age = String.Empty;
            var years = DateTime.Today.Year - petBirthday.Year;
            var months = Math.Abs(DateTime.Today.Month - petBirthday.Month);

            if (DateTime.Today.DayOfYear < petBirthday.DayOfYear) //Pregunto si todavía no cumplió años
            {
                years--;
                months = 12 - Math.Abs(DateTime.Today.Month - petBirthday.Month);
                if (DateTime.Today.Month == petBirthday.Month)
                {
                    months--;
                }
            }

            if (years > 0)
            {
                age = years.ToString() + " año";
                if (years > 1)
                {
                    age += "s";
                }
                if (months > 0)
                {
                    age += " y " + months.ToString() + " mes";
                    if (months > 1)
                    {
                        age += "es";
                    }
                }
                
                if (years > 30)
                {
                    return String.Empty;
                }
            }
            else
            {
                age = months.ToString() + " mes";
                if (months > 1)
                {
                    age += "es";
                }
            }

            return age;
        }
    }
}