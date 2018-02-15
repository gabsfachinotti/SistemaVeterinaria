using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaVeterinaria.Models
{
    public class Owner
    {
        [Key]
        public int OwnerId { get; set; }

        public string OwnerName { get; set; }

        public string OwnerLastName { get; set; }

        public string OwnerAddress { get; set; }

        public string OwnerPhone { get; set; }

        public virtual ICollection<Pet> Pets { get; set; }
    }
}