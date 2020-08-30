using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VetDesk.Entity;

namespace VetDesk.Models
{
    public class CritterListModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Customer { get; set; }
        public string Type { get; set; }
        public string ImageUrl { get; set; }

        
    }
}
