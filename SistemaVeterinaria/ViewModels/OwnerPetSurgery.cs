using SistemaVeterinaria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaVeterinaria.ViewModels
{
    public class OwnerPetSurgery
    {
        public Surgery Surgery { get; set; }

        public Owner Owner { get; set; }

        public Pet Pet { get; set; }
    }
}