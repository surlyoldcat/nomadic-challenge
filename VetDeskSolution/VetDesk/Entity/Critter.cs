using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VetDesk.Entity
{
    public partial class Critter
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal LastWeight { get; set; }

        [Required]
        public int CritterTypeId { get; set; }

        [Required]
        public string Color { get; set; }
        public int PhotoId { get; set; }

        public virtual CritterType CritterType { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
