using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VetDesk.Entity
{
    public class Photo
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] PhotoFile { get; set; }
    }
}
