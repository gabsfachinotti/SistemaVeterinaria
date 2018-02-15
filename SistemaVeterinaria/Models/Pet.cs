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

        public int OwnerId { get; set; } //Clave foránea Owner

        public virtual Owner Owner { get; set; }

        public virtual ICollection<Vaccine> Vaccinations { get; set; }

        public virtual ICollection<Surgery> Surgeries { get; set; }

        public virtual ICollection<Shower> Showers { get; set; }
    }
}