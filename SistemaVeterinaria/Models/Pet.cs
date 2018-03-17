using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaVeterinaria.Models
{
    public class Pet
    {
        [Key]
        public int PetId { get; set; }

        public string PetName { get; set; }

        public Species PetSpecie { get; set; }

        public string PetBreed { get; set; }

        public bool PetSex { get; set; } // 0: hembra - 1: macho

        public DateTime PetBirthday { get; set; }

        //public string PetAge{ get { return (DateTime.Today.Year - PetBirthday.Year).ToString() + " años y " + Math.Abs(DateTime.Today.Month - PetBirthday.Month) + " meses"; } }

        public string PetColor { get; set; }

        public int OwnerId { get; set; } //Clave foránea Owner

        public virtual Owner Owner { get; set; }

        public virtual ICollection<Vaccine> Vaccinations { get; set; }

        public virtual ICollection<Surgery> Surgeries { get; set; }

        //public virtual ICollection<Shower> Showers { get; set; }
    }
}