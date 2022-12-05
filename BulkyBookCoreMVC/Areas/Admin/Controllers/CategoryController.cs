using BulkyBookCoreMVC.Data;
using Microsoft.AspNetCore.Mvc;

using BulkyBookCoreMVC.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebAppCore6.Areas.Admin.Controllers
{
    [Area("Admin")]
   // [Authorize]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;


        public CategoryController(ApplicationDbContext db)
        {
            _db= db;
            
        }
        public IActionResult Index()
        {
            IEnumerable<Category>? objCategoryList = _db.Categories;
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("CustomError", "The display order cannot exactly match the same");
            }

            if (ModelState.IsValid)
            {
                _db.Categories?.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Category Created Successfully";
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

            //var categoryFromDB = _db.Categories.Find(id);

            var categoryFromDB = _db.Categories?.FirstOrDefault(u=>u.Id==id);
            //
            if (categoryFromDB == null)
            {
                return NotFound();
            }

            return View(categoryFromDB);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("CustomError", "The display order cannot exactly match the same");
            }

            if (ModelState.IsValid)
            {
                _db.Categories?.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }


        public IActionResult Delete(int? id)
        {


            var categoryFromDB = _db.Categories?.FirstOrDefault(u => u.Id == id); ;

            //var categoryFromDB = _db.Categories.FirstOrDefault(c => c.Id==id);
            //
            if (categoryFromDB == null)
            {
                return NotFound();
            }

            return View(categoryFromDB);
        }

        public IActionResult DeletePOST(int? Id)
        {



            var obj = _db.Categories?.FirstOrDefault(u => u.Id == Id); 

            //var categoryFromDB = _db.Categories.FirstOrDefault(c => c.Id==id);
            //
            if (obj == null)
            {
                return NotFound();
            }

            _db.Categories?.Remove(obj);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
