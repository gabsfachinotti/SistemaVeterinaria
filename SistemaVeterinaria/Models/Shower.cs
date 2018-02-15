using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaVeterinaria.Models
{
    public class Shower
    {
        [Key]
        public int ShowerId { get; set; }

        public DateTime ShowerDate { get; set; }

        public bool ShowerTurn { get; set; } // 0: mañana - 1: tarde

        public int PetId { get; set; }

        public virtual Pet Pet { get; set; }
    }
}