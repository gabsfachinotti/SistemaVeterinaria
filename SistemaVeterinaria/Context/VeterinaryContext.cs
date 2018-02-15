using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SistemaVeterinaria.Context
{
    public class VeterinaryContext: DbContext
    {
        public VeterinaryContext()
            : base("name=VeterinaryContext")
        {

        }

        public System.Data.Entity.DbSet<SistemaVeterinaria.Models.Owner> Owners { get; set; }

        public System.Data.Entity.DbSet<SistemaVeterinaria.Models.Pet> Pets { get; set; }

        public System.Data.Entity.DbSet<SistemaVeterinaria.Models.Surgery> Surgeries { get; set; }

        public System.Data.Entity.DbSet<SistemaVeterinaria.Models.SurgeryType> SurgeryTypes { get; set; }

        public System.Data.Entity.DbSet<SistemaVeterinaria.Models.Vaccine> Vaccines { get; set; }

        public System.Data.Entity.DbSet<SistemaVeterinaria.Models.Shower> Showers { get; set; }
    }
}