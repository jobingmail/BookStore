using BulkyBookCoreMVC.Data;
using BulkyBookCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookCoreMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CoverTypeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {

            IEnumerable<CoverType> obj = _db.CoverTypes.ToList();
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }



        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())  //custom validation example
            {
                ModelState.AddModelError("CustomError", "The display order cannot exactly match the same");
            }


            if (ModelState.IsValid)
            {
                _db.CoverTypes?.Add(obj);
                _db.SaveChanges();

                return RedirectToAction("Index");

            }

            return View(obj);
        }


        public IActionResult Edit(int? id)
        {

            if (id == null || id == 0)
            {
                return NotFound();
            }

            var categoryFromDB = _db.CoverTypes?.FirstOrDefault(c => c.Id == id);

            
            if (categoryFromDB == null)
            {
                return NotFound();
            }

            return View(categoryFromDB);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("CustomError", "The display order cannot exactly match the same");
            }

            if (ModelState.IsValid)
            {
                
                _db.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }


        public IActionResult Delete(int? id)
        {


            var categoryFromDB = _db.CoverTypes?.FirstOrDefault(c => c.Id == id);


            if (categoryFromDB == null)
            {
                return NotFound();
            }

            return View(categoryFromDB);
        }

        public IActionResult DeletePOST(int? Id)
        {



            var obj = _db.CoverTypes?.FirstOrDefault(c => c.Id == Id);

            //var categoryFromDB = _db.CoverTypes.FirstOrDefault(c => c.Id==id);
            //
            if (obj == null)
            {
                return NotFound();
            }

            _db.CoverTypes?.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "CoverType Deleted Successfully";

            return RedirectToAction("Index");
        }
    }
}
