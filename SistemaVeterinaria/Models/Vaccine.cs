using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaVeterinaria.Models
{
    public class Vaccine
    {
        [Key]
        public int VaccineId { get; set; }

        public int VaccineNumber { get; set; }

        public DateTime VaccineDate { get; set; }

        public int PetId { get; set; }

        public virtual Pet Pet { get; set; }
    }
}