using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaVeterinaria.Models
{
    public class ClinicHistory
    {
        [Key]
        public int ClinicHistoryId { get; set; }

        public int ClinicHistoryNumber { get; set; }

        public string ClinicHistoryData { get; set; }

        public int PetId { get; set; }

        public virtual Pet Pet { get; set; }
    }
}