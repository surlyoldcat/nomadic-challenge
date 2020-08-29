using System;
using System.Collections.Generic;

namespace VetDesk.Entity
{
    public partial class Customer
    {
        public Customer()
        {
            Critters = new HashSet<Critter>();
        }

        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public virtual ICollection<Critter> Critters { get; set; }
    }
}
