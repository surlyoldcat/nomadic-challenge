using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VetDesk.Entity
{
    public partial class Customer
    {
        public Customer()
        {
            Critters = new HashSet<Critter>();
        }

        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        public virtual ICollection<Critter> Critters { get; set; }
    }
}
