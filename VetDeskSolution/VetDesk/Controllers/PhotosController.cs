using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VetDesk.Entity;
using VetDesk.Repository;

namespace VetDesk.Controllers
{
    [Route("api/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IPhotoRepository photoRepo;

        public PhotosController(IPhotoRepository repo)
        {
            photoRepo = repo;
        }
        
        // GET api/<PhotosController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var p = photoRepo.Fetch(id);
            if (null != p)
                return File(p.PhotoFile, p.ContentType);
            else
                return NotFound();
        }

        // POST api/<PhotosController>
        [HttpPost]
        public IActionResult Post(IFormFile imageFile)
        {
            var p = new Photo
            {
                ContentType = imageFile.ContentType,
                FileName = imageFile.FileName
            };
            using (MemoryStream ms = new MemoryStream())
            {
                imageFile.CopyTo(ms);
                p.PhotoFile = ms.ToArray();

            }
            var saved = photoRepo.Create(p);
            return new JsonResult(new { PhotoId = saved.Id });
        }
    }
}
