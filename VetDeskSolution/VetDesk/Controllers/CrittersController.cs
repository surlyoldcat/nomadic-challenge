﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetDesk.Entity;
using VetDesk.Models;
using VetDesk.Repository;

namespace VetDesk.Controllers
{
    public class CrittersController : Controller
    {
        private readonly ICritterRepository critterRepo;
        private readonly ICustomerRepository customerRepo;
        private readonly IPhotoRepository photoRepo;

        public CrittersController(ICustomerRepository custRepo, ICritterRepository critRepo, IPhotoRepository phRepo)
        {
            critterRepo = critRepo;
            customerRepo = custRepo;
            photoRepo = phRepo;
        }

        // GET: Critters
        public IActionResult Index()
        {
            var q = critterRepo.ListCritters(ListFetchOptions.DefaultOptions)
                .OrderBy(cr => cr.Name)
                .Select(cr => new CritterListModel
                {
                    Id = cr.Id,
                    Name = cr.Name,
                    Customer = cr.Customer.FullName,
                    Type = cr.CritterType.Description,
                    ImageUrl = Url.Action("Get", "Photos", new { Id = cr.PhotoId})
                });
            
            return View(q.ToList());
        }

       
        // GET: Critters/Create?customerId=5
        public IActionResult Create(int? customerId)
        {
            ViewData["CritterTypeId"] = BuildCritterTypeSelectList();
            
            ViewData["CustomerId"] = BuildCustomerSelectList(customerId);


            return View();
        }

        // GET: Critters/Create
        //public IActionResult Create()
        //{
        //    ViewData["CritterTypeId"] = new SelectList(_context.CritterTypes, "Id", "Description");
        //    ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Email");
        //    return View();
        //}

        // POST: Critters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,CustomerId,Name,LastWeight,CritterTypeId,Color")] Critter critter)
        {
            if (!HttpContext.Request.Form.Files.Any())
            {
                ModelState.AddModelError("PhotoId", "Image file is missing");
            }

            if (ModelState.IsValid)
            {
                IFormFile photo = HttpContext.Request.Form.Files[0];   
                int photoId = InsertPhoto(photo);
                critter.PhotoId = photoId;
                critterRepo.Create(critter);
                //we want to go back to the Customer edit page
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Edit", "Customers", new { id = critter.CustomerId });
            }
            ViewData["CritterTypeId"] = BuildCritterTypeSelectList(critter.CritterTypeId);
            ViewData["CustomerId"] = BuildCustomerSelectList(critter.CustomerId);
            
            return View(critter);
            
            
        }
        // GET: Critters/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var critter = critterRepo.FetchCritter(id.Value) ;
            if (critter == null)
            {
                return NotFound();
            }
            ViewData["CritterTypeId"] = BuildCritterTypeSelectList(critter.CritterTypeId);
            ViewData["CustomerId"] = BuildCustomerSelectList(critter.CustomerId);
            return View(critter);
        }

        // POST: Critters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,CustomerId,Name,LastWeight,CritterTypeId,Color")] Critter critter)
        {
            if (id != critter.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    critterRepo.Update(critter);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!critterRepo.DoesCritterExist(critter.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CritterTypeId"] = BuildCritterTypeSelectList(critter.CritterTypeId);
            ViewData["CustomerId"] = BuildCustomerSelectList(critter.CustomerId);
            return View(critter);
        }

        // GET: Critters/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var critter = critterRepo.FetchCritter(id.Value);
            if (critter == null)
            {
                return NotFound();
            }

            return View(critter);
        }

        // POST: Critters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var cr = critterRepo.FetchCritterNoTracking(id);
            photoRepo.Delete(cr.PhotoId);
            critterRepo.Delete(id);
            //TODO add ability to intelligently return to the correct  list view
            return RedirectToAction(nameof(Index));
        }

        private SelectList BuildCustomerSelectList(int? customerId)
        {
            IEnumerable<Customer> custList;
            if (customerId.HasValue)
            {
                var cust = customerRepo.FetchCustomer(customerId.Value);
                custList = new List<Customer> { cust };

            }
            else
            {
                custList = customerRepo.ListCustomers(ListFetchOptions.DefaultOptions);
            }
            return new SelectList(custList, "Id", "Email"); ;
        }

        private SelectList BuildCritterTypeSelectList(int id = 1)
        {
            var types = critterRepo.ListCritterTypes();
            return new SelectList(types, "Id", "Description", id);
        }

        private int InsertPhoto(IFormFile ff)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ff.CopyTo(ms);
                Photo p = new Photo
                {
                    FileName = ff.FileName,
                    ContentType = ff.ContentType,
                    PhotoFile = ms.ToArray()
                };
                var savedPhoto = photoRepo.Create(p);
                return savedPhoto.Id;
            }
        }

    }
}