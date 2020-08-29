using System;
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
using VetDesk.Repository;

namespace VetDesk.Views
{
    public class CrittersController : Controller
    {
        private readonly ICritterRepository critterRepo;
        private readonly ICustomerRepository customerRepo;

        public CrittersController(ICustomerRepository custRepo, ICritterRepository critRepo)
        {
            critterRepo = critRepo;
            customerRepo = custRepo;
        }

        public IActionResult GetCritterImage(int? critterId)
        {
            if (!critterId.HasValue)
                return NotFound();

            var crit = critterRepo.FetchCritter(critterId.Value);
            return File(crit.Photo, "image/jpeg");

        }

        // GET: Critters
        public IActionResult Index()
        {
            var q = critterRepo.ListCritters(ListFetchOptions.DefaultOptions);
            return View(q.ToList());
        }

        // GET: Critters/Details/5
        public IActionResult Details(int? id)
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
        public IActionResult Create([Bind("Id,CustomerId,Name,LastWeight,CritterTypeId,Color,Photo")] Critter critter,
            IFormFile photo)
        {
            if (ModelState.IsValid)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    photo.CopyTo(ms);
                    critter.Photo = ms.ToArray();
                }
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
        public IActionResult Edit(int id, [Bind("Id,CustomerId,Name,LastWeight,CritterTypeId,Color,Photo")] Critter critter)
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
            critterRepo.Delete(id);
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

    }
}
