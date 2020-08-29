using System;
using System.Collections.Generic;

namespace VetDesk.Entity
{
    public partial class CritterType
    {
        public CritterType()
        {
            Critter = new HashSet<Critter>();
        }

        public int Id { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Critter> Critter { get; set; }
    }
}
