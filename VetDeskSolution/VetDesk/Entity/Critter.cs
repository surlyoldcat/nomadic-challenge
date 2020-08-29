using System;
using System.Collections.Generic;

namespace VetDesk.Entity
{
    public partial class Critter
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public decimal LastWeight { get; set; }
        public int CritterTypeId { get; set; }
        public string Color { get; set; }
        public byte[] Photo { get; set; }

        public virtual CritterType CritterType { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
