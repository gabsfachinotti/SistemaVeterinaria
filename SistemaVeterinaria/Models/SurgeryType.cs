using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaVeterinaria.Models
{
    public class SurgeryType
    {
        [Key]
        public int SurgeryTypeId { get; set; }

        public string SurgeryTypeName { get; set; }

        public virtual ICollection<Surgery> Surgeries { get; set; }
    }
}