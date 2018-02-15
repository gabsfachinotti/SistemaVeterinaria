using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaVeterinaria.Models
{
    public class Surgery
    {
        [Key]
        public int SurgeryId { get; set; }

        public int SurgeryTypeId { get; set; }

        public virtual SurgeryType SurgeryType { get; set; }

        public DateTime SurgeryDate { get; set; }

        public int PetId { get; set; }

        public virtual Pet Pet { get; set; }
    }
}