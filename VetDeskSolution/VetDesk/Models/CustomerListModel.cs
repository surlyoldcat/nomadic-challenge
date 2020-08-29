using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VetDesk.Entity;

namespace VetDesk.Models
{
    public class CustomerListModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int NumCritters { get; set; }

        public static CustomerListModel CreateFrom(Customer ent)
        {
            return new CustomerListModel
            {
                Id = ent.Id,
                Name = ent.FullName,
                Phone = ent.Phone,
                NumCritters = ent.Critters.Count()
            };
        }
    }

   
}
