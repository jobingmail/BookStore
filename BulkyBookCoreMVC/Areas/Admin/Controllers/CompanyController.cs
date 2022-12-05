using BulkyBookCoreMVC.Data;
using BulkyBookCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookCoreMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CompanyController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {

            IEnumerable<Company> obj = _db.Companies.ToList();
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpGet]
        public IActionResult GetAllCompany()
        {
            var companyList = _db.Companies.ToList();
            return Json(new { data = companyList });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Company obj)
        {
           


            if (ModelState.IsValid)
            {
                _db.Companies?.Add(obj);
                _db.SaveChanges();

                return RedirectToAction("Index");

            }

            return View(obj);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {

            if (id == null || id == 0)
            {
                return NotFound();
            }

            var companyFromDB = _db.Companies?.FirstOrDefault(c => c.Id == id);

            
            if (companyFromDB == null)
            {
                return NotFound();
            }

            return View(companyFromDB);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Company obj)
        {
            

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


            var companyFromDB = _db.Companies?.FirstOrDefault(c => c.Id == id);


            if (companyFromDB == null)
            {
                return NotFound();
            }

            return View(companyFromDB);
        }

        public IActionResult DeletePOST(int? Id)
        {



            var obj = _db.Companies?.FirstOrDefault(c => c.Id == Id);

            //var companyFromDB = _db.Companys.FirstOrDefault(c => c.Id==id);
            //
            if (obj == null)
            {
                return NotFound();
            }

            _db.Companies?.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Company Deleted Successfully";

            return RedirectToAction("Index");
        }
    }
}
